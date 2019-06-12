
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class MonsterInfo : BaseInfoForServer
{
    [DataMember]
    public int monster_type_id;
    [DataMember]
    public int monster_num;
    [DataMember]
    public int radius;
}
