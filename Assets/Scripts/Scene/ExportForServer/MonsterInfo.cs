
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class MonsterInfo : BaseInfoForServer
{
    [DataMember]
    public int monster_id;
}
