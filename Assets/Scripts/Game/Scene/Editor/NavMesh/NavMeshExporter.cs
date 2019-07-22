using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine.AI;
using System.IO;
using System.Runtime.Serialization;

namespace Extend
{
public static class NavMeshExporter
{
    // [System.Runtime.InteropServices.DllImport("NavigatorForUnity", CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]    
    [System.Runtime.InteropServices.DllImport("NavigatorForUnity")]    
    private static extern int ConvertJsonToNavBinFile(string jsonContent, string savePath);   

    private static int MaxVertexPerPoly = 6;
    private const ushort NullIndex = 0xffff;
    
    private const float xzCellSize = 0.30f;      //these two gotten from recast demo
    private const float yCellSize = 0.20f;
    private static string ProgressBarName = "前方正在施工...";

    [UnityEditor.MenuItem("NavMeshExporter/ExportToNavBin")]
    static void ExportToNavBin()
    {
        ProgressBarName = "ExportToNavBin";
        string select_path = EditorUtility.SaveFilePanel("Export Navmesh As NavBin File", "", "navmesh", "bin");
        string outstring = GenNavMesh("json");
        EditorUtility.DisplayProgressBar(ProgressBarName, "正在转化为tile cache", 0.85f);
        int resultCode = ConvertJsonToNavBinFile(outstring, select_path);
        EditorUtility.ClearProgressBar();
        if (resultCode==1)
            EditorUtility.DisplayDialog("Tip", "Export Navmesh As NavBin File Succeed!", "ok");
        else
            EditorUtility.DisplayDialog("Error", "Export Navmesh As NavBin File Faild!", "close");
    }

    [UnityEditor.MenuItem("NavMeshExporter/Debug/ExportToJson")]
    static void ExportToJson()
    {
        ProgressBarName = "ExportToJson";
        string outstring = GenNavMesh("json");
        string select_path = EditorUtility.SaveFilePanel("Export Navmesh As Json File", "", "navmesh", "json");
        EditorUtility.DisplayProgressBar(ProgressBarName, "开始写入json文件", 0.95f);
        System.IO.File.WriteAllText(select_path, outstring);
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("Tip", "Export Navmesh As Json File Succeed!", "ok");
    }

    [UnityEditor.MenuItem("NavMeshExporter/Debug/ExportToObj")]
    static void ExportToObj()
    {
        ProgressBarName = "ExportToObj";
        string outstring = GenNavMesh("obj");
        string select_path = EditorUtility.SaveFilePanel("Export Navmesh As Obj File", "", "navmesh", "obj");
        EditorUtility.DisplayProgressBar(ProgressBarName, "开始写入obj文件", 0.95f);
        System.IO.File.WriteAllText(select_path, outstring);
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("Tip", "Export Navmesh As Obj File Succeed!", "ok");
    }

    static void RevertX(ref List<Vector3> vertices)
    {
        for (int i = 0; i < vertices.Count; i++)
            vertices[i] = new Vector3(-vertices[i].x, vertices[i].y, vertices[i].z);
    }
    
    static void GetBounds(List<Vector3> vertices, ref float[] boundsMin, ref float[] boundsMax)
    {
        for (int axis = 0; axis <= 2; axis++)
        {
            float min_value = Int32.MaxValue;
            float max_value = Int32.MinValue;
            for (int i = 0; i < vertices.Count; i++)
            {
                // float test_value = axis==1?-vertices[i][axis]:vertices[i][axis];//x轴反转
                float test_value = vertices[i][axis];//x轴反转
                if (test_value < min_value)
                {
                    min_value = test_value;
                }

                if (test_value > max_value)
                {
                    max_value = test_value;
                }
            }
            boundsMin[axis] = min_value;
            boundsMax[axis] = max_value;
        }
    }

    static string GenNavMeshOriginJson()
    {
        UnityEngine.AI.NavMeshTriangulation navtri = UnityEngine.AI.NavMesh.CalculateTriangulation();
        string outnav = "";
        outnav = "{\"v\":[\n";
        for (int i = 0; i < navtri.vertices.Length; i++)
        {
            if (i > 0)
                outnav += ",\n";

            outnav += "[" + navtri.vertices[i].x + "," + navtri.vertices[i].y + "," + navtri.vertices[i].z + "]";
        }
        outnav += "\n],\"p\":[\n";

        for (int i = 0; i < navtri.indices.Length; i++)
        {
            if (i > 0)
                outnav += ",\n";

            int index = navtri.indices[i];
            outnav += index.ToString();
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "s"+index;
            sphere.transform.position = navtri.vertices[index];
        }
        outnav += "\n]}";
        return outnav;
    }

    static void GetShardVertex(List<int> polysA, List<int> polysB, ref List<int> shardVertex)
    {
        for (int i=0; i<polysA.Count; i++)
        {
            for (int j=0; j<polysB.Count; j++)
            {
                if (polysA[i] == polysB[j] && polysA[i] != NullIndex)
                {
                    // Debug.Log("polysA[i]:"+polysA[i]+" b:"+polysB[j]+" i:"+i);
                    shardVertex.Add(i);
                }
            }
        }
    }

    static void MergePolyAndNeighbor(ref List<List<int>> polys, List<List<int>> neighbor)
    {
        for (int i=0; i < polys.Count; i++)
        {
            var poly = polys[i];
            List<int> neighborVal = neighbor[i];
            for (int j=0; j<MaxVertexPerPoly; j++)
            {
                poly.Add(neighborVal[j]);
            }
        }
    }

    static int GetVaildVertexNum(List<int> poly)
    {
        int num = 0;
        for (int i=0; i<poly.Count; i++)
        {
            if (poly[i]==NullIndex)
                break;
            num++;
        }
        return num;
    }

    static void GenNeighbor(List<List<int>> polys, ref List<List<int> > neighbor, float progress)
    {
        for (int i=0; i < polys.Count; i++)
        {
            float detailPro = progress+0.1f*(i+1)/polys.Count;
            EditorUtility.DisplayProgressBar(ProgressBarName, "获取邻近多边形数据("+(i+1)+"/"+polys.Count+")", detailPro);
            // Debug.Log("polys.Count : "+polys.Count+ " I:"+i);
            neighbor.Add(new List<int>());
            for (int j=0; j<MaxVertexPerPoly; j++)
                neighbor[i].Add(NullIndex);
            for (int j=0; j < polys.Count; j++)
            {
                if (i==j)
                    continue;
                List<int> shardVertex = new List<int>();
                GetShardVertex(polys[i], polys[j], ref shardVertex);
                if (shardVertex.Count>2)
                {
                    EditorUtility.DisplayDialog("Error", "More than two vertexes shard between poly "+i+" and "+j, "ok");
                    return;
                }
                // Debug.Log("shardVertex.Count : "+shardVertex.Count);
                if (shardVertex.Count==2)
                {
                    // Debug.Log("shard vertex : "+shardVertex[0]+" "+shardVertex[1]+" i:"+i+" j:"+j);
                    if (shardVertex[0]==0)
                    {
                        if (shardVertex[1]==1)
                            neighbor[i][0] = j;
                        else
                            neighbor[i][GetVaildVertexNum(polys[i])-1] = j;
                    }
                    else
                    {
                        neighbor[i][shardVertex[0]] = j;
                    }
                }
            }
        }
    }

    static private void AddPolyByVertex(List<int> vertexList, ref List<List<int>> polys, bool isReverse)
    {
        if (MaxVertexPerPoly < vertexList.Count)
            MaxVertexPerPoly = vertexList.Count;
        var polyVert = new List<int>(vertexList);
        if (isReverse)
            polyVert.Reverse();
        // if (isNeedFillNullIndex)
        // {
        //     for (int ii=polyVert.Count; ii<MaxVertexPerPoly; ii++)
        //         polyVert.Add(NullIndex);
        // }
        polys.Add(polyVert);
    }

    //TODO: 导出area字段
    static string GenNavMesh(string style)
    {
        float progress = 0;
        float detailPro = 0;
        EditorUtility.DisplayProgressBar(ProgressBarName, "获取NavMesh三角形数据", progress);
        // return GenNavMeshOriginJson(); 
        UnityEngine.AI.NavMeshTriangulation navtri = UnityEngine.AI.NavMesh.CalculateTriangulation();
        //{
        //    var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    var mf = obj.GetComponent<MeshFilter>();
        //    Mesh m = new Mesh();
        //    m.vertices = navtri.vertices;
        //    m.triangles = navtri.indices;
        //    mf.mesh = m;
        //}
        Dictionary<int, int> indexmap = new Dictionary<int, int>();
        List<Vector3> repos = new List<Vector3>();
        progress += 0.1f;
        float mergeSameVertexSumPro = 0.1f;
        for (int i = 0; i < navtri.vertices.Length; i++)
        {
            detailPro = (progress+mergeSameVertexSumPro*(i+1)/navtri.vertices.Length);
            EditorUtility.DisplayProgressBar(ProgressBarName, "合并相同顶点("+(i+1)+"/"+navtri.vertices.Length+")", detailPro);
            int ito = -1;
            for (int j = 0; j < repos.Count; j++)
            {
                if (Vector3.Distance(navtri.vertices[i], repos[j]) < 0.01)
                {
                    ito = j;
                    break;
                }
            }
            if (ito < 0)
            {
                indexmap[i] = repos.Count;
                repos.Add(navtri.vertices[i]);
            }
            else
            {
                indexmap[i] = ito;
            }
        }
        progress = detailPro;
        detailPro = 0.0f;
        // for(int i=0; i<navtri.areas.Length; i++)
        //     Debug.Log("area : "+navtri.areas[i].ToString());
        //关系是 index 公用的三角形表示他们共同组成多边形
        //多边形之间的连接用顶点位置识别
        List<int> polylast = new List<int>();
        // List<int[]> polys = new List<int[]>();
        List<List<int>> polys = new List<List<int>>();
        MaxVertexPerPoly = 6;
        float makePolySumPro = 0.1f;
        for (int i = 0; i < navtri.indices.Length / 3; i++)
        {
            detailPro = progress+makePolySumPro*i/navtri.indices.Length/3;
            EditorUtility.DisplayProgressBar(ProgressBarName, "合并邻近三角形("+i+"/"+(navtri.indices.Length/3)+")", detailPro);
            int i0 = navtri.indices[i * 3 + 0];
            int i1 = navtri.indices[i * 3 + 1];
            int i2 = navtri.indices[i * 3 + 2];
            // i0 = indexmap[i0];
            // i1 = indexmap[i1];
            // i2 = indexmap[i2];
            if (polylast.Contains(i0) || polylast.Contains(i1) || polylast.Contains(i2))
            {
                if (polylast.Contains(i0) == false)
                    polylast.Add(i0);
                if (polylast.Contains(i1) == false)
                    polylast.Add(i1);
                if (polylast.Contains(i2) == false)
                    polylast.Add(i2);
            }
            else
            {
                if (polylast.Count > 0)
                {
                    AddPolyByVertex(polylast, ref polys, style == "json");
                }
                polylast.Clear();
                polylast.Add(i0);
                polylast.Add(i1);
                polylast.Add(i2);
            }
        }
        if (polylast.Count > 0)
        {
            AddPolyByVertex(polylast, ref polys, style == "json");
        }
        progress = detailPro;
        detailPro = 0.0f;
        string outnav = "";
        if (style == "json")
        {
            for (int i=0; i<polys.Count; i++)
            {
                for (int j=0; j<polys[i].Count; j++)
                {
                    if (polys[i][j]!=NullIndex)
                        polys[i][j] = indexmap[polys[i][j]];
                }
                for (int ii=polys[i].Count; ii<MaxVertexPerPoly; ii++)
                    polys[i].Add(NullIndex);
            }
            List<List<int> > neighbor = new List<List<int>>();
            GenNeighbor(polys, ref neighbor, progress);
            progress+=0.1f;
            MergePolyAndNeighbor(ref polys, neighbor);
            float[] boundsMin = new float[3];
            float[] boundsMax = new float[3];
            EditorUtility.DisplayProgressBar(ProgressBarName, "反转所有顶点X轴", progress+=0.1f);
            RevertX(ref repos);
            EditorUtility.DisplayProgressBar(ProgressBarName, "计算包围盒", progress+=0.1f);
            GetBounds(repos, ref boundsMin, ref boundsMax);
            // Debug.Log("max bounds :" + boundsMax[1].ToString()+" boundsMin:"+boundsMin[1].ToString());
            for (int i = 0; i < repos.Count; i++)
            {
                ushort x = (ushort)Math.Round((repos[i].x - boundsMin[0]) / xzCellSize);
                ushort y = (ushort)Math.Round((repos[i].y - boundsMin[1]) / yCellSize);
                ushort z = (ushort)Math.Round((repos[i].z - boundsMin[2]) / xzCellSize);
                repos[i] = new Vector3(x, y, z);
            }
            
            outnav = "{";
            outnav += "\"nvp\":"+MaxVertexPerPoly+",\n";
            outnav += "\"cs\":"+xzCellSize+",\n";
            outnav += "\"ch\":"+yCellSize+",\n";
            NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex( 0 );
            outnav += "\"agentHeight\":"+settings.agentHeight+",\n";
            outnav += "\"agentRadius\":"+settings.agentRadius+",\n";
            outnav += "\"agentMaxClimb\":"+settings.agentClimb+",\n";
            outnav += "\"bmin\":["+boundsMin[0]+", "+boundsMin[1]+", "+boundsMin[2]+"],\n";
            outnav += "\"bmax\":["+boundsMax[0]+", "+boundsMax[1]+", "+boundsMax[2]+"],\n";
            outnav += "\"v\":[\n";
            // List<List<uint>> vertexes = new List<List<uint>>();
            detailPro = 0.0f;
            for (int i = 0; i < repos.Count; i++)
            {
                detailPro = progress+0.1f*(i+1)/repos.Count;
                EditorUtility.DisplayProgressBar(ProgressBarName, "生成json顶点数据("+(i+1)+"/"+repos.Count+")", detailPro);
                // List<uint> vertex = new List<uint>();
                // vertex.Add((uint)repos[i].x);
                // vertex.Add((uint)repos[i].y);
                // vertex.Add((uint)repos[i].z);
                // vertexes.Add(vertex);
                if (i > 0)
                    outnav += ",\n";
                outnav += "[" + (repos[i].x) + "," + repos[i].y + "," + repos[i].z + "]";
            }
            outnav += "\n],\n\"p\":[\n";
            progress = detailPro;
            detailPro = 0.0f;
            for (int i = 0; i < polys.Count; i++)
            {
                detailPro = progress+0.1f*(i+1)/polys.Count;
                EditorUtility.DisplayProgressBar(ProgressBarName, "生成json多边形数据("+(i+1)+"/"+polys.Count+")", detailPro);
                // string outs = indexmap[polys[i][0]].ToString();
                string outs = polys[i][0].ToString();
                for (int j = 1; j < MaxVertexPerPoly; j++)
                {
                    var verIndex = polys[i][j];
                    // if (verIndex!=NullIndex)
                        // verIndex = indexmap[verIndex];
                    outs += "," + verIndex;
                }
                for (int j = MaxVertexPerPoly; j < MaxVertexPerPoly*2; j++)
                {
                    outs += "," + polys[i][j];
                }
                if (i > 0)
                    outnav += ",\n";
                outnav += "[" + outs + "]";
            }
            outnav += "\n]}";
            // Debug.Log("outnav : "+outnav);
        }
        else if (style == "obj")
        {
            detailPro = 0.0f;
            outnav = "";
            for (int i = 0; i < repos.Count; i++)
            {//unity 对obj 做了 x轴 -1
                detailPro = progress+0.4f*(i+1)/polys.Count;
                EditorUtility.DisplayProgressBar(ProgressBarName, "生成json顶点数据("+(i+1)+"/"+repos.Count+")", detailPro);
                outnav += "v " + (repos[i].x * -1) + " " + repos[i].y + " " + repos[i].z + "\r\n";
                // outnav += "v " + (repos[i].x) + " " + repos[i].y + " " + repos[i].z + "\r\n";
            }
            outnav += "\r\n";
            progress = detailPro;
            detailPro = 0.0f;
            for (int i = 0; i < polys.Count; i++)
            {
                detailPro = progress+0.5f*(i+1)/polys.Count;
                EditorUtility.DisplayProgressBar(ProgressBarName, "生成json多边形数据("+(i+1)+"/"+polys.Count+")", detailPro);
                outnav += "f";
                //逆向
                for (int j = polys[i].Count - 1; j >= 0; j--)
                // for (int j = 0; j < polys[i].Count; j++)
                {
                    outnav += " " + (indexmap[polys[i][j]] + 1);
                }
                outnav += "\r\n";
            }
        }
        return outnav;
    }
}

}