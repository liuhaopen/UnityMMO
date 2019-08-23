using Cinemachine;
using UnityEngine;

namespace UnityMMO
{    
[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraCtrl : MonoBehaviour {
    private CinemachineFreeLook freeLookCam;
    public Vector2 additionMove;
    public Vector2 speed = new Vector2(10, 5);
    // Use this for initialization
    void Start () {
        freeLookCam = GetComponent<CinemachineFreeLook>();
        additionMove = Vector3.zero;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
        // freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
#endif
    }

    public void ApplyMove(float x, float y)
    {
        // Debug.Log("x y : "+x+" "+y);
        additionMove /= 1.2f;
        additionMove.x += x;
        additionMove.y += y;
    }

    // Update is called once per frame
    void Update () {
// #if !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
        if (additionMove.Equals(Vector2.zero))
            return;
        var newAdditionX = speed.x*Time.deltaTime*additionMove.x;
        if (Mathf.Abs(newAdditionX) > Mathf.Abs(additionMove.x))
            newAdditionX = additionMove.x;
        freeLookCam.m_XAxis.Value += newAdditionX;
        additionMove.x -= newAdditionX;

        var newAdditionY = speed.y*Time.deltaTime*additionMove.y;
        if (Mathf.Abs(newAdditionY) > Mathf.Abs(additionMove.y))
            newAdditionY = additionMove.y;
        freeLookCam.m_YAxis.Value += newAdditionY;
        additionMove.y -= newAdditionY;
// #endif
        // Debug.Log("freeLookCam.m_XAxis.Value : "+freeLookCam.m_XAxis.Value+" "+freeLookCam.m_YAxis.Value);
    }
}
}