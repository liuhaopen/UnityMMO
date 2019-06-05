using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using XLua;

namespace XLuaFramework {
    using NetEventType = KeyValuePair<Action<byte[]>, byte[]>;
    
    public enum DisType 
    {
        Exception,
        Disconnect,
    }

    //网络包的类型
    [Hotfix]
    [LuaCallCSharp]
    public enum NetPackageType
    {
        BaseLine=1,//基于行的解析
        BaseHead=2,//头两字节为包大小
    }

    [Hotfix]
    [LuaCallCSharp]
    public class NetworkManager : MonoBehaviour 
    {
        static NetworkManager _instance;
        static readonly object m_lockObject = new object();
        static Queue<NetEventType> mEvents = new Queue<NetEventType>();
        private TcpClient client = null;
        private NetworkStream outStream = null;
        private MemoryStream memStream;
        private BinaryReader reader;
        private NetPackageType curPackageType = NetPackageType.BaseLine;
        private const int MAX_READ = 8192;
        // private int session = 0;
        // private int maxSession = int.MaxValue/2;
        private byte[] byteBuffer = new byte[MAX_READ];
        // public static bool loggedIn = false;
        Action<byte[]> onConnectCallBack = null;
        Action<byte[]> onDisConnectCallBack = null;
        Action<byte[]> onReceiveLineCallBack = null;
        Action<byte[]> onReceiveMsgCallBack = null;
        public Action<byte[]> OnConnectCallBack
        {
            get
            {
                return onConnectCallBack;
            }
            set
            {
                onConnectCallBack = value;
            }
        }
        public Action<byte[]> OnDisConnectCallBack
        {
            get
            {
                return onDisConnectCallBack;
            }
            set
            {
                onDisConnectCallBack = value;
            }
        }
        public Action<byte[]> OnReceiveLineCallBack
        {
            get
            {
                return onReceiveLineCallBack;
            }
            set
            {
                onReceiveLineCallBack = value;
            }
        }
        public Action<byte[]> OnReceiveMsgCallBack
        {
            get
            {
                return onReceiveMsgCallBack;
            }
            set
            {
                onReceiveMsgCallBack = value;
            }
        }

        public static NetworkManager GetInstance()
        {
            return _instance;
        }

        void Awake() 
        {
            _instance = this;
            Init();
        }

        void Init() 
        {
            memStream = new MemoryStream();
            reader = new BinaryReader(memStream);
        }

        public static void AddEvent(Action<byte[]> _event, byte[] data) 
        {
            lock (m_lockObject) 
            {
                mEvents.Enqueue(new NetEventType(_event, data));
            }
        }

        void Update() {
            if (mEvents.Count > 0) 
            {
                while (mEvents.Count > 0) 
                {
                    NetEventType _event = mEvents.Dequeue();
                    _event.Key(_event.Value);
                }
            }
        }

        public void SendConnect(string host, int port, NetPackageType type)
        {
            Debug.Log("host : " + host + " port:" + port.ToString() + " type:"+ type.ToString());
            this.Close(); 
            curPackageType = type;
            try 
            {
                IPAddress[] address = Dns.GetHostAddresses(host);
                if (address.Length == 0)
                {
                    Debug.LogError("host invalid");
                    return;
                }
                if (address[0].AddressFamily == AddressFamily.InterNetworkV6)
                {
                    client = new TcpClient(AddressFamily.InterNetworkV6);
                }
                else 
                {
                    client = new TcpClient(AddressFamily.InterNetwork);
                }
                client.SendTimeout = 1000;
                client.ReceiveTimeout = 1000;
                client.NoDelay = true;
                Debug.Log("begin connect socket");
                client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
            } 
            catch (Exception e) 
            {
                Debug.Log("begin connect socket error");
                this.Close(); 
                Debug.LogError(e.Message);
            }
        }

        void OnConnect(IAsyncResult asr) {
            Debug.Log("on connect : "+ client.Connected.ToString());
            if (!client.Connected)
            {
                Close();
                AddEvent(onDisConnectCallBack, null);
                return;
            }
            outStream = client.GetStream();
            if (curPackageType == NetPackageType.BaseLine)
            {
                client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
            }
            else
            {
                client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
            }
            Debug.Log("onConnectCallBack : "+(onConnectCallBack!=null).ToString());
            AddEvent(onConnectCallBack, null);
        }
       
        public void SendBytes(byte[] message) {
            MemoryStream ms = null;
            using (ms = new MemoryStream())
            {
                ms.Position = 0;
                BinaryWriter writer = new BinaryWriter(ms);
                if (curPackageType == NetPackageType.BaseHead)
                {
                    UInt16 msglen = Util.SwapUInt16((UInt16)(message.Length));
                    writer.Write(msglen);
                }
                writer.Write(message);
                writer.Flush();
                if (client != null && client.Connected) 
                {
                    byte[] payload = ms.ToArray();
                    outStream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
                } 
                else 
                {
                    Debug.LogError("client.connected----->>false");
                }
            }
        }

        public void SendBytesWithoutSize(byte[] message) 
        {
            if (client != null && client.Connected) 
            {
                outStream.BeginWrite(message, 0, message.Length, new AsyncCallback(OnWrite), null);
            }
            else
                Debug.LogError("SendBytesWithoutSize failed:connected----->>false");
        }

        void OnRead(IAsyncResult asr) 
        {
            int bytesRead = 0;
            try 
            {
                lock (client.GetStream()) 
                {         //读取字节流到缓冲区
                    bytesRead = client.GetStream().EndRead(asr);
                }
                if (bytesRead < 1) 
                {                //包尺寸有问题，断线处理
                    OnDisconnected(DisType.Disconnect, "bytesRead < 1");
                    return;
                }
                OnReceive(byteBuffer, bytesRead);   //分析数据包内容，抛给逻辑层
                lock (client.GetStream()) 
                {         //分析完，再次监听服务器发过来的新消息
                    Array.Clear(byteBuffer, 0, byteBuffer.Length);   //清空数组
                    client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
                }
            } 
            catch (Exception ex) 
            {
                OnDisconnected(DisType.Exception, ex.Message);
            }
        }

        void OnReadLine(IAsyncResult asr)
        {
            int bytesRead = 0;
            try
            {
                lock (client.GetStream())
                {         
                    //读取字节流到缓冲区
                    bytesRead = client.GetStream().EndRead(asr);
                }
                if (bytesRead < 1)
                {      
                    //包尺寸有问题，断线处理
                    Debug.Log("net manager read empty!");
                    OnDisconnected(DisType.Disconnect, "bytesRead < 1");
                    return;
                }
                OnReceiveLine(byteBuffer, bytesRead);   //分析数据包内容，抛给逻辑层
                lock (client.GetStream())
                {         //分析完，再次监听服务器发过来的新消息
                    Array.Clear(byteBuffer, 0, byteBuffer.Length);   //清空数组
                    client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
                }
            }
            catch (Exception ex)
            {
                //PrintBytes();
                Debug.Log("net manager GetStream exeption!");
                OnDisconnected(DisType.Exception, ex.Message);
            }
        }

        void OnDisconnected(DisType dis, string msg) 
        {
            Close();   //关掉客户端链接
            Debug.Log("networkmanager on disconnect!" + msg + " trace:" + new System.Diagnostics.StackTrace().ToString());
            AddEvent(onDisConnectCallBack, null);
        }

        void PrintBytes() 
        {
            string returnStr = string.Empty;
            for (int i = 0; i < byteBuffer.Length; i++) 
            {
                returnStr += byteBuffer[i].ToString("X2");
            }
            Debug.LogError(returnStr);
        }

        void OnWrite(IAsyncResult r) 
        {
            try 
            {
                outStream.EndWrite(r);
            } 
            catch (Exception ex) 
            {
                Debug.LogError("OnWrite--->>>" + ex.Message);
            }
        }

        void OnReceiveLine(byte[] bytes, int length)
        {
            int line_start_index = 0;
            for (int i = 0; i < length; i++)
            {
                if (bytes[i] == (int)'\n')
                {
                    int can_read_len = i - line_start_index;
                    if (can_read_len > 0)
                    {
                        long msg_len = memStream.Length + can_read_len;
                        memStream.Seek(0, SeekOrigin.End);
                        memStream.Write(bytes, line_start_index, can_read_len);
                        memStream.Seek(0, SeekOrigin.Begin);
                        OnReceivedMessageLine(reader.ReadBytes((int)msg_len));
                        memStream.SetLength(0);
                    }
                    line_start_index = i + 1;
                }
            }
            int left_len = length - line_start_index;
            if (left_len > 0)
            {
                memStream.Seek(0, SeekOrigin.End);
                memStream.Write(bytes, line_start_index, left_len);
            }
        }

        void OnReceive(byte[] bytes, int length)
        {
            memStream.Seek(0, SeekOrigin.End);
            memStream.Write(bytes, 0, length);
            memStream.Seek(0, SeekOrigin.Begin);
            //Debug.Log("on receive RemainingBytes len : " + RemainingBytes().ToString()+ "  length len:" + length.ToString());
            while (RemainingBytes() > 2)
            {
                ushort messageLen = reader.ReadUInt16();
                messageLen = Util.SwapUInt16((UInt16)(messageLen));
                if (RemainingBytes() >= messageLen)
                {
                    OnReceivedMessage(reader.ReadBytes((int)messageLen));
                }
                else
                {
                    memStream.Position = memStream.Position - 2;
                    break;
                }
            }
            //Create a new stream with any leftover bytes
            byte[] leftover = reader.ReadBytes((int)RemainingBytes());
            memStream.SetLength(0);     //Clear
            memStream.Write(leftover, 0, leftover.Length);
        }

        long RemainingBytes() 
        {
            return memStream.Length - memStream.Position;
        }

        void OnReceivedMessageLine(byte[] cmd_byte)
        {
            AddEvent(onReceiveLineCallBack, cmd_byte);
        }
        
        void OnReceivedMessage(byte[] cmd_byte)
        {
            AddEvent(onReceiveMsgCallBack, cmd_byte);
        }

        void Close() 
        {
            if (client != null) 
            {
                if (client.Connected) 
                    client.Close();
                client = null;
            }
            // loggedIn = false;
        }

        void OnDestroy() 
        {
            onConnectCallBack = null;
            onDisConnectCallBack = null;
            onReceiveLineCallBack = null;
            onReceiveMsgCallBack = null;
            if (client != null)
            {
                if (client.Connected) client.Close();
                client = null;
            }
            // loggedIn = false;
            reader.Close();
            memStream.Close();
            Debug.Log("~NetworkManager was destroy");
        }
    }
}