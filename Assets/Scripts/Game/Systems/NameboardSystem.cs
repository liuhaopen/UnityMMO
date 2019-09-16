using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Profiling;
using UnityMMO;
using UnityMMO.Component;

namespace UnityMMO
{
[DisableAutoCreation]
public class NameboardSystem : BaseComponentSystem
{
    EntityQuery Group;
    Transform nameboardCanvas;
    public NameboardSystem(GameWorld gameWorld) : base(gameWorld)
    {
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        Group = GetEntityQuery(typeof(Transform), typeof(NameboardData));
        nameboardCanvas = GameObject.Find("UICanvas/Nameboard").transform;
    }

    protected override void OnUpdate()
    {      
        if (Camera.main == null)
            return;
        var entityArray = Group.ToEntityArray(Allocator.TempJob);
        var nameboardArray = Group.ToComponentArray<NameboardData>();
        var posArray = Group.ToComponentArray<Transform>();
        for (var i = 0; i < nameboardArray.Length; i++)
        {
            var nameboard = nameboardArray[i];
            var entity = entityArray[i];
            UpdateNameboard(posArray[i], nameboard, entity);
        }
        entityArray.Dispose();
    }

    void UpdateNameboard(Transform target, NameboardData nameboardData, Entity entity)
    {
        Vector2 board2DPosition = Camera.main.WorldToScreenPoint(target.position);
        Vector3 BloodSlotWorldPos = target.position + new Vector3 (0f, nameboardData.Height, 0f);
        Vector3 BloodSlotToCamera = Camera.main.transform.position - BloodSlotWorldPos;
        float BloodSlotDIs = BloodSlotToCamera.magnitude;
        float maxVisualDis = 20;
        float scaleFactor = Mathf.Clamp(1-(BloodSlotDIs-maxVisualDis)/maxVisualDis, 0, 1);
        Vector3 dir = (target.position - Camera.main.transform.position).normalized;
        float dot = Vector3.Dot(Camera.main.transform.forward, dir);     //判断物体是否在相机前面
        bool isBoardVisible = dot > 0 && (board2DPosition.x <= Screen.width && board2DPosition.x >= 0 && board2DPosition.y <= Screen.height && board2DPosition.y >= 0);
        if (isBoardVisible)
        {
            if (nameboardData.UIResState == NameboardData.ResState.WaitLoad)
            {
                GenNameboardLooksNode(entity, nameboardData);
            }
            else if (nameboardData.UIResState == NameboardData.ResState.Loaded)
            {
                nameboardData.LooksNode.position = BloodSlotWorldPos;
            }
        }
        else if (nameboardData.UIResState == NameboardData.ResState.Loaded)
        {
            nameboardData.UnuseLooks();
            // var transform = nameboardData.LooksNode;
            // transform.gameObject.SetActive(false);
            // ResMgr.GetInstance().UnuseGameObject("Nameboard", transform.gameObject);
            // nameboardData.UIResState = NameboardData.ResState.WaitLoad;
            // nameboardData.LooksNode = null;
        }
    }

    private void GenNameboardLooksNode(Entity owner, NameboardData nameboardData)
    {
        if (!EntityManager.Exists(owner))
            return;
        var looksNode = ResMgr.GetInstance().GetGameObject("Nameboard");
        looksNode.transform.SetParent(nameboardCanvas);
        looksNode.transform.localScale = new Vector3(-1, 1, 1);
        var nameboardBehav = looksNode.GetComponent<Nameboard>();
        var uid = EntityManager.GetComponentData<UID>(owner);
        string name = SceneMgr.Instance.GetNameByUID(uid.Value);
        // nameboardBehav.Name = name;
        var isMainRole = RoleMgr.GetInstance().IsMainRoleEntity(owner);
        nameboardBehav.CurColorStyle = isMainRole ? Nameboard.ColorStyle.Green : Nameboard.ColorStyle.Red;
        nameboardData.LooksNode = looksNode.transform;
        nameboardData.UIResState = NameboardData.ResState.Loaded;
        nameboardData.SetName(name);
        bool hasHPData = EntityManager.HasComponent<HealthStateData>(owner);
        if (hasHPData)
        {
            var hpData = EntityManager.GetComponentData<HealthStateData>(owner);
            nameboardBehav.MaxHp = hpData.MaxHp;
            nameboardBehav.CurHp = hpData.CurHp;
        }

        var sceneObjType = EntityManager.GetComponentData<SceneObjectTypeData>(owner);
        nameboardBehav.SetBloodVisible(sceneObjType.Value==SceneObjectType.Role || sceneObjType.Value==SceneObjectType.Monster);
    }
}

}
