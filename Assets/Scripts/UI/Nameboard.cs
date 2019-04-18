using TMPro;
using UnityEngine;

namespace UnityMMO{
public class Nameboard : MonoBehaviour 
{
    public enum ColorStyle
    {
        Green,Red,Blue
    }
    public Transform target;
    string name;
    ColorStyle curColorStyle;

    private TextMeshProUGUI nameLabel;

    public string Name 
    { 
        get => name; 
        set 
        {
            name = value; 
            nameLabel.text = name;
        }
    }
    public ColorStyle CurColorStyle 
    { 
        get => curColorStyle; 
        set => curColorStyle = value; 
    }

    private void Awake() 
    {
        nameLabel = transform.Find("name_con/name_label").GetComponent<TextMeshProUGUI>();
    }

    // private void Update() {
    //     if (target==null)
    //         return;
    //     Vector2 player2DPosition = Camera.main.WorldToScreenPoint(target.position);
    //     Vector3 BloodSlotWorldPos = target.position + new Vector3 (0f, 2.8f, 0f);
    //     Vector3 BloodSlotToCamera = Camera.main.transform.position - BloodSlotWorldPos;
    //     float BloodSlotDIs = BloodSlotToCamera.magnitude;
    //     float maxVisualDis = 200;
    //     float scaleFactor = Mathf.Clamp(1-(BloodSlotDIs-maxVisualDis)/maxVisualDis, 0, 1);
    //     Debug.Log("scaleFactor : "+scaleFactor+" dis:"+BloodSlotDIs);
    //     transform.position = Camera.main.WorldToScreenPoint(BloodSlotWorldPos);
    //     transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        // Debug.Log("player2DPosition.x:"+player2DPosition.x+" y:"+player2DPosition.y+" scew:"+Screen.width+" he:"+Screen.height);
        // if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
        // {
        //     gameObject.SetActive(false);
        // }
        // else
        // {
        //     gameObject.SetActive(true);
        // }
    // }
}

}