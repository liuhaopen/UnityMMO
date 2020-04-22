using UnityEngine;
using UnityEditor;

public class SplitTerrain : EditorWindow
{
    [MenuItem("Split/Split Terrain")]
    static void Init()
    {
        GetWindow<SplitTerrain>();
    }

    Transform transform;
    int xLen = 3; // Match with TerrainManager if using
    int zLen = 6;
    // Must be power of two plus 1
    int newHeightRes = 65; // Started with 513 in New Terrain
    int newDetailRes = 256; // Started with 1024 in New Terrain
    int newSplatRes = 128; // Started with 512 in New Terrain

    public void OnGUI()
    {
        transform = (Transform)EditorGUILayout.ObjectField("Terrain to split", transform, typeof(Transform), true);
        xLen = EditorGUILayout.IntField("Split x axis count", xLen);
        zLen = EditorGUILayout.IntField("Split z axis count", zLen);
        newHeightRes = EditorGUILayout.IntField("New heightmap res", newHeightRes);
        newDetailRes = EditorGUILayout.IntField("New detail res", newDetailRes);
        newSplatRes = EditorGUILayout.IntField("New splat res", newSplatRes);

        if (GUILayout.Button("Split!"))
        {
            if (transform == null)
            {
                Debug.LogWarning("No terrain found");
                return;
            }

            Terrain origTerrain = transform.GetComponent<Terrain>();
            if (origTerrain == null)
            {
                Debug.LogWarning("No terrain found on transform");
                return;
            }

            if (xLen < 1)
                xLen = 1;
            if (zLen < 1)
                zLen = 1;
            for (int x = 0; x < xLen; x++)
            {
                for (int z = 0; z < zLen; z++)
                {
                    EditorUtility.DisplayProgressBar("Splitting Terrain", "Copying heightmap, detail, splat, and trees", (float)((x * zLen) + z) / (xLen * zLen));
                    float xMin = origTerrain.terrainData.size.x / xLen * x;
                    float xMax = origTerrain.terrainData.size.x / xLen * (x + 1);
                    float zMin = origTerrain.terrainData.size.z / zLen * z;
                    float zMax = origTerrain.terrainData.size.z / zLen * (z + 1);
                    copyTerrain(origTerrain, string.Format("{0}{1}_{2}", origTerrain.name, x, z), xMin, xMax, zMin, zMax, newHeightRes, newDetailRes, newSplatRes);
                }
            }
            EditorUtility.ClearProgressBar();

            for (int x = 0; x < xLen; x++)
            {
                for (int z = 0; z < zLen; z++)
                {
                    GameObject center = GameObject.Find(string.Format("{0}{1}_{2}", origTerrain.name, x, z));
                    GameObject left = GameObject.Find(string.Format("{0}{1}_{2}", origTerrain.name, x - 1, z));
                    GameObject top = GameObject.Find(string.Format("{0}{1}_{2}", origTerrain.name, x, z + 1));
                    stitchTerrain(center, left, top);
                }
            }
        }
    }

    void copyTerrain(Terrain origTerrain, string newName, float xMin, float xMax, float zMin, float zMax, int heightmapResolution, int detailResolution, int alphamapResolution)
    {
        if (heightmapResolution < 33 || heightmapResolution > 4097)
        {
            Debug.Log("Invalid heightmapResolution " + heightmapResolution);
            return;
        }
        if (detailResolution < 0 || detailResolution > 4048)
        {
            Debug.Log("Invalid detailResolution " + detailResolution);
            return;
        }
        if (alphamapResolution < 16 || alphamapResolution > 2048)
        {
            Debug.Log("Invalid alphamapResolution " + alphamapResolution);
            return;
        }

        if (xMin < 0 || xMin > xMax || xMax > origTerrain.terrainData.size.x)
        {
            Debug.Log("Invalid xMin or xMax");
            return;
        }
        if (zMin < 0 || zMin > zMax || zMax > origTerrain.terrainData.size.z)
        {
            Debug.Log("Invalid zMin or zMax");
            return;
        }

        if (AssetDatabase.FindAssets(newName).Length != 0)
        {
            Debug.Log("Asset with name " + newName + " already exists");
            return;
        }

        TerrainData td = new TerrainData();
        GameObject gameObject = Terrain.CreateTerrainGameObject(td);
        Terrain newTerrain = gameObject.GetComponent<Terrain>();

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        // Must do this before Splat
        AssetDatabase.CreateAsset(td, "Assets/Resources/" + newName + ".asset");

        // Copy over all vars
        newTerrain.bakeLightProbesForTrees = origTerrain.bakeLightProbesForTrees;
        newTerrain.basemapDistance = origTerrain.basemapDistance;
        newTerrain.castShadows = origTerrain.castShadows;
        newTerrain.collectDetailPatches = origTerrain.collectDetailPatches;
        newTerrain.detailObjectDensity = origTerrain.detailObjectDensity;
        newTerrain.detailObjectDistance = origTerrain.detailObjectDistance;
        newTerrain.drawHeightmap = origTerrain.drawHeightmap;
        newTerrain.drawTreesAndFoliage = origTerrain.drawTreesAndFoliage;
        newTerrain.editorRenderFlags = origTerrain.editorRenderFlags;
        newTerrain.heightmapMaximumLOD = origTerrain.heightmapMaximumLOD;
        newTerrain.heightmapPixelError = origTerrain.heightmapPixelError;
        newTerrain.legacyShininess = origTerrain.legacyShininess;
        newTerrain.legacySpecular = origTerrain.legacySpecular;
        newTerrain.lightmapIndex = origTerrain.lightmapIndex;
        newTerrain.lightmapScaleOffset = origTerrain.lightmapScaleOffset;
        newTerrain.materialTemplate = origTerrain.materialTemplate;
        newTerrain.materialType = origTerrain.materialType;
        newTerrain.realtimeLightmapIndex = origTerrain.realtimeLightmapIndex;
        newTerrain.realtimeLightmapScaleOffset = origTerrain.realtimeLightmapScaleOffset;
        newTerrain.reflectionProbeUsage = origTerrain.reflectionProbeUsage;
        newTerrain.treeBillboardDistance = origTerrain.treeBillboardDistance;
        newTerrain.treeCrossFadeLength = origTerrain.treeCrossFadeLength;
        newTerrain.treeDistance = origTerrain.treeDistance;
        newTerrain.treeMaximumFullLODCount = origTerrain.treeMaximumFullLODCount;

        td.splatPrototypes = origTerrain.terrainData.splatPrototypes;
        td.treePrototypes = origTerrain.terrainData.treePrototypes;
        td.detailPrototypes = origTerrain.terrainData.detailPrototypes;

        // Get percent of original
        float xMinNorm = xMin / origTerrain.terrainData.size.x;
        float xMaxNorm = xMax / origTerrain.terrainData.size.x;
        float zMinNorm = zMin / origTerrain.terrainData.size.z;
        float zMaxNorm = zMax / origTerrain.terrainData.size.z;
        float dimRatio1, dimRatio2;

        // Height
        td.heightmapResolution = heightmapResolution;
        float[,] newHeights = new float[heightmapResolution, heightmapResolution];
        dimRatio1 = (xMax - xMin) / heightmapResolution;
        dimRatio2 = (zMax - zMin) / heightmapResolution;
        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                // Divide by size.y because height is stored as percentage
                // Note this is [j, i] and not [i, j] (Why?!)
                newHeights[j, i] = origTerrain.SampleHeight(new Vector3(xMin + (i * dimRatio1), 0, zMin + (j * dimRatio2))) / origTerrain.terrainData.size.y;
            }
        }
        td.SetHeightsDelayLOD(0, 0, newHeights);

        // Detail
        td.SetDetailResolution(detailResolution, 8); // Default? Haven't messed with resolutionPerPatch
        for (int layer = 0; layer < origTerrain.terrainData.detailPrototypes.Length; layer++)
        {
            int[,] detailLayer = origTerrain.terrainData.GetDetailLayer(
                    Mathf.FloorToInt(xMinNorm * origTerrain.terrainData.detailWidth),
                    Mathf.FloorToInt(zMinNorm * origTerrain.terrainData.detailHeight),
                    Mathf.FloorToInt((xMaxNorm - xMinNorm) * origTerrain.terrainData.detailWidth),
                    Mathf.FloorToInt((zMaxNorm - zMinNorm) * origTerrain.terrainData.detailHeight),
                    layer);
            int[,] newDetailLayer = new int[detailResolution, detailResolution];
            dimRatio1 = (float)detailLayer.GetLength(0) / detailResolution;
            dimRatio2 = (float)detailLayer.GetLength(1) / detailResolution;
            for (int i = 0; i < newDetailLayer.GetLength(0); i++)
            {
                for (int j = 0; j < newDetailLayer.GetLength(1); j++)
                {
                    newDetailLayer[i, j] = detailLayer[Mathf.FloorToInt(i * dimRatio1), Mathf.FloorToInt(j * dimRatio2)];
                }
            }
            td.SetDetailLayer(0, 0, layer, newDetailLayer);
        }

        // Splat
        td.alphamapResolution = alphamapResolution;
        float[,,] alphamaps = origTerrain.terrainData.GetAlphamaps(
            Mathf.FloorToInt(xMinNorm * origTerrain.terrainData.alphamapWidth),
            Mathf.FloorToInt(zMinNorm * origTerrain.terrainData.alphamapHeight),
            Mathf.FloorToInt((xMaxNorm - xMinNorm) * origTerrain.terrainData.alphamapWidth),
            Mathf.FloorToInt((zMaxNorm - zMinNorm) * origTerrain.terrainData.alphamapHeight));
        // Last dim is always origTerrain.terrainData.splatPrototypes.Length so don't ratio
        float[,,] newAlphamaps = new float[alphamapResolution, alphamapResolution, alphamaps.GetLength(2)];
        dimRatio1 = (float)alphamaps.GetLength(0) / alphamapResolution;
        dimRatio2 = (float)alphamaps.GetLength(1) / alphamapResolution;
        for (int i = 0; i < newAlphamaps.GetLength(0); i++)
        {
            for (int j = 0; j < newAlphamaps.GetLength(1); j++)
            {
                for (int k = 0; k < newAlphamaps.GetLength(2); k++)
                {
                    newAlphamaps[i, j, k] = alphamaps[Mathf.FloorToInt(i * dimRatio1), Mathf.FloorToInt(j * dimRatio2), k];
                }
            }
        }
        td.SetAlphamaps(0, 0, newAlphamaps);

        // Tree
        for (int i = 0; i < origTerrain.terrainData.treeInstanceCount; i++)
        {
            TreeInstance ti = origTerrain.terrainData.treeInstances[i];
            if (ti.position.x < xMinNorm || ti.position.x >= xMaxNorm)
                continue;
            if (ti.position.z < zMinNorm || ti.position.z >= zMaxNorm)
                continue;
            ti.position = new Vector3(((ti.position.x * origTerrain.terrainData.size.x) - xMin) / (xMax - xMin), ti.position.y, ((ti.position.z * origTerrain.terrainData.size.z) - zMin) / (zMax - zMin));
            newTerrain.AddTreeInstance(ti);
        }

        gameObject.transform.position = new Vector3(origTerrain.transform.position.x + xMin, origTerrain.transform.position.y, origTerrain.transform.position.z + zMin);
        gameObject.name = newName;

        // Must happen after setting heightmapResolution
        td.size = new Vector3(xMax - xMin, origTerrain.terrainData.size.y, zMax - zMin);

        AssetDatabase.SaveAssets();
    }

    void stitchTerrain(GameObject center, GameObject left, GameObject top)
    {
        if (center == null)
            return;
        Terrain centerTerrain = center.GetComponent<Terrain>();
        float[,] centerHeights = centerTerrain.terrainData.GetHeights(0, 0, centerTerrain.terrainData.heightmapResolution, centerTerrain.terrainData.heightmapResolution);
        if (top != null)
        {
            Terrain topTerrain = top.GetComponent<Terrain>();
            float[,] topHeights = topTerrain.terrainData.GetHeights(0, 0, topTerrain.terrainData.heightmapResolution, topTerrain.terrainData.heightmapResolution);
            if (topHeights.GetLength(0) != centerHeights.GetLength(0))
            {
                Debug.Log("Terrain sizes must be equal");
                return;
            }
            for (int i = 0; i < centerHeights.GetLength(1); i++)
            {
                centerHeights[centerHeights.GetLength(0) - 1, i] = topHeights[0, i];
            }
        }
        if (left != null)
        {
            Terrain leftTerrain = left.GetComponent<Terrain>();
            float[,] leftHeights = leftTerrain.terrainData.GetHeights(0, 0, leftTerrain.terrainData.heightmapResolution, leftTerrain.terrainData.heightmapResolution);
            if (leftHeights.GetLength(0) != centerHeights.GetLength(0))
            {
                Debug.Log("Terrain sizes must be equal");
                return;
            }
            for (int i = 0; i < centerHeights.GetLength(0); i++)
            {
                centerHeights[i, 0] = leftHeights[i, leftHeights.GetLength(1) - 1];
            }
        }
        centerTerrain.terrainData.SetHeights(0, 0, centerHeights);
    }
}