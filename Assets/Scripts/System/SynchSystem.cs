using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using XLuaFramework;
using SprotoType;

namespace UnityMMO
{
    //synch entities property to server
    public class SynchSystem : ComponentSystem
    {
        float lastSynchTime = 0;
        public struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Position> Position;
            public ComponentDataArray<SynchPosFlag> SynchoFlag;
            
        }

        [Inject] private Data m_Data;

        // protected override void OnCreateManager(int capacity)
	    // {
        //     Debug.Log("synch system OnCreateManager");
		//     base.OnCreateManager(capacity);

        // }
        
        protected override void OnUpdate()
        {
            //upload per second
            // Debug.Log("synch system"+Time.time.ToString()+" lasttime:"+lastSynchTime.ToString());
            if (Time.time - lastSynchTime < 0.1 || !GameVariable.IsNeedSynchSceneInfo)
                return;
            lastSynchTime = Time.time;
            long synchTime = System.DateTime.Now.Millisecond;
            for (int index = 0; index < m_Data.Length; ++index)
            {
                // Debug.Log("synch system");
                scene_walk.request walk = new scene_walk.request();
                float3 cur_pos = m_Data.Position[index].Value;
                walk.pos_x = (int)(cur_pos.x*GameConst.RealToLogic);
                walk.pos_y = (int)(cur_pos.y*GameConst.RealToLogic);
                walk.pos_z = (int)(cur_pos.z*GameConst.RealToLogic);
                walk.time = synchTime;
                NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_walk>(walk);
            }         
        }
    }
}