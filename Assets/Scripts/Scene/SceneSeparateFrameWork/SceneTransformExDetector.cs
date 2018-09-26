using UnityEngine;
using System.Collections;

/// <summary>
/// 该触发器根据Transform的包围盒区域触发-且根据Transform运动趋势扩展包围盒
/// </summary>
public class SceneTransformExDetector : SceneDetectorBase
{
    public Vector3 detectorSize;

    #region 包围盒扩展趋势参数

    public float leftExtDis;
    public float rightExtDis;
    public float topExtDis;
    public float bottomExtDis;
    #endregion

    private Vector3 m_Position;

    private Bounds m_Bounds;

    private Vector3 m_PosOffset;
    private Vector3 m_SizeEx;

    void Start()
    {
        m_Position = transform.position;
    }

    void Update()
    {
        Vector3 movedir = transform.position - m_Position;
        m_Position = transform.position;

        float xex = 0,zex = 0;
        if (movedir.x < -Mathf.Epsilon)
            xex = -leftExtDis;
        else if (movedir.x > Mathf.Epsilon)
            xex = rightExtDis;
        else
            xex = 0;
        if (movedir.z < -Mathf.Epsilon)
            zex = -bottomExtDis;
        else if (movedir.z > Mathf.Epsilon)
            zex = topExtDis;
        else
            zex = 0;
        m_PosOffset = new Vector3(xex*0.5f, 0, zex*0.5f);
        m_SizeEx = new Vector3(Mathf.Abs(xex), 0, Mathf.Abs(zex));
    }

    public override bool IsDetected(Bounds bounds)
    {
        m_Bounds.center = Position + m_PosOffset;
        m_Bounds.size = detectorSize + m_SizeEx;
        return bounds.Intersects(m_Bounds);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Bounds b = new Bounds(transform.position + m_PosOffset, detectorSize + m_SizeEx);
        b.DrawBounds(Color.yellow);
    }
#endif
}
