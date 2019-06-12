using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class RendererLightMapSetting : MonoBehaviour {
    public int lightmapIndex;
    public Vector4 lightmapScaleOffset;
 
    public void SaveSettings()
    {
        if(!IsLightMapGo(gameObject)){
            return;
        }
        Renderer renderer = GetComponent<Renderer>();
        lightmapIndex = renderer.lightmapIndex;
        lightmapScaleOffset = renderer.lightmapScaleOffset;
    }
 
    public void LoadSettings()
    {
        if(!IsLightMapGo(gameObject)){
            return;
        }
 
        Renderer renderer = GetComponent<Renderer>();
        renderer.lightmapIndex = lightmapIndex;
        renderer.lightmapScaleOffset = lightmapScaleOffset;
    }
 
    public static bool IsLightMapGo(GameObject go){
        if(go == null){
            return false;
        }
        Renderer renderer = go.GetComponent<Renderer>();
        if(renderer == null){
            return false;
        }
        return true;
    }
 
    void Awake () {
        if (Application.isPlaying) {
            LoadSettings ();
        }
    }
}