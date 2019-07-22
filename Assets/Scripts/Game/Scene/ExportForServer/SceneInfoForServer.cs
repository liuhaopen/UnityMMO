
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityMMO;

[DataContract]
public class SceneInfoForServer : MonoBehaviour
{
    [DataMember]
    public int scene_id;

    [DataMember]
    public string scene_name;

    [HideInInspector]
    [DataMember]
    public List<DoorInfo> door_list;

    [HideInInspector]
    [DataMember]
    public List<BornInfo> born_list;

    [HideInInspector]
    [DataMember]
    public List<NPCInfo> npc_list;

    [HideInInspector]
    [DataMember]
    public List<MonsterInfo> monster_list;

    [HideInInspector]
    [DataMember]
    public List<CollectableInfo> collectable_list;
    
    // [DataMember]
    // public Dictionary<int, string> test_dic;
}

[DataContract]
public class BaseInfoForServer : MonoBehaviour
{
    [DataMember]
    public float pos_x
    {
        get
        {
            return (int)(transform.position.x*EditorConst.LogicFactor);
        }
        set
        {
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }
    }

    [DataMember]
    public float pos_y
    {
        get
        {
            return (int)(transform.position.y*EditorConst.LogicFactor);
        }
        set
        {
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }
    }

    [DataMember]
    public float pos_z
    {
        get
        {
            return (int)(transform.position.z*EditorConst.LogicFactor);
        }
        set
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }
    }
}