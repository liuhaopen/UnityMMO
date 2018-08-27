using System;
using System.Collections.Generic;
using System.IO;
using Sproto;
using UnityEngine;
using XLuaFramework;
// using static Protocol;

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
        private int maxSession = int.MaxValue/2;
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
            NetworkManager.GetInstance().MaxSession = GameConst.MinLuaNetSessionID;
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
            // int tag = BitConverter.ToInt32(bytes, 0);
            
            char flag = BitConverter.ToChar(bytes, content_size+1);

            RpcRspHandler rpcRspHandler = NetMsgDispatcher.GetHandler(session);
            if (rpcRspHandler != null)
            {
                ProtocolFunctionDictionary.typeFunc GenResponse;
                sessionDict.TryGetValue(session, out GenResponse);
                rpcRspHandler(GenResponse(bytes, 0));
            }
        }
        
        public void SendMessage<T>(SprotoTypeBase rpcReq, RpcRspHandler rpcRspHandler = null) {
            if (rpcRspHandler != null)
            {
                session += 1;
                if (session > maxSession)
                    session = 0;
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
                tag = Util.SwapInt32(tag);
                sessionDict.Add((long)session, protocol[tag].Response.Value);
                writer.Write(tag);
                writer.Write(message);
                session = Util.SwapInt32(session);
                writer.Write(session);
                writer.Flush();
                byte[] payload = ms.ToArray();
                NetworkManager.GetInstance().SendBytesWithoutSize(payload);
            }
        }
    }
}