using System.Collections.Generic;
using UnityEngine;

namespace UnityMMO
{    
public class EffectUtil
{
    public static void SetHitEffectColor(Transform trans, Color color, bool genMaterialIfNone)
    {
        var renderers = trans.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            var renderer = renderers[i];
            var materials = renderer.materials;
            int hitMaterialIndex = XLuaFramework.Util.FindMaterial(materials, "function_hitfresnel");
            if (genMaterialIfNone && hitMaterialIndex == -1)
            {
                hitMaterialIndex = materials.Length;
                var hitMaterial = ResMgr.GetInstance().GetMeterial("function_hitfresnel");
                hitMaterial.name = "function_hitfresnel";
                List<Material> newMaterials = new List<Material>(materials);
                newMaterials.Add(hitMaterial);
                renderer.materials = newMaterials.ToArray();
            }
            if (hitMaterialIndex != -1)
            {
                var property = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(property, hitMaterialIndex);
                property.SetColor("_TintColor", color);
                renderer.SetPropertyBlock(property, hitMaterialIndex);
            }
        }
    }
}
}