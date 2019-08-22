using UnityEngine;
using UnityEngine.EventSystems;
using UnityMMO;

public class FixedJoystick : Joystick
{
    public static FixedJoystick Instance;
    Vector2 joystickPosition = Vector2.zero;
    // private Camera cam = new Camera();
    public Camera UICamera;

    void Awake() {
        Instance = this;
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    void Start()
    {
        joystickPosition = RectTransformUtility.WorldToScreenPoint(UICamera, background.position);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        var goe = RoleMgr.GetInstance().GetMainRole();
        AutoFight fightAi = goe.GetComponent<UnityMMO.AutoFight>();
        if (fightAi != null) 
            fightAi.enabled = false;
        var moveQuery = goe.GetComponent<UnityMMO.MoveQuery>();
        if (moveQuery != null)
            moveQuery.StopFindWay();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
        GameInput.GetInstance().JoystickDir = inputVector;
        // if (Event.current != null)
        //     Event.current.Use();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        GameInput.GetInstance().JoystickDir = inputVector;
    }
}