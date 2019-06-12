using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class SceneLightMapSetting : MonoBehaviour {
    public Texture2D []lightmapFar, lightmapNear;
    public LightmapsMode mode;
 
    public void SaveSettings()
    {
        mode = LightmapSettings.lightmapsMode;
        lightmapFar = null;
        lightmapNear = null;
        if (LightmapSettings.lightmaps != null && LightmapSettings.lightmaps.Length > 0)
        {
            int l = LightmapSettings.lightmaps.Length;
            lightmapFar = new Texture2D[l];
            lightmapNear = new Texture2D[l];
            for (int i = 0; i < l; i++)
            {
                lightmapFar[i] = LightmapSettings.lightmaps[i].lightmapColor;
                lightmapNear[i] = LightmapSettings.lightmaps[i].lightmapDir;
            }
        }
        RendererLightMapSetting[] savers = Transform.FindObjectsOfType<RendererLightMapSetting>();
        foreach(RendererLightMapSetting s in savers)
        {
            s.SaveSettings();
        }
   }
 
    public void LoadSettings()
    {
        LightmapSettings.lightmapsMode = mode;
        int l1 = (lightmapFar == null) ? 0 : lightmapFar.Length;
        int l2 = (lightmapNear == null) ? 0 : lightmapNear.Length;
        int l = (l1 < l2) ? l2 : l1;
        LightmapData[] lightmaps = null;
        if (l > 0)
        {
            lightmaps = new LightmapData[l];
            for (int i = 0; i < l; i++)
            {
                lightmaps[i] = new LightmapData();
                if (i < l1)
                    lightmaps[i].lightmapColor = lightmapFar[i];
                if (i < l2)
                    lightmaps[i].lightmapDir = lightmapNear[i];
            }
 
            LightmapSettings.lightmaps = lightmaps;
        }
    }
 
    void OnEnable()
    {
#if UNITY_EDITOR
        UnityEditor.Lightmapping.completed += SaveSettings;
#endif
    }
    void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.Lightmapping.completed -= SaveSettings;
#endif
    }
 
    void Awake () {
        if(Application.isPlaying){
            LoadSettings();
        }
    }
}