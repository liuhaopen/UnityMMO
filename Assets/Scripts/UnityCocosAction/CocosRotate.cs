using System;
using UnityEngine;

namespace Cocos
{
    public class RotateBy : ActionInterval
    {
        protected Vector3 rotationDelta;
        protected Vector3 previousRotation;
        protected Vector3 startRotation;
        protected CoordinateStyle style;
        protected RectTransform rectTrans;
        
        public static RotateBy Create(float duration, Vector3 deltaRotation, CoordinateStyle style=CoordinateStyle.Local)
        {
            RotateBy ret = new RotateBy();
            if (ret != null && ret.InitWithStyle(duration, deltaRotation, style))
                return ret;
            return null;
        }

        public static RotateBy CreateLocal(float duration, Vector3 deltaRotation)
        {
            return Create(duration, deltaRotation, CoordinateStyle.Local);
        }

        public static RotateBy CreateAbs(float duration, Vector3 deltaRotation)
        {
            return Create(duration, deltaRotation, CoordinateStyle.Abs);
        }

        private bool InitWithStyle(float duration, Vector3 deltaRotation, CoordinateStyle style)
        {
            bool ret = false;
            if (base.InitWithDuration(duration))
            {
                this.rotationDelta = deltaRotation;
                this.style = style;
                ret = true;
            }
            return ret;
        }

        public override Action Clone()
        {
            Debug.Log("Rotate by clone");
            return RotateBy.Create(duration, rotationDelta, style);
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            switch (style)
            {
                case CoordinateStyle.Local:
                {
                    previousRotation = startRotation = target.localEulerAngles;
                    break;
                }
                case CoordinateStyle.Abs:
                {
                    previousRotation = startRotation = target.eulerAngles;
                    break;
                }
                default:
                    Debug.LogError("cannot support coordinate style for rotate action : "+style);
                    break;
            }
        }

        public override Action Reverse()
        {
            return RotateBy.Create(duration, -rotationDelta, style);
        }

        public override void Update(float t) 
        {
            switch (style)
            {
                case CoordinateStyle.Local:
                {
                    var currentPos = target.localEulerAngles;
                    var diff = currentPos - previousRotation;
                    startRotation = startRotation + diff;
                    var newPos = startRotation + (rotationDelta*t);
                    target.localEulerAngles = newPos;
                    previousRotation = newPos;
                    break;
                }
                case CoordinateStyle.Abs:
                {
                    var currentPos = target.eulerAngles;
                    var diff = currentPos - previousRotation;
                    startRotation = startRotation + diff;
                    var newPos = startRotation + (rotationDelta*t);
                    target.eulerAngles = newPos;
                    previousRotation = newPos;
                    break;
                }
                default:
                    break;
            }
        }
    }

    //
    // RotateTo
    //
    public class RotateTo : RotateBy
    {
        Vector3 endRotation;
        public static new RotateTo Create(float duration, Vector3 rotation, CoordinateStyle style=CoordinateStyle.Local)
        {
            RotateTo ret = new RotateTo();
            if (ret != null && ret.InitWithDuration(duration, rotation, style))
                return ret;
            return null;
        }

        public static new RotateTo CreateLocal(float duration, Vector3 rotation)
        {
            return Create(duration, rotation, CoordinateStyle.Local);
        }
        
        public static new RotateTo CreateAbs(float duration, Vector3 rotation)
        {
            return Create(duration, rotation, CoordinateStyle.Abs);
        }

        private bool InitWithDuration(float duration, Vector3 rotation, CoordinateStyle style)
        {
            bool ret = false;
            if (base.InitWithDuration(duration))
            {
                this.style = style;
                this.endRotation = rotation;
                ret = true;
            }
            return ret;
        }

        public override Action Clone()
        {
            return RotateTo.Create(duration, endRotation);
        }

        public override void StartWithTarget(Transform target)
        {
            base.StartWithTarget(target);
            switch (style)
            {
                case CoordinateStyle.Local:
                {
                    rotationDelta = endRotation - target.localEulerAngles;
                    break;
                }
                case CoordinateStyle.Abs:
                {
                    rotationDelta = endRotation - target.eulerAngles;
                    break;
                }
                default:
                    Debug.LogError("cannot support coordinate style for rotate action : "+style);
                    break;
            }
        }

        public override Action Reverse()
        {
            Debug.LogError("reverse() not supported in RotateTo");
            return null;
        }
    }


}