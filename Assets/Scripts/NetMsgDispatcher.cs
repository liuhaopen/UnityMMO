using System;
using UnityEngine;
using XLuaFramework;

namespace UnityMMO
{
    //handle game play msg from net
    public class NetMsgDispatcher
    {
        static NetMsgDispatcher _instance = null;
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
            NetworkManager.GetInstance().MaxSession = GameConst.MinLuaNetSessionID;
            NetworkManager.GetInstance().OnReceiveMsgCallBack += OnReceiveMsgFromNet;
        }
        public void OnReceiveMsgFromNet(byte[] bytes)
        {
            // Debug.Log("NetMsgHandler:OnReceiveMsgFromNet : "+bytes.Length.ToString());
            int content_size = bytes.Length-5;
            // BitConverter.ToString()
            // int tag = BitConverter.ToInt32(bytes, 0);
        }
    }
}