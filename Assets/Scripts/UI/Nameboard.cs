using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLuaFramework;

namespace UnityMMO{
public class Nameboard : MonoBehaviour 
{
    public enum ColorStyle
    {
        Green,
        Red,
        Blue,
        None,
    }
    public Transform target;
    string name;
    ColorStyle curColorStyle = ColorStyle.None;

    private TextMeshProUGUI nameLabel;
    Image bloodImg;
    GameObject bloodObj;

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
        set
        {
            if (curColorStyle != value)
            {
                string resPath = GameConst.GetBloodResPath(value);
                UIHelper.SetImage(bloodImg, resPath);
            }
            curColorStyle = value; 
        }
    }

    public void SetBloodVisible(bool isShow)
    {
        bloodObj.SetActive(isShow);
    }

    private void Awake() 
    {
        nameLabel = transform.Find("name_con/name_label").GetComponent<TextMeshProUGUI>();
        bloodImg = transform.Find("blood_con/blood_bar/blood").GetComponent<Image>();
        bloodObj = transform.Find("blood_con").gameObject;
    }

}

}