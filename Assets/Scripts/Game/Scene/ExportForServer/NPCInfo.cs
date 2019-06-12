
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class NPCInfo : BaseInfoForServer
{
    [DataMember]
    public int npc_id;
}
