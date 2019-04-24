using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cocos
{
    //
    // FadeTo
    //
    public class FadeTo : ActionInterval
    {
        protected Material material;
        protected Graphic graphic;
        protected float toOpacity;
        public float fromOpacity;
        public static FadeTo Create(float d, float opacity)
        {
            FadeTo action = new FadeTo();
            if (action != null && action.InitWithDuration(d, opacity))
                return action;
            return null;
        }
        
        public bool InitWithDuration(float duration, float opacity)
        {
            if (base.InitWithDuration(duration))
            {
                toOpacity = opacity;
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
        }

        public override void Update(float time)
        {
            Color color;
            if (material != null)
            {
                color = material.color;
                color.a = (fromOpacity + (toOpacity - fromOpacity)*time)/255.0f;
                Debug.Log("color.a : "+color.a);
                material.color = color;
            }
            else if (graphic != null)
            {
                color = graphic.color;
                color.a = (fromOpacity + (toOpacity - fromOpacity)*time)/255.0f;
                graphic.color = color;
            }
        }
    }

    //
    // FadeIn
    //
    public class FadeIn : FadeTo
    {
        FadeTo reverseAction;
        public static FadeIn Create(float d)
        {
            FadeIn action = new FadeIn();
            if (action != null && action.InitWithDuration(d, 255.0f))
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
            if (material != null)
                fromOpacity = material.color.a * 255.0f;
            else if (graphic != null)
                fromOpacity = graphic.color.a * 255.0f;
        }
    }
    
    //
    // FadeOut
    //
    public class FadeOut : FadeTo
    {
        FadeTo reverseAction;
        public static FadeOut Create(float d)
        {
            FadeOut action = new FadeOut();
            if (action != null && action.InitWithDuration(d, 0.0f))
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
            if (material != null)
                fromOpacity = material.color.a * 255.0f;
            else if (graphic != null)
                fromOpacity = graphic.color.a * 255.0f;
        }
    }
    
}

