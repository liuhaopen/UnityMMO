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
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<PlayerInputSystem>());
            
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<HandleRoleLooks>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<HandleRoleLooksNetRequest>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<HandleRoleLooksSpawnRequests>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<CreateTargetPosFromUserInputSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<MovementUpdateSystem>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<HandleMovementQueries>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<MovementHandleGroundCollision>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<GroundTestSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<UploadMainRolePosSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<SkillSpawnSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<TimelineSpawnSystem>(m_GameWorld));
            
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<UpdateAnimatorSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<ResetPosOffsetSystem>(m_GameWorld));
            
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<NameboardSystem>(m_GameWorld));
            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<NameboardSpawnRequestSystem>(m_GameWorld));

            m_Systems.Add(m_GameWorld.GetECSWorld().CreateSystem<ActionDataResetSystem>(m_GameWorld));
            
        }

        public void StartGame() {
            Initialize();
            if (GameVariable.IsSingleMode)
            {
                //目前只有一个场景（本来就想做成无限大世界的）
                SceneMgr.Instance.AddMainRole(1, 1, "testRole", 2, Vector3.zero, 100, 100);
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
            m_GameWorld.ProcessDespawns();
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