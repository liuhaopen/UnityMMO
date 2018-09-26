using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
//copy from https://github.com/huailiang/terrain_proj

public class trnconst
{
    
    public const string sep = "_";

    /// <summary>
    /// 切割成4x4
    /// </summary>
    public const int SLICE = 4;
}
public class TerrainSliceEditor : Editor
{
    const string SavePath = "Assets/AssetBundleRes/scene/";
    //开始分割地形
    [MenuItem("Terrain/Slicing")]
    private static void Slicing()
    {
        Terrain terrain = GameObject.FindObjectOfType<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("找不到地形!");
            return;
        }
        string savepath = SavePath + terrain.name + "/";
        if (Directory.Exists(savepath)) Directory.Delete(savepath, true);
        Directory.CreateDirectory(savepath);
        TerrainData terrainData = terrain.terrainData;

        Vector3 oldSize = terrainData.size;
        Vector3 oldPos = terrain.transform.position;
        oldPos = new Vector3(oldPos.x - oldSize.x, oldPos.y, oldPos.z);

        //得到新地图分辨率
        int newAlphamapResolution = terrainData.alphamapResolution / trnconst.SLICE;
        SplatPrototype[] splatProtos = terrainData.splatPrototypes;


        var detailProtos = terrainData.detailPrototypes;
        var treeProtos = terrainData.treePrototypes;
        var treeInst = terrainData.treeInstances;
        var grassStrength = terrainData.wavingGrassStrength;
        var grassAmount = terrainData.wavingGrassAmount;
        var grassSpeed = terrainData.wavingGrassSpeed;
        var grassTint = terrainData.wavingGrassTint;

        int terrainsWide = trnconst.SLICE;
        int terrainsLong = trnconst.SLICE;

        int newDetailResolution = terrainData.detailResolution / trnconst.SLICE;
        int resolutionPerPatch = 8;
        //设置高度
        int xBase = terrainData.heightmapWidth / terrainsWide;
        int yBase = terrainData.heightmapHeight / terrainsLong;
        TerrainData[] data = new TerrainData[terrainsWide * terrainsLong];
        Dictionary<int, List<TreeInstance>> map = new Dictionary<int, List<TreeInstance>>();
        int arrayPos = 0;
        try
        {
            //循环宽和长,生成小块地形
            for (int x = 0; x < terrainsWide; ++x)
            {
                for (int y = 0; y < terrainsLong; ++y)
                {
                    //创建资源
                    TerrainData newData = new TerrainData();
                    map[arrayPos] = new List<TreeInstance>();
                    data[arrayPos++] = newData;
                    string terrainName = terrain.name + trnconst.sep + y + trnconst.sep + x + ".asset";
                    AssetDatabase.CreateAsset(newData, savepath + terrainName);

                    EditorUtility.DisplayProgressBar("正在分割地形", terrainName, (float)(x * terrainsWide + y) / (float)(terrainsWide * terrainsLong));

                    //设置分辨率参数
                    newData.heightmapResolution = (terrainData.heightmapResolution - 1) / trnconst.SLICE;
                    newData.alphamapResolution = terrainData.alphamapResolution / trnconst.SLICE;
                    newData.baseMapResolution = terrainData.baseMapResolution / trnconst.SLICE;

                    //设置大小
                    newData.size = new Vector3(oldSize.x / terrainsWide, oldSize.y, oldSize.z / terrainsLong);

                    //设置地形原型
                    SplatPrototype[] newSplats = new SplatPrototype[splatProtos.Length];
                    for (int i = 0; i < splatProtos.Length; ++i)
                    {
                        newSplats[i] = new SplatPrototype();
                        newSplats[i].texture = splatProtos[i].texture;
                        newSplats[i].tileSize = splatProtos[i].tileSize;

                        float offsetX = (newData.size.x * x) % splatProtos[i].tileSize.x + splatProtos[i].tileOffset.x;
                        float offsetY = (newData.size.z * y) % splatProtos[i].tileSize.y + splatProtos[i].tileOffset.y;
                        newSplats[i].tileOffset = new Vector2(offsetX, offsetY);
                    }
                    newData.splatPrototypes = newSplats;

                    //设置混合贴图
                    float[,,] alphamap = new float[newAlphamapResolution, newAlphamapResolution, splatProtos.Length];
                    alphamap = terrainData.GetAlphamaps(x * newData.alphamapWidth, y * newData.alphamapHeight, newData.alphamapWidth, newData.alphamapHeight);
                    newData.SetAlphamaps(0, 0, alphamap);

                    float[,] height = terrainData.GetHeights(xBase * x, yBase * y, xBase + 1, yBase + 1);
                    newData.SetHeights(0, 0, height);

                    newData.SetDetailResolution(newDetailResolution, resolutionPerPatch);

                    int[] layers = terrainData.GetSupportedLayers(x * newData.detailWidth - 1, y * newData.detailHeight - 1, newData.detailWidth, newData.detailHeight);
                    int layerLength = layers.Length;

                    DetailPrototype[] tempDetailProtos = new DetailPrototype[layerLength];
                    for (int i = 0; i < layerLength; i++)
                        tempDetailProtos[i] = detailProtos[layers[i]];
                    newData.detailPrototypes = tempDetailProtos;

                    for (int i = 0; i < layerLength; i++)
                        newData.SetDetailLayer(0, 0, i, terrainData.GetDetailLayer(x * newData.detailWidth, y * newData.detailHeight, newData.detailWidth, newData.detailHeight, layers[i]));

                    newData.wavingGrassStrength = grassStrength;
                    newData.wavingGrassAmount = grassAmount;
                    newData.wavingGrassSpeed = grassSpeed;
                    newData.wavingGrassTint = grassTint;
                    newData.treePrototypes = treeProtos;
                }
            }

            int newWidth = (int)oldSize.x / terrainsWide;
            int newLength = (int)oldSize.z / terrainsLong;
            for (int i = 0; i < treeInst.Length; i++)
            {
                Vector3 origPos = Vector3.Scale(new Vector3(oldSize.x, 1, oldSize.z), new Vector3(treeInst[i].position.x, treeInst[i].position.y, treeInst[i].position.z));
                int column = Mathf.FloorToInt(origPos.x / newWidth);
                int row = Mathf.FloorToInt(origPos.z / newLength);
                Vector3 tempVect = new Vector3((origPos.x - newWidth * column) / newWidth, origPos.y, (origPos.z - newLength * row) / newWidth);

                TreeInstance tempTree = new TreeInstance();
                tempTree.position = tempVect;
                tempTree.widthScale = treeInst[i].widthScale;
                tempTree.heightScale = treeInst[i].heightScale;
                tempTree.color = treeInst[i].color;
                tempTree.rotation = treeInst[i].rotation;
                tempTree.lightmapColor = treeInst[i].lightmapColor;
                int indx = (column * terrainsWide) + row;
                tempTree.prototypeIndex = 0;
                map[indx].Add(tempTree);
            }
            for (int i = 0; i < terrainsWide * terrainsLong; i++)
            {
                data[i].treeInstances = map[i].ToArray();
                data[i].RefreshPrototypes();
            }
            WriteTerrainInfo(terrain, savepath + terrain.name + ".bytes");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }



    private static void WriteTerrainInfo(Terrain terrain, string path)
    {
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        BinaryWriter writer = new BinaryWriter(fs);

        //这里我分割的宽和长度是一样的.这里求出循环次数,TerrainLoad.SIZE要生成的地形宽度,长度相同
        //高度地图的分辨率只能是2的N次幂加1,所以SLICING_SIZE必须为2的N次幂
        int size = (int)terrain.terrainData.size.x / trnconst.SLICE;
        Vector3 pos = terrain.transform.position;
        writer.Write(pos.x);
        writer.Write(pos.y);
        writer.Write(pos.z);
        writer.Write(size);
        writer.Write(terrain.treeDistance);
        writer.Write(terrain.treeBillboardDistance);
        writer.Write(terrain.treeCrossFadeLength);
        writer.Write(terrain.treeMaximumFullLODCount);
        writer.Write(terrain.detailObjectDistance);
        writer.Write(terrain.detailObjectDensity);
        writer.Write(terrain.heightmapPixelError);
        writer.Write(terrain.heightmapMaximumLOD);
        writer.Write(terrain.basemapDistance);
        writer.Write(terrain.lightmapIndex);
        writer.Write(terrain.castShadows);
        WriteParts(writer);
        writer.Flush();
        writer.Close();
        fs.Close();
    }

    private static void WriteParts(BinaryWriter writer)
    {
        GameObject go = GameObject.Find("parts");
        int cnt = go.transform.childCount;
        writer.Write(cnt);
        for (int i = 0; i < cnt; i++)
        {
            Transform tf = go.transform.GetChild(i);
            writer.Write(tf.position.x);
            writer.Write(tf.position.y);
            writer.Write(tf.position.z);
            writer.Write(tf.eulerAngles.x);
            writer.Write(tf.eulerAngles.y);
            writer.Write(tf.eulerAngles.z);
            writer.Write(tf.localScale.x);
            writer.Write(tf.localScale.y);
            writer.Write(tf.localScale.z);
            MeshRenderer render = tf.gameObject.GetComponent<MeshRenderer>();
            if (render == null)
            {
                writer.Write(-1);
                writer.Write(1.0f);
                writer.Write(1.0f);
                writer.Write(0f);
                writer.Write(0f);
            }
            else
            {
                writer.Write(render.lightmapIndex);
                writer.Write(render.lightmapScaleOffset.x);
                writer.Write(render.lightmapScaleOffset.y);
                writer.Write(render.lightmapScaleOffset.z);
                writer.Write(render.lightmapScaleOffset.w);
            }
            Object oobj = PrefabUtility.GetCorrespondingObjectFromSource(tf.gameObject);
            string path = AssetDatabase.GetAssetPath(oobj);
            path = path.Replace(SavePath, "");
            path = path.Remove(path.LastIndexOf('.'));
            writer.Write(path);
        }
    }

}