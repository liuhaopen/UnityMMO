using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace UnityMMO
{
public class AutoFight : MonoBehaviour 
{
    float attackInterval = 0.8f;
    float lastAttackTime;
    GameObjectEntity mainRoleGOE;
    Transform mainRoleTrans;
    MoveQuery mainRoleMoveQuery;
    Entity target;

    public AutoFight()
    {
        Reset();
    }
    public void Reset()
    {
        lastAttackTime = 0;
        target = Entity.Null;
    }

    private void OnEnable() {
        mainRoleGOE = RoleMgr.GetInstance().GetMainRole();
        mainRoleTrans = mainRoleGOE.transform;
        mainRoleMoveQuery = mainRoleGOE.GetComponent<MoveQuery>();
    }

    private void Update() 
    {
        // Debug.Log("mainRoleMoveQuery.IsAutoFinding : "+mainRoleMoveQuery.IsAutoFinding+" lastTime:"+lastAttackTime+" target:"+(target!=Entity.Null));
        if (mainRoleMoveQuery.IsAutoFinding || mainRoleMoveQuery.navAgent.pathPending || !mainRoleMoveQuery.navAgent.isStopped)
            return;
        if (Time.time - lastAttackTime < attackInterval)
            return;
        if (target == Entity.Null)
        {
            FindTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    private void FindTarget()
    {
        Entity nearestMon = Entity.Null;
        float minDis = float.MaxValue;
        //find the nearest monster
        Dictionary<long, Entity> monsters = SceneMgr.Instance.GetSceneObjects(SceneObjectType.Monster);
        foreach (var item in monsters)
        {
            var monsTrans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(item.Value);
            var dis = Vector3.Distance(mainRoleTrans.position, monsTrans.position);
            if (dis < minDis)
            {
                minDis = dis;
                nearestMon = item.Value;
            }
        }
        if (nearestMon != Entity.Null)
        {
            target = nearestMon;
        }
    }

    private void AttackTarget()
    {
        var isExist = SceneMgr.Instance.EntityManager.Exists(target);
        if (!isExist)
        {
            target = Entity.Null;
            return;
        }
        var monsTrans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(target);
        var dis = Vector3.Distance(mainRoleTrans.position, monsTrans.position);
        if (dis <= 1.2)
        {
            lastAttackTime = Time.time;
            var skillID = SkillManager.GetInstance().CastRandomSkill();
            var isNormalAttack = SkillManager.GetInstance().IsNormalAttack(skillID);
            attackInterval = isNormalAttack?0.8f:1.5f;
        }
        else
        {
            var findInfo = new FindWayInfo{
                destination = monsTrans.position,
                stoppingDistance = 1.2f,
                onStop = null,
            };
            mainRoleMoveQuery.StartFindWay(findInfo);
        }
    }

}
}