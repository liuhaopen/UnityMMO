using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

namespace LuaFramework
{
    public class UIHelper
    {
        public static void SetPosition(Transform transform, float x, float y, float z)
        {
            if (transform != null)
            {
                transform.position = new Vector3(x, y, z);
            }
        }

        public static void SetPositionX(Transform transform, float x)
        {
            if (transform != null)
            {
                transform.position = new Vector3(x, transform.position.y, transform.position.z);
            }
        }

        public static void SetPositionY(Transform transform, float y)
        {
            if (transform != null)
            {
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
            }
        }

        public static void SetPositionZ(Transform transform, float z)
        {
            if (transform != null)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, z);
            }
        }

        public static float GetPositionX(Transform transform)
        {
            if (transform != null)
            {
                return transform.position.x;
            }
            return 0;
        }

        public static float GetPositionY(Transform transform)
        {
            if (transform != null)
            {
                return transform.position.y;
            }
            return 0;
        }


        public static float GetPositionZ(Transform transform)
        {
            if (transform != null)
            {
                return transform.position.z;
            }
            return 0;
        }


        public static void SetLocalPosition(Transform transform, float x, float y, float z)
        {
            if (transform != null)
            {
                transform.localPosition = new Vector3(x, y, z);
            }
        }

        public static void SetLocalPositionX(Transform transform, float x)
        {
            if (transform != null)
            {
                transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            }
        }

        public static void SetLocalPositionY(Transform transform, float y)
        {
            if (transform != null)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            }
        }

        public static void SetLocalPositionZ(Transform transform, float z)
        {
            if (transform != null)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
            }
        }

        public static float GetLocalPositionX(Transform transform)
        {
            if (transform != null)
            {
                return transform.localPosition.x;
            }
            return 0;
        }


        public static float GetLocalPositionY(Transform transform)
        {
            if (transform != null)
            {
                return transform.localPosition.y;
            }
            return 0;
        }

        public static float GetLocalPositionZ(Transform transform)
        {
            if (transform != null)
            {
                return transform.localPosition.z;
            }
            return 0;
        }
        public static void SetLocalScale(Transform transform, float x, float y, float z)
        {
            if (transform != null)
            {
                transform.localScale = new Vector3(x, y, z);
            }
        }

        public static void SetLocalRotation(Transform transform, float x, float y, float z, float w)
        {
            if (transform != null)
            {
                transform.localRotation = new Quaternion(x, y, z, w);
            }
        }

        public static void SetRotate(Transform transform, float x, float y, float z)
        {
            if (transform != null)
            {
                transform.Rotate(new Vector3(x, y, z));
            }
        }


        public static float GetRenderBoundsSize(SkinnedMeshRenderer render)
        {
            if (render != null)
            {
                return render.bounds.size.y;
            }
            return 0;
        }


        public static void SetSizeDelta(RectTransform transform, float x, float y)
        {
            if (transform != null)
            {
                transform.sizeDelta = new Vector2(x, y);
            }
        }

        public static void SetSizeDeltaX(RectTransform transform, float x)
        {
            if (transform != null)
            {
                transform.sizeDelta = new Vector2(x, transform.sizeDelta.y);
            }
        }


        public static void SetSizeDeltaY(RectTransform transform, float y)
        {
            if (transform != null)
            {
                transform.sizeDelta = new Vector2(transform.sizeDelta.x, y);
            }
        }

        public static float GetSizeDeltaX(RectTransform transform)
        {
            if (transform != null)
            {
                return transform.sizeDelta.x;
            }
            return 0;
        }

        public static float GetSizeDeltaY(RectTransform transform)
        {
            if (transform != null)
            {
                return transform.sizeDelta.y;
            }
            return 0;
        }

        public static void BindClickEvent(GameObject obj, LuaFunction luafunc)
        {
            if (obj == null || luafunc == null) return;
            ClickTriggerListener listener = ClickTriggerListener.Get(obj);
            if (listener != null)
                listener.onClick = luafunc;
        }

        //public static void BindClickEvent(Button btn, LuaFunction luafunc)
        //{
        //    if (btn == null || luafunc == null) return;
        //    btn.onClick.AddListener(
        //        delegate () {
        //            luafunc.Call(btn);
        //        }
        //    );
        //}
    }
}