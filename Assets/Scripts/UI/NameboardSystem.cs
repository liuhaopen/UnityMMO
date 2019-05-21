using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class NameboardSystem : BaseComponentSystem
{
    ComponentGroup Group;

    public NameboardSystem(GameWorld gameWorld) : base(gameWorld)
    {
    }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        Group = GetComponentGroup(typeof(Transform), typeof(NameboardData));
    }

    protected override void OnUpdate()
    {      
        var entityArray = Group.GetEntityArray();
        var nameboardArray = Group.GetComponentDataArray<NameboardData>();
        var posArray = Group.GetComponentArray<Transform>();

        for (var i = 0; i < nameboardArray.Length; i++)
        {
            var nameboard = nameboardArray[i];
            var entity = entityArray[i];
            UpdateNameboard(posArray[i], nameboard, entity);
        }
    }

    void UpdateNameboard(Transform target, NameboardData nameboardData, Entity entity)
    {
        if (nameboardData.UIResState == NameboardData.ResState.Loaded)
        {
            if (nameboardData.UIEntity != Entity.Null)
            {
                Vector2 player2DPosition = Camera.main.WorldToScreenPoint(target.position);
                Vector3 BloodSlotWorldPos = target.position + new Vector3 (0f, 1.8f, 0f);
                Vector3 BloodSlotToCamera = Camera.main.transform.position - BloodSlotWorldPos;
                float BloodSlotDIs = BloodSlotToCamera.magnitude;
                float maxVisualDis = 20;
                float scaleFactor = Mathf.Clamp(1-(BloodSlotDIs-maxVisualDis)/maxVisualDis, 0, 1);
                // Debug.Log("scaleFactor : "+scaleFactor+" dis:"+BloodSlotDIs);
                if (EntityManager.HasComponent<RectTransform>(nameboardData.UIEntity))
                {
                    var transform = EntityManager.GetComponentObject<RectTransform>(nameboardData.UIEntity);
                    transform.position = Camera.main.WorldToScreenPoint(BloodSlotWorldPos);
                    transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                    //TODO: DeSpawnRequest.Create
                    // if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
                    // {
                    //     transform.gameObject.SetActive(false);
                    // }
                    // else
                    // {
                    //     transform.gameObject.SetActive(true);
                    // }
                }
            }
        }
        else if (nameboardData.UIResState == NameboardData.ResState.WaitLoad)
        {
            NameboardSpawnRequest.Create(PostUpdateCommands, entity);
            nameboardData.UIResState = NameboardData.ResState.Loading;
            EntityManager.SetComponentData(entity, nameboardData);
        }
       
    }

}

public struct NameboardSpawnRequest : IComponentData
{
    public Entity Owner;
    public static void Create(EntityCommandBuffer commandBuffer, Entity Owner)
    {
        var data = new NameboardSpawnRequest()
        {
            Owner = Owner,
        };
        commandBuffer.CreateEntity();
        commandBuffer.AddComponent(data);
    }
}

[DisableAutoCreation]
public class NameboardSpawnRequestSystem : BaseComponentSystem
{
    ComponentGroup Group;
    Transform nameboardCanvas;

    public NameboardSpawnRequestSystem(GameWorld gameWorld) : base(gameWorld)
    {
    }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        Group = GetComponentGroup(typeof(NameboardSpawnRequest));
        nameboardCanvas = GameObject.Find("UICanvas/Scene").transform;
    }

    protected override void OnUpdate()
    {      
        var requestArray = Group.GetComponentDataArray<NameboardSpawnRequest>();
        var entityArray = Group.GetEntityArray();

        var spawnRequests = new NameboardSpawnRequest[requestArray.Length];
        for (var i = 0; i < requestArray.Length; i++)
        {
            spawnRequests[i] = requestArray[i];
            PostUpdateCommands.DestroyEntity(entityArray[i]);
        }

        for(var i =0;i<spawnRequests.Length;i++)
        {
            var request = spawnRequests[i];
            GameObjectEntity nameboardGOE = m_world.Spawn<GameObjectEntity>(ResMgr.GetInstance().GetPrefab("Nameboard"));
            nameboardGOE.transform.SetParent(nameboardCanvas);
            var nameboardBehav = nameboardGOE.GetComponent<Nameboard>();
            var uid = EntityManager.GetComponentData<UID>(request.Owner);
            string name = SceneMgr.Instance.GetNameByUID(uid.Value);
            nameboardBehav.Name = name;
            var isMainRole = RoleMgr.GetInstance().IsMainRoleEntity(request.Owner);
            nameboardBehav.CurColorStyle = isMainRole ? Nameboard.ColorStyle.Green : Nameboard.ColorStyle.Red;
            if (EntityManager.HasComponent<NameboardData>(request.Owner))
            {
                var nameboardData = EntityManager.GetComponentData<NameboardData>(request.Owner);
                nameboardData.UIEntity = nameboardGOE.Entity;
                nameboardData.UIResState = NameboardData.ResState.Loaded;
                EntityManager.SetComponentData(request.Owner, nameboardData);
            }
            var sceneObjType = EntityManager.GetComponentData<SceneObjectTypeData>(request.Owner);
            nameboardBehav.SetBloodVisible(sceneObjType.Value==SceneObjectType.Role || sceneObjType.Value==SceneObjectType.Monster);
        }
    }
}