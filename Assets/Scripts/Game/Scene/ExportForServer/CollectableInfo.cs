
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class CollectableInfo : BaseInfoForServer
{
    [DataMember]
    public int collectable_type_id;
}
