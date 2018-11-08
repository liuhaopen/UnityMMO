
using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class SceneInfoForServer
{
    [DataMember]
    public int scene_id;

    [DataMember]
    public string scene_name;
    [DataMember]
    public List<DoorInfo> door_list;
    [DataMember]
    public List<NPCInfo> npc_list;
    [DataMember]
    public List<MonsterInfo> monster_list;

    [DataMember]
    public Dictionary<int, string> test_dic;
}

[DataContract]
public class DoorInfo
{
    [DataMember]
    public int door_id;

    [DataMember]
    public float pos_x;

    [DataMember]
    public float pos_y;

    [DataMember]
    public float pos_z;

    [DataMember]
    public int target_scene_id;

    [DataMember]
    public float target_x;

    [DataMember]
    public float target_y;

    [DataMember]
    public float target_z;
}

[DataContract]
public class NPCInfo
{
    [DataMember]
    public int npc_id;

    [DataMember]
    public float pos_x;

    [DataMember]
    public float pos_y;

    [DataMember]
    public float pos_z;
}

[DataContract]
public class MonsterInfo
{
    [DataMember]
    public int monster_id;

    [DataMember]
    public float pos_x;

    [DataMember]
    public float pos_y;

    [DataMember]
    public float pos_z;
}