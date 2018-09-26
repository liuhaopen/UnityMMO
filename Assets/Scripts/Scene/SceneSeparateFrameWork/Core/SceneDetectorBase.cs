using UnityEngine;
using System.Collections;

public abstract class SceneDetectorBase : MonoBehaviour, IDetector
{
    public Vector3 Position
    {
        get { return transform.position; }
    }
    

    public abstract bool IsDetected(Bounds bounds);
    
}
