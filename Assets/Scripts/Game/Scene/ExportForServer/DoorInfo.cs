
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class DoorInfo : BaseInfoForServer
{
    [DataMember]
    public int door_id;

    [DataMember]
    public int target_scene_id;

    [DataMember]
    public float target_x;

    [DataMember]
    public float target_y;

    [DataMember]
    public float target_z;

    
}
