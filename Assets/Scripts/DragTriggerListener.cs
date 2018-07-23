using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace LuaFramework {
	public class DragTriggerListener : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
		public delegate void VoidDelegate (GameObject go,float x,float y);
        public VoidDelegate onDrag;
        public VoidDelegate onDragBegin;
        public VoidDelegate onDragEnd;
        static public DragTriggerListener Get (GameObject go)
		{
            DragTriggerListener listener = go.GetComponent<DragTriggerListener>();
			if (listener == null) listener = go.AddComponent<DragTriggerListener>();
			return listener;
		}

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (onDragBegin != null)
            {
                onDragBegin(gameObject, eventData.position.x, eventData.position.y);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
            {
                onDrag(gameObject, eventData.position.x, eventData.position.y);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (onDragEnd != null)
            {
                onDragEnd(gameObject, eventData.position.x, eventData.position.y);
            }
        }
    }
}