using System;
using Sproto;
using Unity.Entities;
using UnityEngine;
using static Protocol;

namespace UnityMMO {
public class SynchFromNet {
    private static SynchFromNet instance = null;

    public static SynchFromNet Instance 
    { 
        get  {
            if (instance != null)
                return instance;
            return instance = new SynchFromNet();
        }
    }

    public void Init()
    {
    }

    public void ReqSceneObjInfoChange()
    {
        // Debug.Log("GameVariable.IsNeedSynchSceneInfo : "+GameVariable.IsNeedSynchSceneInfo.ToString());
        if (GameVariable.IsNeedSynchSceneInfo)
        {
            Debug.Log("ReqSceneObjInfoChange");
            SprotoType.scene_get_objs_info_change.request req = new SprotoType.scene_get_objs_info_change.request();
            NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_get_objs_info_change>(req, OnAckSceneObjInfoChange);
        }
        else
        {
            Timer.Register(0.5f, () => ReqSceneObjInfoChange());
        }
    }

    public void OnAckSceneObjInfoChange(SprotoTypeBase result)
    {
        Debug.Log("synch from net received OnAckSceneObjInfoChange:"+(result!=null).ToString());
        SprotoType.scene_get_objs_info_change.response ack = result as SprotoType.scene_get_objs_info_change.response;
        int len = ack.obj_infos.Count;
        for (int i = 0; i < len; i++)
        {
            long uid = ack.obj_infos[i].scene_obj_uid;
            Entity scene_obj = SceneMgr.Instance.GetSceneObject(uid);
            var change_info_list = ack.obj_infos[i].info_list;
            int info_len = change_info_list.Count;
            Debug.Log("uid : "+uid.ToString()+ " info_len:"+info_len.ToString());
            for (int info_index = 0; info_index < info_len; info_index++)
            {
                var cur_change_info = change_info_list[info_index];
                if (cur_change_info.key == (int)SceneInfoKey.EnterScene)
                {
                    if (scene_obj==Entity.Null)
                    {
                        SceneObjectType sceneObjType = (SceneObjectType)Enum.Parse(typeof(SceneObjectType), cur_change_info.value);
                        Debug.Log("sceneObjType : "+sceneObjType.ToString());
                        scene_obj = SceneMgr.Instance.AddSceneObject(uid, sceneObjType);
                    }
                }
                else
                {
                    ApplyChangeInfo(scene_obj, cur_change_info);
                }
            }
        }
    }

    private void ApplyChangeInfo(Entity obj, SprotoType.info_item change_info)
    {
        if (obj==Entity.Null)
            return;
        
    }
}
}