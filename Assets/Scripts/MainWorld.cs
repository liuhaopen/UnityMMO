using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO{
    public class MainWorld : MonoBehaviour
    {
        private MainWorld(){}
        public static MainWorld Instance = null;
        GameWorld m_GameWorld;

        private void Awake() {
            Instance = this;
            Initialize();
        }

        public void Initialize() {
            m_GameWorld = new GameWorld("ClientWorld");
            SceneMgr.Instance.InitArcheType();
            SynchFromNet.Instance.Init();


        }

        public void StartGame() {
            //目前只有一个场景（本来就想做成无限大世界的）
            SceneMgr.Instance.LoadScene(1001);
            //开始从后端请求场景信息，一旦开启就会在收到回复时再次请求
            SynchFromNet.Instance.ReqSceneObjInfoChange();

        }

        private void Update() {
            
        }
        
        // void TestLoadMultipleNavMeshInRunTime()
        // {
        //     XLuaFramework.ResourceManager.GetInstance().LoadNavMesh("Test1");
        //     XLuaFramework.ResourceManager.GetInstance().LoadNavMesh("Test2");
        //     AsyncOperation asy = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Test1", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        //     AsyncOperation asy2 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Test2", UnityEngine.SceneManagement.LoadSceneMode.Additive); 
        // }

    }

}