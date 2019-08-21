using UnityEngine;
using UnityEngine.UI;

namespace Cocos
{
    public class Helper
    {
        public static float GetOpacity(Transform target, IColorAttrCatcher attrCatcher=null)
        {
            float opacity = 1.0f;
            if (target != null)
            {
                if (attrCatcher == null)
                {
                    var renderer = target.GetComponent<MeshRenderer>();
                    Material material = null;
                    if (renderer != null)
                        material = renderer.material;
                    if (material != null)
                    {
                        opacity = material.color.a * 255.0f;
                    }
                    else
                    {
                        Graphic graphic = target.GetComponent<Image>();
                        if (graphic == null)
                            graphic = target.GetComponent<Text>();
                        if (graphic != null)
                            opacity = graphic.color.a * 255.0f;
                    }
                }
                else
                {
                    var color = attrCatcher.GetColor(target);
                    opacity = color.a * 255.0f;
                }
            }
            return opacity;
        }

        public static void SetOpacity(Transform target, float opacity, IColorAttrCatcher attrCatcher=null)
        {
            Color color;
            if (attrCatcher == null)
            {
                var renderer = target.GetComponent<MeshRenderer>();
                Material material = null;
                if (renderer != null)
                    material = renderer.material;
                if (material != null)
                {
                    color = material.color;
                    color.a = opacity;
                    material.color = color;
                }
                else 
                {
                    Graphic graphic = target.GetComponent<Image>();
                    if (graphic != null)
                    {
                        color = graphic.color;
                        color.a = opacity;
                        graphic.color = color;
                    }
                }
            }
            else
            {
                color = attrCatcher.GetColor(target);
                color.a = opacity;
                attrCatcher.SetColor(target, color);
            }
        }
    }
}