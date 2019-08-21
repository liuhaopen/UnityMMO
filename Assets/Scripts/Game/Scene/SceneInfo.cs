using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace UnityMMO {
[DataContract]
public class SceneInfo{
    [DataMember]
    public List<string> ResPathList;
    [DataMember]
    public LightmapsMode LightmapMode;
    [DataMember]
    public List<string> LightColorResPath;
    [DataMember]
    public List<string> LightDirResPath;
    [DataMember]
    public Bounds Bounds;
    [DataMember]
    public List<SceneStaticObject> ObjectInfoList;
    [DataMember]
    public List<BornInfoData> BornList;
    [DataMember]
    public List<int> MonsterList;
}

public class BaseSceneInfoData
{
    [SerializeField]
    [DataMember]
    public Vector3 pos;

    public BaseSceneInfoData(Vector3 pos)
    {
        this.pos = pos;
    }
}
}