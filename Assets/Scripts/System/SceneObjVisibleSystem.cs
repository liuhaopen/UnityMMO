using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using XLuaFramework;
using SprotoType;

namespace UnityMMO
{
    public class SceneObjVisibleSystem : ComponentSystem
    {
        float lastExecTime = 0;
        public struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Position> Position;
            public ComponentDataArray<SynchPosFlag> SynchoFlag;
            
        }

        [Inject] private Data m_Data;
        protected override void OnUpdate()
        {
            if (Time.time - lastExecTime < 0.1 || !GameVariable.IsNeedSynchSceneInfo)
                return;
            lastExecTime = Time.time;
            for (int index = 0; index < m_Data.Length; ++index)
            {
                
            }         
        }
    }
}