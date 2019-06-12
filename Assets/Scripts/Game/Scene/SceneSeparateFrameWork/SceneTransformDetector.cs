using UnityEngine;
using System.Collections;

/// <summary>
/// 该触发器根据Transform的包围盒区域触发
/// </summary>
public class SceneTransformDetector : SceneDetectorBase
{
    public Vector3 detectorSize;

    private Bounds m_Bounds;

    public override bool IsDetected(Bounds bounds)
    {
        m_Bounds.center = Position;
        m_Bounds.size = detectorSize;
        return bounds.Intersects(m_Bounds);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Bounds b = new Bounds(transform.position, detectorSize);
        b.DrawBounds(Color.yellow);
    }
#endif
}
