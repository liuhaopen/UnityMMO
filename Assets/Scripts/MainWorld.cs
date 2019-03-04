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
        SystemCollection m_Systems;

        private void Awake() {
            Instance = this;
            Initialize();
        }

        public void Initialize() {
            m_GameWorld = new GameWorld("ClientWorld");
            SceneMgr.Instance.Init(m_GameWorld);
            SynchFromNet.Instance.Init();

            InitializeSystems();
        }

        public void InitializeSystems() {
            m_Systems = new SystemCollection();
            // m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<PlayerInputSystem>());
            
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<HandleRoleLooks>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<HandleRoleLooksNetRequest>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<HandleRoleLooksSpawnRequests>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<UpdateRoleTransformFromLooks>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<TargetPosSystem>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<UploadMainRolePosSystem>(m_GameWorld));

            
        }

        public void StartGame() {
            if (GameVariable.IsSingleMode)
            {
                //目前只有一个场景（本来就想做成无限大世界的）
                SceneMgr.Instance.AddMainRole(1, Vector3.zero);
                SceneMgr.Instance.LoadScene(1001);
            }
            else
            {
                //开始从后端请求场景信息，一旦开启就会在收到回复时再次请求
                SynchFromNet.Instance.ReqSceneObjInfoChange();
            }
            // TestLoadMultipleNavMeshInRunTime();
        }

        private void Update() {
            // Debug.Log("main world update");
            m_Systems.Update();
        }
        
        void TestLoadMultipleNavMeshInRunTime()
        {
            XLuaFramework.ResourceManager.GetInstance().LoadNavMesh("Test1");
            XLuaFramework.ResourceManager.GetInstance().LoadNavMesh("Test2");
            AsyncOperation asy = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Test1", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            AsyncOperation asy2 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Test2", UnityEngine.SceneManagement.LoadSceneMode.Additive); 
        }

    }

}