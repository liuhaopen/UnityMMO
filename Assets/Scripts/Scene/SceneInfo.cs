using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace UnityMMO {
[Serializable]
[DataContract]
public class SceneInfo{
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

    public SceneInfo()
    {
    }
}
}