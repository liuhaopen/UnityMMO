using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using XLua;

namespace XLuaFramework {
	public class ClickTriggerListener : MonoBehaviour, IPointerClickHandler{
		public delegate void VoidDelegate (GameObject go,float x,float y);
    	
		public LuaFunction onClick;
		static public ClickTriggerListener Get (GameObject go)
		{
			ClickTriggerListener listener = go.GetComponent<ClickTriggerListener>();
			if (listener == null) listener = go.AddComponent<ClickTriggerListener>();
			return listener;
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			if (onClick != null) {
				onClick.Call(gameObject,eventData.position.x,eventData.position.y);
			}
		}
	}
}