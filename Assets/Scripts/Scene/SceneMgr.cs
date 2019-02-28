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
    GameWorld m_GameWorld;
    // public Transform container;
	// EntityManager entityManager;
    // public EntityArchetype RoleArchetype;
    public EntityArchetype MonsterArchetype;
    public EntityArchetype NPCArchetype;
    Dictionary<long, Entity> entityDic;
    Entity mainRole;
    public SceneDetectorBase detector;
    private SceneObjectLoadController m_Controller;
    const string SceneInfoPath = "Assets/AssetBundleRes/scene/";

    // GameObject mainRolePrefab;
    // GameObject rolePrefab;

    Dictionary<string, GameObject> prefabDic;

    public EntityManager EntityManager { get => m_GameWorld.GetEntityManager();}

    public void Awake()
	{
		Instance = this; // worst singleton ever but it works
		// EntityManager = World.Active.GetExistingManager<EntityManager>();
        entityDic = new Dictionary<long, Entity>();
        prefabDic = new Dictionary<string, GameObject>();
	}

    void Update()
    {
        // Debug.Log("detector : "+(detector!=null).ToString()+" cont : "+(m_Controller != null).ToString());
        if (detector != null && m_Controller != null)
            m_Controller.RefreshDetector(detector);
    }

	public void Init(GameWorld world)
	{
        m_GameWorld = world;

        RoleMgr.GetInstance().Init(world);
        // LoadPrefab("Assets/AssetBundleRes/role/prefab/MainRole.prefab", "MainRole");
        // LoadPrefab("Assets/AssetBundleRes/role/prefab/Role.prefab", "Role");

		// RoleArchetype = EntityManager.CreateArchetype(
        //         typeof(Position),typeof(TargetPosition),
        //         typeof(MoveSpeed));

        NPCArchetype = EntityManager.CreateArchetype(
                typeof(Position));
	}

    void LoadPrefab(string path, string storePrefabName)
    {
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(path, delegate(UnityEngine.Object[] objs) {
            if (objs.Length > 0 && (objs[0] as GameObject)!=null)
            {
                GameObject prefab = objs[0] as GameObject;
                if (prefab != null) 
                {
                    this.prefabDic[storePrefabName] = prefab;
                    return;
                }
            }
            Debug.LogError("cannot find prefab in "+path);
        });
    }

	public void OnDestroy()
	{
		Instance = null;
	}

    private static string Repalce(string str)
    {
        str = System.Text.RegularExpressions.Regex.Replace(str, @"<", "lt;");
        str = System.Text.RegularExpressions.Regex.Replace(str, @">", "gt;");
        // str = System.Text.RegularExpressions.Regex.Replace(str, @"\\", "quot;");
        str = System.Text.RegularExpressions.Regex.Replace(str, @"\r", "");
        str = System.Text.RegularExpressions.Regex.Replace(str, @"\n", "");
        str = System.Text.RegularExpressions.Regex.Replace(str, @"\/", "/");
        return str;
    }

    public void LoadScene(int scene_id, float pos_x=0.0f, float pos_y=0.0f, float pos_z=0.0f)
    {
        Debug.Log("LoadScene scene_id "+(scene_id).ToString());
        //load scene info from json file(which export from SceneInfoExporter.cs)
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<TextAsset>(SceneInfoPath+"scene_"+scene_id.ToString() +"/scene_info.json", delegate(UnityEngine.Object[] objs) {
            TextAsset txt = objs[0] as TextAsset;
            string scene_json = txt.text;
            scene_json = Repalce(scene_json);
            SceneInfo scene_info = JsonUtility.FromJson<SceneInfo>(scene_json);
            ApplyLightInfo(scene_info);
            
            m_Controller = gameObject.GetComponent<SceneObjectLoadController>();
            if (m_Controller == null)
                m_Controller = gameObject.AddComponent<SceneObjectLoadController>();

            int max_create_num = 19;
            int min_create_num = 0;
            m_Controller.Init(scene_info.Bounds.center, scene_info.Bounds.size, true, max_create_num, min_create_num, SceneSeparateTreeType.QuadTree);

            Debug.Log("scene_info.ObjectInfoList.Count : "+scene_info.ObjectInfoList.Count.ToString());
            for (int i = 0; i < scene_info.ObjectInfoList.Count; i++)
            {
                m_Controller.AddSceneBlockObject(scene_info.ObjectInfoList[i]);
            }

            GameObjectEntity mainRoleGOE = RoleMgr.GetInstance().GetMainRole();
            if (mainRoleGOE != null)
            {
                detector = mainRoleGOE.GetComponent<SceneDetectorBase>();
            }
        });
    }
    LightmapData[] lightmaps = null;

    public void ApplyDetector(SceneDetectorBase detector)
    {
        this.detector = detector;
    }

    private void ApplyLightInfo(SceneInfo scene_info)
    {
        LightmapSettings.lightmapsMode = scene_info.LightmapMode;
        int l1 = (scene_info.LightColorResPath == null) ? 0 : scene_info.LightColorResPath.Count;
        int l2 = (scene_info.LightDirResPath == null) ? 0 : scene_info.LightDirResPath.Count;
        int l = (l1 < l2) ? l2 : l1;
        // Debug.Log("ApplyLightInfo : "+ l.ToString());
        if (l > 0)
        {
            lightmaps = new LightmapData[l];
            for (int i = 0; i < l; i++)
            {
                if (i < l1)
                {
                    int temp_i = i;
                    XLuaFramework.ResourceManager.GetInstance().LoadAsset<Texture2D>(scene_info.LightColorResPath[i], delegate(UnityEngine.Object[] objs) {
                        // Debug.Log("load lightmap color texture : "+scene_info.LightColorResPath[i].ToString());
                        // Debug.Log("i : "+temp_i.ToString()+" objs:"+(objs!=null).ToString());
                        lightmaps[temp_i] = new LightmapData();
                        if (objs != null && objs.Length > 0)
                            lightmaps[temp_i].lightmapColor = objs[0] as Texture2D;
                        if (temp_i == l-1)
                            LightmapSettings.lightmaps = lightmaps;
                    });
                }
                if (i < l2)
                {
                    int temp_i = i;
                    XLuaFramework.ResourceManager.GetInstance().LoadAsset<Texture2D>(scene_info.LightDirResPath[i], delegate(UnityEngine.Object[] objs) {
                        // Debug.Log("load lightmap dir texture : "+scene_info.LightDirResPath[i].ToString());
                        lightmaps[temp_i] = new LightmapData();
                        if (objs != null && objs.Length > 0)
                            lightmaps[temp_i].lightmapDir = objs[0] as Texture2D;
                        if (temp_i == l-1)
                            LightmapSettings.lightmaps = lightmaps;
                    });
                }
            }
 
        }
    }

    public Entity AddMainRole(long uid, Vector3 pos)
	{
        Entity role = RoleMgr.GetInstance().AddMainRole(uid, pos);
        entityDic.Add(uid, role);
        return role;
    }
    
    //     GameObjectEntity roleGameOE = m_GameWorld.Spawn<GameObjectEntity>(prefabDic["MainRole"]);
    //     roleGameOE.name = "MainRole_"+uid;
    //     Entity role = roleGameOE.Entity;
    //     InitRole(role, uid);
	// 	// Entity role = AddRole(uid);
    //     EntityManager.AddComponent(role, ComponentType.Create<MainRoleTag>());
    //     EntityManager.AddComponent(role, ComponentType.Create<PlayerInput>());
    //     EntityManager.AddComponent(role, ComponentType.Create<SynchPosFlag>());
    //     entityDic.Add(uid, role);
    //     mainRole = role;
    //     return role;
	// }
    public Entity AddRole(long uid)
	{
        Entity role = RoleMgr.GetInstance().AddRole(uid);
        entityDic.Add(uid, role);
        return role;
    }
  
    public Entity AddNPC(long uid)
	{
		Entity entity = EntityManager.CreateEntity(NPCArchetype);
        EntityManager.SetComponentData(entity, new Position {Value = new int3(0, 0, 0)});
        // EntityManager.AddSharedComponentData(entity, GetLookFromPrototype("Prototype/NPCRenderPrototype"));
        entityDic.Add(uid, entity);
        return entity;
	}

    public Entity AddSceneObject(long uid, SceneObjectType type)
    {
        if (type == SceneObjectType.Role)
            return AddRole(uid);
        // else if (type == SceneObjectType.NPC)
            // return AddNPC(uid);
        return Entity.Null;
    }

    public void RemoveSceneObject(long uid)
    {
        Entity entity = GetSceneObject(uid);
        if (entity!=Entity.Null)
            EntityManager.DestroyEntity(entity);
    }

    public Entity GetSceneObject(long uid)
    {
        Debug.Log("GetSceneObject uid"+uid.ToString()+" ContainsKey:"+entityDic.ContainsKey(uid).ToString());
        if (entityDic.ContainsKey(uid))
            return entityDic[uid];
        return Entity.Null;
    }

	// private MeshInstanceRenderer GetLookFromPrototype(string protoName)
    // {
    //     var proto = GameObject.Find(protoName);
    //     var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
    //     // Object.Destroy(proto);
    //     return result;
    // }
}

}