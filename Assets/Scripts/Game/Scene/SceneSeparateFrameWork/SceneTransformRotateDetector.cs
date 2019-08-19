using UnityEngine;
using System.Collections;

/// <summary>
/// 该触发器根据Transform的包围盒区域触发-且根据Transform面向角度旋转obb包围盒
/// </summary>
[ExecuteInEditMode]
public class SceneTransformRotateDetector : SceneDetectorBase
{
    public Vector3 detectorSize;
    public Vector3 posOffset;
    private Vector3[] vertexes = new Vector3[8];
    private Transform cameraTrans;
    
    void Start()
    {
        cameraTrans = Camera.main.transform;
    }

    void Update()
    {
        if (cameraTrans == null)
        {
            Debug.LogWarning("SceneTransformRotateDetector had not find main camera!");
            return;
        }
        var centerPos = transform.position+posOffset;
        var halfSize = detectorSize/2;
        vertexes[0].x = centerPos.x - halfSize.x;
        vertexes[0].y = centerPos.y - halfSize.y;
        vertexes[0].z = centerPos.z - halfSize.z;
        vertexes[1].x = centerPos.x + halfSize.x;
        vertexes[1].y = centerPos.y - halfSize.y;
        vertexes[1].z = centerPos.z - halfSize.z;
        vertexes[2].x = centerPos.x - halfSize.x;
        vertexes[2].y = centerPos.y - halfSize.y;
        vertexes[2].z = centerPos.z + halfSize.z;
        vertexes[3].x = centerPos.x + halfSize.x;
        vertexes[3].y = centerPos.y - halfSize.y;
        vertexes[3].z = centerPos.z + halfSize.z;

        vertexes[4].x = centerPos.x - halfSize.x;
        vertexes[4].y = centerPos.y + halfSize.y;
        vertexes[4].z = centerPos.z - halfSize.z;
        vertexes[5].x = centerPos.x + halfSize.x;
        vertexes[5].y = centerPos.y + halfSize.y;
        vertexes[5].z = centerPos.z - halfSize.z;
        vertexes[6].x = centerPos.x - halfSize.x;
        vertexes[6].y = centerPos.y + halfSize.y;
        vertexes[6].z = centerPos.z + halfSize.z;
        vertexes[7].x = centerPos.x + halfSize.x;
        vertexes[7].y = centerPos.y + halfSize.y;
        vertexes[7].z = centerPos.z + halfSize.z;
        
        var yAngle = cameraTrans.rotation.eulerAngles.y;
        Quaternion q = Quaternion.AngleAxis(yAngle-90, Vector3.up);
        for (int i = 0; i < vertexes.Length; i++)
        {
            Vector3 o = transform.position - vertexes[i];
            vertexes[i] = q * o;
            vertexes[i] = transform.position-vertexes[i];
        }
    }

    public override bool IsDetected(Bounds bounds)
    {
        for (int i = 0; i < vertexes.Length; i++)
        {
            if (bounds.Contains(vertexes[i]))
                return true;
        }
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < vertexes.Length; i++)
        {
            for (int j = 0; j < vertexes.Length; j++)
            {
                if (i != j && (vertexes[i].x==vertexes[j].x || (vertexes[i].y==vertexes[j].y) || (vertexes[i].z==vertexes[j].z)))
                    Gizmos.DrawLine(vertexes[i], vertexes[j]);
            }
        }
    }
#endif
}
