using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace UnityMMO {
[DataContract]
public class SceneInfo{
    [DataMember]
    public Bounds Bounds;

    [DataMember]
    public List<SceneStaticObject> ObjectInfoList;
}
}