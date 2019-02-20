
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

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
    // [DataMember]
    // public Dictionary<int, string> test_dic;
}
