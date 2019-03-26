using Unity.Entities;
using Unity.Mathematics;
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
        }

        public void Initialize() {
            m_GameWorld = new GameWorld("ClientWorld");
            TimelineManager.GetInstance().Init();
            SceneMgr.Instance.Init(m_GameWorld);
            SynchFromNet.Instance.Init();

            InitializeSystems();
        }

        public void InitializeSystems() {
            m_Systems = new SystemCollection();
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<PlayerInputSystem>());
            
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<HandleRoleLooks>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<HandleRoleLooksNetRequest>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<HandleRoleLooksSpawnRequests>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<CreateTargetPosFromUserInputSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<MovementUpdateSystem>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<HandleMovementQueries>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<GroundTestSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<UploadMainRolePosSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<SkillSpawnSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<TimelineSpawnSystem>(m_GameWorld));
            
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateManager<UpdateRoleAnimatorSystem>(m_GameWorld));

            // TimelineSpawnRequest.Create(m_GameWorld.GetEntityManager(), Entity.Null, "haha");
        }

        public void StartGame() {
            Initialize();
            if (GameVariable.IsSingleMode)
            {
                //目前只有一个场景（本来就想做成无限大世界的）
                SceneMgr.Instance.AddMainRole(1, "testRole", 2, Vector3.zero);
                SceneMgr.Instance.LoadScene(1001);
            }
            else
            {
                //开始从后端请求场景信息，一旦开启就会在收到回复时再次请求
                SynchFromNet.Instance.StartSynchFromNet();
            }
            // TestLoadMultipleNavMeshInRunTime();
        }

        private void Update() {
            // Debug.Log("main world update");
            m_Systems.Update();
            GameInput.GetInstance().Reset();
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