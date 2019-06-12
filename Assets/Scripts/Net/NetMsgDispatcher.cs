using System;
using System.Collections.Generic;
using System.IO;
using Sproto;
using SprotoType;
using UnityEngine;
using XLuaFramework;

namespace UnityMMO
{
    public delegate SprotoTypeBase RpcReqHandler(SprotoTypeBase rpcReq);
    public delegate void RpcRspHandler(SprotoTypeBase rpcRsp);

    //send and receive game play msg from server
    public class NetMsgDispatcher
    {
        static NetMsgDispatcher _instance = null;
        private static ProtocolFunctionDictionary protocol = Protocol.Instance.Protocol;
        private static Dictionary<long, RpcRspHandler> rpcRspHandlerDict;
        private static Dictionary<long, ProtocolFunctionDictionary.typeFunc> sessionDict;
        private int session = 0;
        private int maxSession = GameConst.MinLuaNetSessionID;
        private NetMsgDispatcher(){}
        public static NetMsgDispatcher GetInstance()
        {
            if (_instance != null)
                return _instance;
            _instance = new NetMsgDispatcher();
            return _instance;
        }

        public void Init()
        {
            rpcRspHandlerDict = new Dictionary<long, RpcRspHandler>();
            sessionDict = new Dictionary<long, ProtocolFunctionDictionary.typeFunc>();
            NetworkManager.GetInstance().OnReceiveMsgCallBack += OnReceiveMsgFromNet;

        }

        private static void AddHandler(long session, RpcRspHandler rpcRspHandler)
        {
            rpcRspHandlerDict.Add(session, rpcRspHandler);
        }

        private static void RemoveHandler(long session)
        {
            if (rpcRspHandlerDict.ContainsKey(session))
            {
                rpcRspHandlerDict.Remove(session);
            }
        }

        public static RpcRspHandler GetHandler(long session)
        {
            RpcRspHandler rpcRspHandler;
            rpcRspHandlerDict.TryGetValue(session, out rpcRspHandler);
            RemoveHandler(session);
            return rpcRspHandler;
        }

        public void OnReceiveMsgFromNet(byte[] bytes)
        {
            // Debug.Log("NetMsgHandler:OnReceiveMsgFromNet : "+bytes.Length.ToString());
            int content_size = bytes.Length-5;
            if (content_size <= 0)
                return;
            char flag = BitConverter.ToChar(bytes, content_size);
            int cur_session = BitConverter.ToInt32(bytes, content_size+1);
            cur_session = Util.SwapInt32(cur_session);
            // Debug.Log("NetMsgHandler:OnReceiveMsgFromNet flag:"+flag.ToString()+" session:"+cur_session.ToString());

            RpcRspHandler rpcRspHandler = NetMsgDispatcher.GetHandler(cur_session);
            if (rpcRspHandler != null)
            {
                SprotoTypeBase receive_info = null;
                ProtocolFunctionDictionary.typeFunc GenResponse;
                sessionDict.TryGetValue(cur_session, out GenResponse);
                // if (flag == 1)
                // {
                    receive_info = GenResponse(bytes, 0, content_size);
                // }
                // else
                // {
                //     receive_info = GenResponse(null, 0, 0); 
                // }
                rpcRspHandler(receive_info);
            }
        }
        
        public void SendMessage<T>(SprotoTypeBase rpcReq, RpcRspHandler rpcRspHandler = null) 
        {
            session += 1;
            if (session >= maxSession)
                session = 0;
            if (rpcRspHandler != null)
            {
                AddHandler(session, rpcRspHandler);
            }
            byte[] message = rpcReq.encode();
            MemoryStream ms = null;
            using (ms = new MemoryStream())
            {
                ms.Position = 0;
                BinaryWriter writer = new BinaryWriter(ms);
                UInt16 msglen = Util.SwapUInt16((UInt16)(message.Length+8));
                writer.Write(msglen);
                int tag = protocol[typeof(T)];
                sessionDict.Add((long)session, protocol[tag].Response.Value);
                tag = Util.SwapInt32(tag);
                writer.Write(tag);
                writer.Write(message);
                int swap_session = Util.SwapInt32(session);
                writer.Write(swap_session);
                writer.Flush();
                byte[] payload = ms.ToArray();
                NetworkManager.GetInstance().SendBytesWithoutSize(payload);
            }
        }

        public void ListenMessage<T>(SprotoTypeBase rpcReq, RpcRspHandler rpcRspHandler = null) 
        {
            RpcRspHandler on_ack = null;
            on_ack = (SprotoTypeBase result)=>{
                rpcRspHandler(result);
                this.SendMessage<T>(rpcReq, on_ack);
            };
            this.SendMessage<T>(rpcReq, on_ack);
        }
    }
}