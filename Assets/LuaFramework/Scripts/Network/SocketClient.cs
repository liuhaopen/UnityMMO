using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using LuaFramework;

public enum DisType {
    Exception,
    Disconnect,
}
//网络包的类型
public enum NetPackageType
{
    BaseLine=1,//基于行的解析
    BaseHead=2,//头两字节为包大小
}


public class SocketClient {
   
    private TcpClient client = null;
    private NetworkStream outStream = null;
    private MemoryStream memStream;
    private BinaryReader reader;
    private NetPackageType curPackageType = NetPackageType.BaseLine;

    private const int MAX_READ = 8192;
    private byte[] byteBuffer = new byte[MAX_READ];
    public static bool loggedIn = false;

    // Use this for initialization
    public SocketClient() {
    }

    /// <summary>
    /// 注册代理
    /// </summary>
    public void OnRegister() {
        memStream = new MemoryStream();
        reader = new BinaryReader(memStream);
    }

    /// <summary>
    /// 移除代理
    /// </summary>
    public void OnRemove() {
        this.Close();
        reader.Close();
        memStream.Close();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    public void ConnectServer(string host, int port, NetPackageType type) {
        Debug.Log("host : " + host + " port:" + port.ToCString() + " type:"+ type.ToString());
        client = null;
        curPackageType = type;
        try {
            IPAddress[] address = Dns.GetHostAddresses(host);
            Debug.Log("address.Length : " + address.Length.ToCString());
            if (address.Length == 0) {
                Debug.LogError("host invalid");
                return;
            }
            if (address[0].AddressFamily == AddressFamily.InterNetworkV6) {
                client = new TcpClient(AddressFamily.InterNetworkV6);
            }
            else {
                client = new TcpClient(AddressFamily.InterNetwork);
            }
            client.SendTimeout = 1000;
            client.ReceiveTimeout = 1000;
            client.NoDelay = true;
            Debug.Log("begin connect socket");
            client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
        } catch (Exception e) {
            Debug.Log("begin connect socket error");
            Close(); Debug.LogError(e.Message);
        }
    }

    /// <summary>
    /// 连接上服务器
    /// </summary>
    void OnConnect(IAsyncResult asr) {
        Debug.Log("on connect : "+ client.Connected.ToString());
        outStream = client.GetStream();
        if (curPackageType == NetPackageType.BaseLine)
        {
            client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
        }
        else
        {
            client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
        }

        NetworkManager.AddEvent(Protocal.Connect, new ByteBuffer());
    }

    public static ushort SwapUInt16(ushort n)
    {
        //Debug.Log("SwapUInt16() BitConverter.IsLittleEndian : " + BitConverter.IsLittleEndian.ToCString());
        if (BitConverter.IsLittleEndian)
            return (ushort)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
        else
            return n;
    }
    /// <summary>
    /// 写数据
    /// </summary>
    public void WriteMessage(byte[] message) {
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);
            if (curPackageType == NetPackageType.BaseHead)
            {
                UInt16 msglen = SwapUInt16((UInt16)(message.Length));
                writer.Write(msglen);
            }
            writer.Write(message);
            writer.Flush();
            if (client != null && client.Connected) {
                byte[] payload = ms.ToArray();
                outStream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
            } else {
                Debug.LogError("client.connected----->>false");
            }
        }
    }

    /// <summary>
    /// 读取消息
    /// </summary>
    void OnRead(IAsyncResult asr) {
        int bytesRead = 0;
        try {
            lock (client.GetStream()) {         //读取字节流到缓冲区
                bytesRead = client.GetStream().EndRead(asr);
            }
            if (bytesRead < 1) {                //包尺寸有问题，断线处理
                OnDisconnected(DisType.Disconnect, "bytesRead < 1");
                return;
            }
            OnReceive(byteBuffer, bytesRead);   //分析数据包内容，抛给逻辑层
            lock (client.GetStream()) {         //分析完，再次监听服务器发过来的新消息
                Array.Clear(byteBuffer, 0, byteBuffer.Length);   //清空数组
                client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
            }
        } catch (Exception ex) {
            //PrintBytes();
            OnDisconnected(DisType.Exception, ex.Message);
        }
    }

    void OnReadLine(IAsyncResult asr)
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
            OnDisconnected(DisType.Exception, ex.Message);
        }
    }

    /// <summary>
    /// 丢失链接
    /// </summary>
    void OnDisconnected(DisType dis, string msg) {
        Close();   //关掉客户端链接
        int protocal = dis == DisType.Exception ?
        Protocal.Exception : Protocal.Disconnect;

        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteShort((ushort)protocal);
        NetworkManager.AddEvent(protocal, buffer);
        Debug.Log("Connection was closed by the server:>" + msg + " Distype:>" + dis);
    }

    /// <summary>
    /// 打印字节
    /// </summary>
    /// <param name="bytes"></param>
    void PrintBytes() {
        string returnStr = string.Empty;
        for (int i = 0; i < byteBuffer.Length; i++) {
            returnStr += byteBuffer[i].ToString("X2");
        }
        Debug.LogError(returnStr);
    }

    /// <summary>
    /// 向链接写入数据流
    /// </summary>
    void OnWrite(IAsyncResult r) {
        try {
            outStream.EndWrite(r);
        } catch (Exception ex) {
            Debug.LogError("OnWrite--->>>" + ex.Message);
        }
    }

    void OnReceiveLine(byte[] bytes, int length)
    {
        //long left_len = bytes.Length;
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
            //left_len -= 1;
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
        //Reset to beginning
        memStream.Seek(0, SeekOrigin.Begin);
        //Debug.Log("on receive RemainingBytes len : " + RemainingBytes().ToString()+ "  length len:" + length.ToString());
        while (RemainingBytes() > 2)
        {
            //messageLen = reader.ReadUInt32();
            //int len = Util.SwapUInt32(messageLen);
            ////Util.Log("len = " + len);
            //len -= 2;//减去4位的数据长度本身所占字节
            ushort messageLen = reader.ReadUInt16();
            messageLen = SwapUInt16((UInt16)(messageLen));
            //Debug.Log("onreceive msg len:" + messageLen.ToString());
            //messageLen -= 2;
            if (RemainingBytes() >= messageLen)
            {
                OnReceivedMessage(reader.ReadBytes((int)messageLen));
            }
            else
            {
                //Back up the position two bytes
                memStream.Position = memStream.Position - 2;
                break;
            }
        }
        //Create a new stream with any leftover bytes
        byte[] leftover = reader.ReadBytes((int)RemainingBytes());
        memStream.SetLength(0);     //Clear
        memStream.Write(leftover, 0, leftover.Length);
    }

    /// <summary>
    /// 剩余的字节
    /// </summary>
    private long RemainingBytes() {
        return memStream.Length - memStream.Position;
    }

    void OnReceivedMessageLine(byte[] cmd_byte)
    {
        ByteBuffer buffer = new ByteBuffer(cmd_byte);
        NetworkManager.AddEvent(Protocal.MessageLine, buffer);
    }
    /// <summary>
    /// 接收到消息
    /// </summary>
    /// <param name="ms"></param>
    void OnReceivedMessage(byte[] cmd_byte)
    {
        ByteBuffer buffer = new ByteBuffer(cmd_byte);
        NetworkManager.AddEvent(Protocal.Message, buffer);
    }

    /// <summary>
    /// 会话发送
    /// </summary>
    void SessionSend(byte[] bytes) {
        WriteMessage(bytes);
    }

    /// <summary>
    /// 关闭链接
    /// </summary>
    public void Close() {
        if (client != null) {
            if (client.Connected) client.Close();
            client = null;
        }
        loggedIn = false;
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    //public void SendMessage(ByteBuffer buffer) {
    //    SessionSend(buffer.ToBytes());
    //    buffer.Close();
    //}
}
