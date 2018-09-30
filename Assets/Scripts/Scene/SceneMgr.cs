using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using XLua;

namespace UnityMMO
{
    
[Hotfix]
[LuaCallCSharp]
public class SceneMgr : MonoBehaviour
{
	public static SceneMgr Instance;
    public Transform container;
	EntityManager entityManager;
    public EntityArchetype RoleArchetype;
    public EntityArchetype MonsterArchetype;
    public EntityArchetype NPCArchetype;

    Dictionary<long, Entity> entityDic;
    Entity mainRole;
    public SceneDetectorBase detector;
    private SceneObjectLoadController m_Controller;
    const string SceneInfoPath = "Assets/AssetBundleRes/scene/";

    public EntityManager EntityManager { get => entityManager; set => entityManager = value; }

    public void Awake()
	{
		Instance = this; // worst singleton ever but it works
		EntityManager = World.Active.GetExistingManager<EntityManager>();
        entityDic = new Dictionary<long, Entity>();
	}

    void Start()
    {
    }

    void Update()
    {
        if (detector != null && m_Controller != null)
            m_Controller.RefreshDetector(detector);
    }

	public void InitArcheType()
	{
		RoleArchetype = EntityManager.CreateArchetype(
                typeof(Position),typeof(TargetPosition),
                typeof(MoveSpeed));
	}

	public void OnDestroy()
	{
		Instance = null;
	}

    public void LoadScene(int scene_id, float pos_x=0.0f, float pos_y=0.0f, float pos_z=0.0f)
    {
        Debug.Log("LoadScene scene_id "+(scene_id).ToString());
        //load scene info from json file(which export from SceneInfoExporter.cs)
        string scene_json = File.ReadAllText(SceneInfoPath+"scene_"+scene_id.ToString()+"/scene_info.json", Encoding.UTF8);
        SceneInfo scene_info;
        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(scene_json)))
        {
            DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(SceneInfo));
            scene_info = (SceneInfo)deseralizer.ReadObject(ms);// //反序列化ReadObject
        }

        m_Controller = gameObject.GetComponent<SceneObjectLoadController>();
        if (m_Controller == null)
            m_Controller = gameObject.AddComponent<SceneObjectLoadController>();

        int max_create_num = 9;
        int min_create_num = 0;
        m_Controller.Init(scene_info.Bounds.center, scene_info.Bounds.size, true, max_create_num, min_create_num, SceneSeparateTreeType.QuadTree);

        Debug.Log("scene_info.ObjectInfoList.Count : "+scene_info.ObjectInfoList.Count.ToString());
        for (int i = 0; i < scene_info.ObjectInfoList.Count; i++)
        {
            m_Controller.AddSceneBlockObject(scene_info.ObjectInfoList[i]);
        }
    }

    private void LoadSceneObjectCollidersInfo(int scene_id)
    {}

    public Entity AddMainRole(long uid)
	{
		Entity role = AddRole(uid);
        EntityManager.AddComponent(role, ComponentType.Create<PlayerInput>());
        EntityManager.AddComponent(role, ComponentType.Create<SynchPosFlag>());
        entityDic.Add(uid, role);
        mainRole = role;
        return role;
	}

    public Entity AddRole(long uid)
	{
		Entity role = EntityManager.CreateEntity(RoleArchetype);
        EntityManager.SetComponentData(role, new Position {Value = new int3(0, 0, 0)});
        EntityManager.SetComponentData(role, new MoveSpeed {speed = 12});
        EntityManager.SetComponentData(role, new TargetPosition {Value = new int3(0, 0, 0)});
        EntityManager.AddSharedComponentData(role, GetLookFromPrototype("Prototype/MainRoleRenderPrototype"));
        entityDic.Add(uid, role);
        return role;
	}

    public Entity AddSceneObject(long uid, SceneObjectType type)
    {
        if (type == SceneObjectType.Role)
            return AddRole(uid);
        return Entity.Null;
    }

    public void RemoveSceneObject(long uid)
    {
        Entity entity = GetSceneObject(uid);
        if (entity!=Entity.Null)
            entityManager.DestroyEntity(entity);
    }

    public Entity GetSceneObject(long uid)
    {
        Debug.Log("GetSceneObject uid"+uid.ToString()+" ContainsKey:"+entityDic.ContainsKey(uid).ToString());
        if (entityDic.ContainsKey(uid))
            return entityDic[uid];
        return Entity.Null;
    }

	private MeshInstanceRenderer GetLookFromPrototype(string protoName)
    {
        var proto = GameObject.Find(protoName);
        var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
        // Object.Destroy(proto);
        return result;
    }
}

}