using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SceneOnlyShowFrontDetector : SceneDetectorBase
{
    public Vector3 size;

    private Bounds m_Bounds;
    private Transform cameraTrans;

    void Start()
    {
        cameraTrans = Camera.main.transform;
        m_Bounds.size = size;
    }

    void Update()
    {
        m_Bounds.center = transform.position;
    }

    public override bool IsDetected(Bounds bounds)
    {
        var isIntersect =  bounds.Intersects(m_Bounds);
        if (isIntersect)
        {   
            if (IsInMyFront(bounds))
                return true;
        }
        return false;
    }

    bool IsInMyFront(Bounds bounds)
    {
        var myPos = cameraTrans.position;
        var otherPos = bounds.center;
        myPos.y = 0;
        otherPos.y = 0;
        var dir = otherPos-myPos;
        dir.Normalize();
        var cameraForward = cameraTrans.forward;
        cameraForward.y = 0;
        var angle = Vector3.Angle(dir, cameraForward);
        var dot = Vector3.Dot(dir, cameraForward);
        // Debug.Log("angle : "+angle+" dot:"+dot+" "+cameraForward);
        if (dot <= 0)
        {
            Debug.Log("bounds "+bounds.center+" "+myPos+" "+cameraTrans.forward+" "+dir);
        }
        return dot > 0;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        m_Bounds.DrawBounds(Color.yellow);
        Gizmos.color = Color.red;
        var cameraForward = cameraTrans.forward;
        cameraForward.y = 0;
        Gizmos.DrawLine(cameraTrans.position, cameraTrans.position+cameraForward*1000);
    }
#endif
}
