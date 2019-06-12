
using System.Runtime.Serialization;
using UnityEngine;

namespace UnityMMO
{
[DataContract]
public class BornInfo : BaseInfoForServer
{
    [DataMember]
    public int born_id;

    public Vector3 GetUnityPos()
    {
        return new Vector3(pos_x/EditorConst.LogicFactor, pos_y/EditorConst.LogicFactor, pos_z/EditorConst.LogicFactor);
    }
}

[System.Serializable]
[DataContract]
public class BornInfoData : BaseSceneInfoData
{
    [SerializeField]
    [DataMember]
    public int bornId;

    public BornInfoData(Vector3 pos, int bornId) : base(pos)
    {
        this.bornId = bornId;
    }
}
}