using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cocos
{
    //因为需要为各种不同的组件获取或设置颜色，所以FadeIn等action支持传入实现过本接口的实例，可以参考ColorAttrCatcherTextMeshPro.cs实现你自己的组件
    public interface IColorAttrCatcher
    {
        System.Func<Transform, Color> GetColor{get;}
        System.Action<Transform, Color> SetColor{get;}
    }

    //
    // FadeTo
    //
    public class FadeTo : ActionInterval
    {
        protected Material material;
        protected Graphic graphic;
        protected float toOpacity;
        public float fromOpacity;
        protected IColorAttrCatcher attrCatcher;
        
        public static FadeTo Create(float d, float opacity, IColorAttrCatcher attrCatcher=null)
        {
            FadeTo action = new FadeTo();
            if (action != null && action.InitWithDuration(d, opacity, attrCatcher))
                return action;
            return null;
        }
        
        public bool InitWithDuration(float duration, float opacity, IColorAttrCatcher attrCatcher=null)
        {
            if (base.InitWithDuration(duration))
            {
                this.toOpacity = opacity;
                this.attrCatcher = attrCatcher;
                return true;
            }
            return false;
        }

        public override Action Clone()
        {
            return FadeTo.Create(duration, toOpacity);
        }

        public override Action Reverse()
        {
            Debug.LogError("reverse() not supported in FadeTo");
            return null;
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            if (target != null)
            {
                if (attrCatcher == null)
                {
                    var renderer = target.GetComponent<MeshRenderer>();
                    if (renderer != null)
                        material = renderer.material;
                    if (material != null)
                    {
                        fromOpacity = material.color.a * 255.0f;
                    }
                    else
                    {
                        graphic = target.GetComponent<Image>();
                        if (graphic == null)
                            graphic = target.GetComponent<Text>();
                        if (graphic != null)
                            fromOpacity = graphic.color.a * 255.0f;
                    }
                }
                else
                {
                    var color = attrCatcher.GetColor(target);
                    fromOpacity = color.a * 255.0f;
                }
            }
        }

        public override void Update(float time)
        {
            Color color;
            if (attrCatcher == null)
            {
                if (material != null)
                {
                    color = material.color;
                    color.a = (fromOpacity + (toOpacity - fromOpacity)*time)/255.0f;
                    material.color = color;
                }
                else if (graphic != null)
                {
                    color = graphic.color;
                    color.a = (fromOpacity + (toOpacity - fromOpacity)*time)/255.0f;
                    graphic.color = color;
                }
            }
            else
            {
                color = attrCatcher.GetColor(target);
                color.a = (fromOpacity + (toOpacity - fromOpacity)*time)/255.0f;
                attrCatcher.SetColor(target, color);
            }
        }
    }

    //
    // FadeIn
    //
    public class FadeIn : FadeTo
    {
        FadeTo reverseAction;
        public static FadeIn Create(float d, IColorAttrCatcher attrCatcher=null)
        {
            FadeIn action = new FadeIn();
            if (action != null && action.InitWithDuration(d, 255.0f, attrCatcher))
                return action;
            return null;
        }
        
        public override Action Clone()
        {
            return FadeIn.Create(duration);
        }

        public void SetReverseAction(FadeTo ac)
        {
            reverseAction = ac;
        }

        public override Action Reverse()
        {
            var action = FadeOut.Create(duration);
            action.SetReverseAction(this);
            return action;
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            if (reverseAction != null)
                toOpacity = reverseAction.fromOpacity;
            else
                toOpacity = 255.0f;
        }
    }
    
    //
    // FadeOut
    //
    public class FadeOut : FadeTo
    {
        FadeTo reverseAction;
        public static FadeOut Create(float d, IColorAttrCatcher attrCatcher=null)
        {
            FadeOut action = new FadeOut();
            if (action != null && action.InitWithDuration(d, 0.0f, attrCatcher))
                return action;
            return null;
        }
        
        public override Action Clone()
        {
            return FadeOut.Create(duration);
        }

        public void SetReverseAction(FadeTo ac)
        {
            reverseAction = ac;
        }

        public override Action Reverse()
        {
            var action = FadeIn.Create(duration);
            action.SetReverseAction(this);
            return action;
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            if (reverseAction != null)
                toOpacity = reverseAction.fromOpacity;
            else
                toOpacity = 0.0f;
        }
    }

    //仅作例子参考用
    public class ColorAttrCatcherGraphic : IColorAttrCatcher
    {
        public Func<Transform, Color> GetColor { get => GetColorFunc; }
        public System.Action<Transform, Color> SetColor { get => SetColorFunc; }
        public static ColorAttrCatcherGraphic Ins = new ColorAttrCatcherGraphic();

        public static Color GetColorFunc(Transform target)
        {
            if (target != null)
            {
                var graphic = target.GetComponent<Graphic>();
                if (graphic != null)
                    return graphic.color;
                Debug.LogError("action target has no Graphic component, please don't use ColorAttrCatcherGraphic!" + new System.Diagnostics.StackTrace().ToString());
            }
            return Color.white;
        }
        public static void SetColorFunc(Transform target, Color color)
        {
            if (target != null)
            {
                var graphic = target.GetComponent<Graphic>();
                if (graphic != null)
                    graphic.color = color;
                else
                    Debug.LogError("action target has no Graphic component, please don't use ColorAttrCatcherGraphic!" + new System.Diagnostics.StackTrace().ToString());
            }
        }
    }
    
}

