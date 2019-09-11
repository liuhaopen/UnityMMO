using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Cinemachine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityMMO.Component;
using XLua;

namespace UnityMMO
{
public class SceneMgr : MonoBehaviour
{
	public static SceneMgr Instance;
    GameWorld m_GameWorld;
    // Dictionary<long, Entity> entityDic;
    Dictionary<SceneObjectType, Dictionary<long, Entity>> entitiesDic;
    SceneInfo curSceneInfo;
    int curSceneID;
    public SceneDetectorBase detector;
    private SceneObjectLoadController m_Controller;
    const string SceneInfoPath = "Assets/AssetBundleRes/scene/";
    private bool isLoadingScene = false;
    bool isBaseWorldLoadOk = false;
    
    public LayerMask groundLayer;
    float lastCheckMainRolePosTime = 0;
    Transform moveQueryContainer = null;
    Transform flyWordContainer = null;
    Transform sceneObjContainer = null;
    public EntityManager EntityManager { get => m_GameWorld.GetEntityManager();}
    public GameWorld World { get => m_GameWorld;}
    public bool IsLoadingScene { get => isLoadingScene; set => isLoadingScene = value; }
    public CinemachineFreeLook FreeLookCamera { get => freeLookCamera; set => freeLookCamera = value; }
    public Transform MainCameraTrans { get => mainCameraTrans; }
    public Transform FreeLookCameraTrans { get => freeLookCameraTrans; }
    public SceneInfo CurSceneInfo { get => curSceneInfo; }
    public Transform MoveQueryContainer { get =>moveQueryContainer; }
    public Transform FlyWordContainer { get =>flyWordContainer; }
    public Transform SceneObjContainer { get =>sceneObjContainer; }
    public int CurSceneID { get => curSceneID; set => curSceneID = value; }

    Cinemachine.CinemachineFreeLook freeLookCamera;
    Transform freeLookCameraTrans;
    Transform mainCameraTrans;

    void Awake()
	{
		Instance = this; // worst singleton ever but it works
        curSceneID = 0;
        groundLayer = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Ground");
        entitiesDic = new Dictionary<SceneObjectType, Dictionary<long, Entity>>();
        entitiesDic.Add(SceneObjectType.Role, new Dictionary<long, Entity>());
        entitiesDic.Add(SceneObjectType.Monster, new Dictionary<long, Entity>());
        entitiesDic.Add(SceneObjectType.NPC, new Dictionary<long, Entity>());

        var mainCamera = GameObject.Find("MainCamera");
        mainCameraTrans = mainCamera.transform;
        var camera = GameObject.Find("FreeLookCamera");
        if (camera != null)
        {
            freeLookCameraTrans = camera.transform;
            FreeLookCamera = camera.GetComponent<Cinemachine.CinemachineFreeLook>();
        }
        moveQueryContainer = GameObject.Find("SceneObjContainer/MoveQueryContainer").transform;
        flyWordContainer = GameObject.Find("SceneObjContainer/FlyWordContainer").transform;
        sceneObjContainer = GameObject.Find("SceneObjContainer").transform;
	}

    void Update()
    {
        if (detector != null && m_Controller != null)
            m_Controller.RefreshDetector(detector);
    }

    public void CheckMainRolePos()
    {
        if (Time.time - lastCheckMainRolePosTime < 3)
            return;
        var mainRole = RoleMgr.GetInstance().GetMainRole();
        if (!isBaseWorldLoadOk || mainRole == null || curSceneInfo==null)
            return;
        
        lastCheckMainRolePosTime = Time.time;

        Vector3 curPos = mainRole.transform.localPosition;
        bool isInScene = curSceneInfo.Bounds.Contains(curPos);
        if (!isInScene)
            CorrectMainRolePos();
    }

	public void Init(GameWorld world)
	{
        m_GameWorld = world;

        RoleMgr.GetInstance().Init(world);
        MonsterMgr.GetInstance().Init(world);
        NPCMgr.GetInstance().Init(world);
        FightMgr.GetInstance().Init(world);
        BuffMgr.GetInstance().Init(world);
	}

	public void OnDestroy()
	{
        Debug.Log("destroy scene mgr");
        // UnloadScene();
		Instance = null;
	}

    private static string Repalce(string str)
    {
        str = System.Text.RegularExpressions.Regex.Replace(str, @"<", "lt;");
        str = System.Text.RegularExpressions.Regex.Replace(str, @">", "gt;");
        str = System.Text.RegularExpressions.Regex.Replace(str, @"\r", "");
        str = System.Text.RegularExpressions.Regex.Replace(str, @"\n", "");
        str = System.Text.RegularExpressions.Regex.Replace(str, @"\/", "/");
        return str;
    }

    void LoadSceneInfo(int scene_id, Action<int> on_ok)
    {
        //load scene info from json file(which export from SceneInfoExporter.cs)
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<TextAsset>(SceneInfoPath+"scene_"+scene_id.ToString() +"/scene_info.json", delegate(UnityEngine.Object[] objs) {
            LoadingView.Instance.SetData(0.4f, "读取场景信息文件...");
            TextAsset txt = objs[0] as TextAsset;
            string scene_json = txt.text;
            scene_json = Repalce(scene_json);
            SceneInfo scene_info = JsonUtility.FromJson<SceneInfo>(scene_json);
            curSceneInfo = scene_info;
            Debug.Log("LoadSceneInfo start LoadSceneRes count:"+scene_info.ResPathList.Count+" "+scene_info.MonsterList.Count);
            // ApplyLightInfo(scene_info);
            ResMgr.GetInstance().LoadSceneRes(scene_info.ResPathList, delegate(bool isOk)
            {
                ResMgr.GetInstance().LoadMonsterResList(scene_info.MonsterList, delegate(bool isOk2) {
                LoadingView.Instance.SetData(0.6f, "初始化场景节点信息...");
                m_Controller = gameObject.GetComponent<SceneObjectLoadController>();
                if (m_Controller == null)
                    m_Controller = gameObject.AddComponent<SceneObjectLoadController>();
                int max_create_num = 25;
                int min_create_num = 5;
                m_Controller.Init(scene_info.Bounds.center, scene_info.Bounds.size, true, max_create_num, min_create_num, SceneSeparateTreeType.QuadTree);
                // Debug.Log("scene_info.ObjectInfoList.Count : "+scene_info.ObjectInfoList.Count.ToString());
                for (int i = 0; i < scene_info.ObjectInfoList.Count; i++)
                {
                    m_Controller.AddSceneBlockObject(scene_info.ObjectInfoList[i]);
                }

                GameObjectEntity mainRoleGOE = RoleMgr.GetInstance().GetMainRole();
                if (mainRoleGOE != null)
                {
                    detector = mainRoleGOE.GetComponent<SceneDetectorBase>();
                }
                on_ok(1);
            });});
        });
    }

    public void LoadScene(int scene_id)
    {
        if (curSceneID == scene_id)
            return;
        if (curSceneID != 0)
        {
            UnloadScene();
        }
        curSceneID = scene_id;
        LoadingView.Instance.SetData(0.2f, "加载场景信息文件...");
        Debug.Log("LoadScene scene_id "+(scene_id).ToString());
        isLoadingScene = true;
        isBaseWorldLoadOk = false;
        LoadSceneInfo(scene_id, delegate(int result){
            string baseWorldScenePath = "";
            if (XLuaFramework.AppConfig.DebugMode)
            {
                baseWorldScenePath = "Assets/AssetBundleRes/scene/base_world/base_world_"+scene_id+".unity";
            }
            else
            {
                baseWorldScenePath = "base_world_"+scene_id;
                XLuaFramework.ResourceManager.GetInstance().LoadNavMesh(baseWorldScenePath);
            }
            LoadingView.Instance.SetData(0.8f, "加载基础场景...");
            AsyncOperation asy = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(baseWorldScenePath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            // asy.allowSceneActivation = false;
            // while (asy.progress < 0.9f)
            // {
            //     Debug.Log("Loading scene " + " [][] Progress: " + asy.progress);
            //     yield return null;
            // }
            asy.completed += delegate(AsyncOperation asyOp)
            {
                Debug.Log("load base world:"+asyOp.isDone.ToString());
                isLoadingScene = false;
                isBaseWorldLoadOk = true;
                // CorrectMainRolePos();
                LoadingView.Instance.SetData(1, "加载场景完毕");
                LoadingView.Instance.SetActive(false, 0.5f);
                Timer.Register(0.5f, () => {
                    //切换场景后需要重置一下 NavMeshAgent 组件
                    RoleMgr.GetInstance().UpdateMainRoleNavAgent();
                });
                CorrectSceneObjectsPos();
                CorrectMainRolePos();
                XLuaFramework.CSLuaBridge.GetInstance().CallLuaFunc(GlobalEvents.SceneChanged);
            };
        });
    }

    public void StopAllTimeline()
    {
        var directors = sceneObjContainer.GetComponentsInChildren<PlayableDirector>();
        foreach (var director in directors)
        {
            director.Stop();
        }
    }

    public void UnloadScene()
    {
        RoleMgr.GetInstance().StopMainRoleRunning();
        StopAllTimeline();
        string baseSceneName = "base_world_"+curSceneID;
        // NavMesh.RemoveAllNavMeshData();
        AsyncOperation asy = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(baseSceneName);
        asy.completed += delegate(AsyncOperation asyOp){
            Debug.Log("unload scene finish");
        };
        if (m_Controller != null)
            m_Controller.ResetAllData();
        RemoveAllSceneObjects();
        var sceneObjects = GameObject.Find("SceneMgr").transform;
        XLuaFramework.Util.ClearChild(sceneObjects);
    }

    public void ReqEnterScene(int scene_id, int door_id=0)
    {
        SprotoType.scene_enter_to.request req = new SprotoType.scene_enter_to.request();
        req.scene_id = scene_id;
        req.door_id = door_id;
        NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_enter_to>(req, (ack)=>{
            SprotoType.scene_enter_to.response rsp = ack as SprotoType.scene_enter_to.response;
            Debug.Log("enter new scene result : "+rsp.result);
        });
    }

    LightmapData[] lightmaps = null;
    public void ApplyDetector(SceneDetectorBase detector)
    {
        this.detector = detector;
    }

    //光照信息放在base_world后此函数就没用了
    private void ApplyLightInfo(SceneInfo scene_info)
    {
        LightmapSettings.lightmapsMode = scene_info.LightmapMode;
        int l1 = (scene_info.LightColorResPath == null) ? 0 : scene_info.LightColorResPath.Count;
        int l2 = (scene_info.LightDirResPath == null) ? 0 : scene_info.LightDirResPath.Count;
        int l = (l1 < l2) ? l2 : l1;
        if (l > 0)
        {
            lightmaps = new LightmapData[l];
            for (int i = 0; i < l; i++)
            {
                if (i < l1)
                {
                    int temp_i = i;
                    XLuaFramework.ResourceManager.GetInstance().LoadAsset<Texture2D>(scene_info.LightColorResPath[i], delegate(UnityEngine.Object[] objs) {
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

    //校正角色的坐标
    public void CorrectMainRolePos()
    {
        var mainRole = RoleMgr.GetInstance().GetMainRole();
        if (!isBaseWorldLoadOk || mainRole == null || curSceneInfo==null)
            return;
        Vector3 oldPos = mainRole.transform.localPosition;
        Vector3 newPos = GetCorrectPos(oldPos, true);
        Debug.Log("old pos:"+oldPos.x+" "+oldPos.y+" "+oldPos.z+" newPos:"+newPos.x+" "+newPos.y+" "+newPos.z);
        mainRole.transform.localPosition = newPos;
    }

    public void CorrectSceneObjectsPos()
    {
        foreach (var dic in entitiesDic)
        {
            foreach (var objs in dic.Value)
            {
                Entity entity = objs.Value;
                var trans = EntityManager.GetComponentObject<Transform>(entity);
                // Debug.Log("CorrectSceneObjectsPos : "+trans.gameObject.name);
                var correctPos = GetCorrectPos(trans.localPosition);
                // Debug.Log("trans.localPosition : "+trans.localPosition.x+" "+trans.localPosition.y+" "+trans.localPosition.z+" correctpos:"+correctPos.x+" "+correctPos.y+" "+correctPos.z);
                trans.localPosition = correctPos;
            }
        }
    }

    public Vector3 GetCorrectPos(Vector3 originPos, bool returnClosePointIfRaycastFail=false)
    {
        Vector3 newPos = originPos;
        Ray ray1 = new Ray(originPos + new Vector3(0, 10000, 0), Vector3.down);
        RaycastHit groundHit;
        bool isRaycast = Physics.Raycast(ray1, out groundHit, 12000, groundLayer);
        if (isRaycast)
        {
            newPos = groundHit.point;
            newPos.y += 0.1f;
        }
        else if (returnClosePointIfRaycastFail)
        {
            //wrong pos, return a nearest safe position
            if (curSceneInfo != null)
            {
                float minDistance = int.MaxValue;
                // int nearestIndex = 0;
                for (int i = 0; i < curSceneInfo.BornList.Count; i++)
                {
                    var bornInfo = curSceneInfo.BornList[i];
                    var bornPos = bornInfo.pos;
                    var dis = Vector3.Distance(bornPos, originPos);
                    if (dis < minDistance)
                    {
                        // nearestIndex = i;
                        minDistance = dis;
                        newPos = bornPos;
                    }
                }
                // if (nearestIndex < curSceneInfo.bornList.Count)
                // {
                //     var bornInfo = curSceneInfo.bornList[nearestIndex];
                // }
            }
        }
        // Debug.Log("get correct pos : "+originPos.x+" "+originPos.y+" "+originPos.z+" isRacast:"+isRaycast+" newPos : "+newPos.x+" "+newPos.y+" "+newPos.z);
        return newPos;
    }
    
    public void ApplyMainRole(GameObjectEntity mainRole)
    {
        ApplyDetector(mainRole.GetComponent<SceneDetectorBase>());
        if (FreeLookCamera)
        {
            var mainRoleTrans = mainRole.GetComponent<Transform>();
            FreeLookCamera.m_Follow = mainRoleTrans;
            // FreeLookCamera.m_LookAt = mainRoleTrans;
            FreeLookCamera.m_LookAt = mainRoleTrans.Find("CameraLook");
        }
        CorrectMainRolePos();
    }

    public Entity AddMainRole(long uid, long typeID, string name, int career, Vector3 pos, float curHp, float maxHp)
	{
        Entity role = RoleMgr.GetInstance().AddMainRole(uid, typeID, name, career, pos, curHp, maxHp);
        // entityDic.Add(uid, role);
        entitiesDic[SceneObjectType.Role].Add(uid, role);

        SkillManager.GetInstance().Init(career);
        return role;
    }

    public Entity AddSceneObject(long uid, string content)
    {
        // Debug.Log("content : "+content);
        string[] info_strs = content.Split(',');
        SceneObjectType type = (SceneObjectType)Enum.Parse(typeof(SceneObjectType), info_strs[0]);
        long typeID = Int64.Parse(info_strs[1]);
        long new_x = Int64.Parse(info_strs[2]);
        long new_y = Int64.Parse(info_strs[3]);
        long new_z = Int64.Parse(info_strs[4]);
        long target_x = Int64.Parse(info_strs[5]);
        long target_y = Int64.Parse(info_strs[6]);
        long target_z = Int64.Parse(info_strs[7]);
        var pos = GetCorrectPos(new Vector3(new_x/GameConst.RealToLogic, new_y/GameConst.RealToLogic, new_z/GameConst.RealToLogic));
        var targetPos = GetCorrectPos(new Vector3(target_x/GameConst.RealToLogic, target_y/GameConst.RealToLogic, target_z/GameConst.RealToLogic));
        if (type == SceneObjectType.Role)
        {
            long curHP = Int64.Parse(info_strs[8]);
            long maxHP = Int64.Parse(info_strs[9]);
            Entity role = RoleMgr.GetInstance().AddRole(uid, typeID, pos, targetPos, curHP, maxHP);
            entitiesDic[SceneObjectType.Role].Add(uid, role);
            return role;
        }
        else if (type == SceneObjectType.Monster)
        {
            long curHP = Int64.Parse(info_strs[8]);
            long maxHP = Int64.Parse(info_strs[9]);
            Entity monster = MonsterMgr.GetInstance().AddMonster(uid, typeID, pos, targetPos, curHP, maxHP);
            entitiesDic[SceneObjectType.Monster].Add(uid, monster);
            return monster;
        }
        else if (type == SceneObjectType.NPC)
        {
            Entity npc = NPCMgr.GetInstance().AddNPC(uid, typeID, pos, targetPos);
            entitiesDic[SceneObjectType.NPC].Add(uid, npc);
            return npc;
        }
        return Entity.Null;
    }

    public string GetNameByUID(long uid)
    {
        SceneObjectType type = GetSceneObjTypeByUID(uid);
        string name;
        switch (type)
        {
            case SceneObjectType.Role:
                name = RoleMgr.GetInstance().GetName(uid);
                break;
            case SceneObjectType.Monster:
                name = MonsterMgr.GetInstance().GetName(GetSceneObject(uid));
                break;
            case SceneObjectType.NPC:
                name = NPCMgr.GetInstance().GetName(GetSceneObject(uid));
                break;
            default:
                name = "";
                break;
        }
        return name;
    }

    public SceneObjectType GetSceneObjTypeByUID(long uid)
    {
        int value = (int)math.floor(uid/10000000000);
        switch (value)
        {
            case 1:
                return SceneObjectType.Role;
            case 2:
                return SceneObjectType.Monster;
            case 3:
                return SceneObjectType.NPC;
            default:
                return SceneObjectType.None;
        }
    }

    public void RemoveSceneObject(long uid)
    {
        Entity entity = GetSceneObject(uid);
        if (entity!=Entity.Null)
        {
            RemoveSceneEntity(entity, true);
        }
    }

    public void RemoveSceneEntity(Entity entity, bool deleInDic)
    {
        MoveQuery moveQuery = null;
        if (EntityManager.HasComponent<MoveQuery>(entity))
            moveQuery = EntityManager.GetComponentObject<MoveQuery>(entity);
        bool hasNameboard = EntityManager.HasComponent<NameboardData>(entity);
        if (hasNameboard)
        {
            var nameboardData = EntityManager.GetComponentObject<NameboardData>(entity);
            nameboardData.UnuseLooks();
        }
        if (EntityManager.HasComponent<LooksInfo>(entity))
        {
            var looks = EntityManager.GetComponentData<LooksInfo>(entity);
            looks.Destroy();
        }
        var trans = EntityManager.GetComponentObject<Transform>(entity);
        World.RequestDespawn(trans.gameObject);
        var playableDir = trans.GetComponent<PlayableDirector>();
        if (playableDir != null)
        {
            playableDir.Stop();
        }
        if (deleInDic)
        {
            var UIDData = EntityManager.GetComponentData<UID>(entity);
            foreach (var item in entitiesDic)
            {
                if (item.Value.ContainsKey(UIDData.Value))
                {
                    item.Value.Remove(UIDData.Value);
                    break;
                }
            }
        }
        if (moveQuery!=null)
        {
            GameObject.Destroy(moveQuery.gameObject);
            GameObject.Destroy(moveQuery.queryObj);
        }
    }

    public void RemoveAllSceneObjects(bool isIncludeMainRole=false)
    {
        var mainRole = RoleMgr.GetInstance().GetMainRole();
        foreach (var dic in entitiesDic)
        {
            foreach (var objs in dic.Value)
            {
                Entity entity = objs.Value;
                if (!isIncludeMainRole && mainRole != null && entity == mainRole.Entity)
                    continue;
                RemoveSceneEntity(entity, false);
            }
            dic.Value.Clear();
        }
        if (mainRole != null)
        {
            var uidData = EntityManager.GetComponentData<UID>(mainRole.Entity);
            entitiesDic[SceneObjectType.Role].Add(uidData.Value, mainRole.Entity);
        }
    }

    public void ChangeRoleUID(Entity entity, long newUID)
    {
        // var mainRoleGOE = RoleMgr.GetInstance().GetMainRole();
        // if (mainRoleGOE == null || mainRoleGOE.Entity==Entity.Null)
            // return;
        // var entity = mainRoleGOE.Entity;
        var lastUID = SceneMgr.Instance.EntityManager.GetComponentData<UID>(entity);
        Debug.Log("lastUID.value : "+lastUID.Value+" newUID:"+newUID);
        var roleDic = entitiesDic[SceneObjectType.Role];
        if (roleDic.ContainsKey(lastUID.Value))
        {
            roleDic.Remove(lastUID.Value);
        }
        roleDic.Add(newUID, entity);
        SceneMgr.Instance.EntityManager.SetComponentData<UID>(entity, new UID{Value=newUID});
        MoveQuery moveQuery = SceneMgr.Instance.EntityManager.GetComponentObject<MoveQuery>(entity);
        // Debug.Log("ApplyChangeInfoSceneChange new uid : "+uid+" moveQuery:"+(moveQuery!=null));
        if (moveQuery != null)
            moveQuery.ChangeUID(newUID);
    }

    public Entity GetSceneObject(long uid)
    {
        foreach (var item in entitiesDic)
        {
            if (item.Value.ContainsKey(uid))
            {
                return item.Value[uid];
            }
        }
        return Entity.Null;
    }

    public Dictionary<long, Entity> GetSceneObjects(SceneObjectType type)
    {
        return entitiesDic[type];
    }
}

}