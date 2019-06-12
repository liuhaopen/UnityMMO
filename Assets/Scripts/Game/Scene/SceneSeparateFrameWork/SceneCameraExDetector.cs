using UnityEngine;
using System.Collections;

/// <summary>
/// 该触发器根据相机裁剪区域触发，且根据相机运动趋势改变裁剪区域
/// </summary>
public class SceneCameraExDetector : SceneDetectorBase
{
    #region 裁剪区域扩展趋势参数
    /// <summary>
    /// 水平方向扩展距离，当相机往左右移动时的裁剪区域扩展
    /// </summary>
    public float horizontalExtDis;
    /// <summary>
    /// 顶部方向扩展距离，当相机往前移动时的裁剪区域扩展
    /// </summary>
    public float topExtDis;
    /// <summary>
    /// 底部方向扩展距离，当相机往后移动时的裁剪区域扩展
    /// </summary>
    public float bottomExtDis;
    #endregion

    private Camera m_Camera;
    private Vector3 m_Position;

    private float m_LeftEx;
    private float m_RightEx;
    private float m_UpEx;
    private float m_DownEx;

    void Start()
    {
        m_Camera = gameObject.GetComponent<Camera>();
        m_Position = transform.position;
    }

    void Update()
    {
        Vector3 movedir = -transform.worldToLocalMatrix.MultiplyPoint(m_Position);
        m_Position = transform.position;
            
        m_LeftEx = movedir.x < -Mathf.Epsilon ? -horizontalExtDis : 0;
        m_RightEx = movedir.x > Mathf.Epsilon ? horizontalExtDis : 0;
        m_UpEx = movedir.y > Mathf.Epsilon ? topExtDis : 0;
        m_DownEx = movedir.y < -Mathf.Epsilon ? -bottomExtDis : 0;
    }

    public override bool IsDetected(Bounds bounds)
    {
        if (m_Camera == null)
            return false;
        
        return bounds.IsBoundsInCameraEx(m_Camera, m_LeftEx, m_RightEx, m_DownEx, m_UpEx);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Camera camera = gameObject.GetComponent<Camera>();
        if (camera)
            GizmosEx.DrawViewFrustumEx(camera, m_LeftEx, m_RightEx, m_DownEx, m_UpEx, Color.yellow);
    }
#endif
}
