using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

[AddComponentMenu("UI/Circle Image")]
public class CircleImage : BaseImage {

	// Use this for initialization
	void Awake () {
        innerVertices = new List<Vector3>();
        outterVertices = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
        this.thickness = (float)Mathf.Clamp(this.thickness, 0, rectTransform.rect.width / 2);
	}

    [Tooltip("圆形或扇形填充比例")]
    [Range(0, 1)]
    public float fillPercent = 1f;
    [Tooltip("是否填充圆形")]
    public bool fill = true;
    [Tooltip("圆环宽度")]
    public float thickness = 5;
    [Tooltip("圆形")]
    [Range(3, 100)]
    public int segements = 20;

    private List<Vector3> innerVertices;
    private List<Vector3> outterVertices;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        innerVertices.Clear();
        outterVertices.Clear();

        float degreeDelta = (float)(2 * Mathf.PI / segements);
        int curSegements = (int)(segements * fillPercent);

        float tw = rectTransform.rect.width;
        float th = rectTransform.rect.height;
        float outerRadius = rectTransform.pivot.x * tw;
        float innerRadius = rectTransform.pivot.x * tw - thickness;

        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;

        float uvCenterX = (uv.x + uv.z) * 0.5f;
        float uvCenterY = (uv.y + uv.w) * 0.5f;
        float uvScaleX = (uv.z - uv.x) / tw;
        float uvScaleY = (uv.w - uv.y) / th;

        float curDegree = 0;
        UIVertex uiVertex;
        int verticeCount;
        int triangleCount;
        Vector2 curVertice;

        if (fill) //圆形
        {
            curVertice = Vector2.zero;
            verticeCount = curSegements + 1;
            uiVertex = new UIVertex();
            uiVertex.color = color;
            uiVertex.position = curVertice;
            uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
            vh.AddVert(uiVertex);

            for (int i = 1; i < verticeCount; i++)
            {
                float cosA = Mathf.Cos(curDegree);
                float sinA = Mathf.Sin(curDegree);
                curVertice = new Vector2(cosA * outerRadius, sinA * outerRadius);
                curDegree += degreeDelta;

                uiVertex = new UIVertex();
                uiVertex.color = color;
                uiVertex.position = curVertice;
                uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
                vh.AddVert(uiVertex);

                outterVertices.Add(curVertice);
            }

            triangleCount = curSegements*3;
            for (int i = 0, vIdx = 1; i < triangleCount - 3; i += 3, vIdx++)
            {
                vh.AddTriangle(vIdx, 0, vIdx+1);
            }
            if (fillPercent == 1)
            {
                //首尾顶点相连
                vh.AddTriangle(verticeCount - 1, 0, 1);
            }
        }
        else//圆环
        {
            verticeCount = curSegements*2;
            for (int i = 0; i < verticeCount; i += 2)
            {
                float cosA = Mathf.Cos(curDegree);
                float sinA = Mathf.Sin(curDegree);
                curDegree += degreeDelta;

                curVertice = new Vector3(cosA * innerRadius, sinA * innerRadius);
                uiVertex = new UIVertex();
                uiVertex.color = color;
                uiVertex.position = curVertice;
                uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
                vh.AddVert(uiVertex);
                innerVertices.Add(curVertice);

                curVertice = new Vector3(cosA * outerRadius, sinA * outerRadius);
                uiVertex = new UIVertex();
                uiVertex.color = color;
                uiVertex.position = curVertice;
                uiVertex.uv0 = new Vector2(curVertice.x * uvScaleX + uvCenterX, curVertice.y * uvScaleY + uvCenterY);
                vh.AddVert(uiVertex);
                outterVertices.Add(curVertice);
            }

            triangleCount = curSegements*3*2;
            for (int i = 0, vIdx = 0; i < triangleCount - 6; i += 6, vIdx += 2)
            {
                vh.AddTriangle(vIdx+1, vIdx, vIdx+3);
                vh.AddTriangle(vIdx, vIdx + 2, vIdx + 3);
            }
            if (fillPercent == 1)
            {
                //首尾顶点相连
                vh.AddTriangle(verticeCount - 1, verticeCount - 2, 1);
                vh.AddTriangle(verticeCount - 2, 0, 1);
            }
        }

    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Sprite sprite = overrideSprite;
        if (sprite == null)
            return true;

        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local);
        return Contains(local, outterVertices, innerVertices);
    }

    private bool Contains(Vector2 p, List<Vector3> outterVertices, List<Vector3> innerVertices)
    {
        var crossNumber = 0;
        RayCrossing(p, innerVertices, ref crossNumber);//检测内环
        RayCrossing(p, outterVertices, ref crossNumber);//检测外环
        return (crossNumber & 1) == 1;
    }

    /// <summary>
    /// 使用RayCrossing算法判断点击点是否在封闭多边形里
    /// </summary>
    /// <param name="p"></param>
    /// <param name="vertices"></param>
    /// <param name="crossNumber"></param>
    private void RayCrossing(Vector2 p, List<Vector3> vertices, ref int crossNumber)
    {
        for (int i = 0, count = vertices.Count; i < count; i++)
        {
            var v1 = vertices[i];
            var v2 = vertices[(i + 1) % count];

            //点击点水平线必须与两顶点线段相交
            if (((v1.y <= p.y) && (v2.y > p.y))
                || ((v1.y > p.y) && (v2.y <= p.y)))
            {
                //只考虑点击点右侧方向，点击点水平线与线段相交，且交点x > 点击点x，则crossNumber+1
                if (p.x < v1.x + (p.y - v1.y) / (v2.y - v1.y) * (v2.x - v1.x))
                {
                    crossNumber += 1;
                }
            }
        }
    }


}
