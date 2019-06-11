using UnityEngine;
using Unity.Entities;

namespace UnityMMO
{
    public class ECSHelper
    {
        public static bool IsDead(Entity entity, EntityManager entityMgr)
        {
            var hpData = entityMgr.GetComponentData<HealthStateData>(entity);
            return hpData.CurHp <= 0;
        }

    }

}