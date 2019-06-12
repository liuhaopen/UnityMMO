using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace XLuaFramework {
public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go,float x,float y);
    public LuaFunction onClick;
    public LuaFunction onDown;
    public LuaFunction onEnter;
    public LuaFunction onExit;
    public LuaFunction onUp;
    public LuaFunction onSelect;
    public LuaFunction onUpdateSelect;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick.Call(gameObject,eventData.position.x,eventData.position.y);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown.Call(gameObject,eventData.position.x,eventData.position.y);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter.Call(gameObject,eventData.position.x,eventData.position.y);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit.Call(gameObject,eventData.position.x,eventData.position.y);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp.Call(gameObject,eventData.position.x,eventData.position.y);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect.Call(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect.Call(gameObject);
    }
}
}