using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace UnityMMO
{
public struct RaycastSceneObjHit 
{
    public Entity entity;
    public Vector3 point;
    public bool hit;
}
public class SceneHelper
{
    private static EntityManager EntityMgr;
    public static Entity[] UIDTempCacheList = new Entity[10];

    public static void Init(EntityManager entityMgr)
    {
        EntityMgr = entityMgr;
    }

    public static RaycastSceneObjHit GetClickSceneObject()
    {
        RaycastSceneObjHit result = new RaycastSceneObjHit();
        result.hit = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.Log("Input.mousePosition : "+Input.mousePosition.x+" "+Input.mousePosition.y+" z:"+Input.mousePosition.z);
        RaycastHit hit=new RaycastHit();
        if(Physics.Raycast(ray,out hit))
        {
            Debug.Log("get click scene object : "+hit.collider.name);
            result.point = hit.point;
            result.hit = true;
            var uid = hit.collider.GetComponentInParent<UIDProxy>();
            if (uid != null)
            {
                result.entity = SceneMgr.Instance.GetSceneObject(uid.Value.Value);
            }
        }
        return result;
    }

    //unfinished method
    public static long GetSceneObjectByPos(Vector3 absPos, Dictionary<long, Entity> entityDic)
    {
        int inAreaNum = 0; 
        foreach (var item in entityDic)
        {
            var isIn = IsPosInEntityBound(absPos, item.Value);
            if (isIn)
            {
                UIDTempCacheList[inAreaNum] = item.Value;
                inAreaNum++;
            }
        }
        return 0;
    }

    public static bool IsPosInEntityBound(Vector3 absPos, Entity entity)
    {
        var hasMoveQuery = EntityMgr.HasComponent<MoveQuery>(entity);
        if (hasMoveQuery)
        {
            var moveQuery = EntityMgr.GetComponentObject<MoveQuery>(entity);
            var entityPos = moveQuery.transform.position;
            var bottomY = entityPos.y;
            var topY = entityPos.y + moveQuery.height;
            var leftX = entityPos.x - moveQuery.radius;
            var rightX = entityPos.x + moveQuery.radius;
        }
        return false;
    }

}
}