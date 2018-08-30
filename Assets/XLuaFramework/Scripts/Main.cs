using UnityEngine;
using System.Collections;
using XLua;

namespace XLuaFramework {

    //负责整个游戏流程调度,从启动后热更新,渠道sdk接入,各系统的初始化,直到登录才完成使命
    public class Main : MonoBehaviour {
        public enum State{
            None,
            UpdateResourceFromNet,
            InitAssetBundle,
            StartGame,
        }
        public enum SubState{
            Enter,Update
        }
        State cur_state = State.None;
        SubState cur_sub_state = SubState.Enter;
        public bool IsNeedPrintConcole = true;
        void Start() {
            Debug.Log("Main:Start()");

            AppConfig.Init();

            GameObject.DontDestroyOnLoad(this.gameObject);

            if (IsNeedPrintConcole)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android)
                    this.gameObject.AddComponent<LogHandler>();
            }

            this.gameObject.AddComponent<ResourceManager>();
            this.gameObject.AddComponent<NetworkManager>();
            UnityMMO.NetMsgDispatcher.GetInstance().Init();

            JumpToState(State.InitAssetBundle);
        }

        void JumpToState(State new_state)
        {
            cur_state = new_state;
            cur_sub_state = SubState.Enter;
        }

        private void Update() {
            switch(cur_state)
            {
                case State.InitAssetBundle:
                    if (cur_sub_state == SubState.Enter)
                    {
                        ResourceManager.GetInstance().Initialize(AppConfig.AssetDir, delegate() {
                            Debug.Log("ResourceManager Initialize OK!!!");
                            JumpToState(State.StartGame);
                        });
                        cur_sub_state = SubState.Update;
                    }
                    break;
                case State.StartGame:
                    if (cur_sub_state == SubState.Enter)
                    {
                        this.gameObject.AddComponent<XLuaManager>();
                        UnityMMO.MainWorld.GetInstance().Initialize();
                        UnityMMO.MainWorld.GetInstance().StartGame();
                        cur_sub_state = SubState.Update;
                    }
                    break;
            }
        }
    }
}