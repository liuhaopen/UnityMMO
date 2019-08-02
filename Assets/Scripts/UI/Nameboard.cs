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
    string showName;
    ColorStyle curColorStyle = ColorStyle.None;
    float maxHp;
    float curHp;
    private TextMeshProUGUI nameLabel;
    Image bloodImg;
    GameObject bloodObj;
    Slider slider;

    public string Name 
    { 
        get => showName; 
        set 
        {
            showName = value; 
            nameLabel.text = showName;
        }
    }
    public ColorStyle CurColorStyle 
    { 
        get => curColorStyle; 
        set
        {
            if (curColorStyle != value)
            {
                string resPath = ResPath.GetBloodResPath(value);
                // UIHelper.SetImage(bloodImg, resPath);
                resPath = UIHelper.FillUIResPath(resPath);
                ResourceManager.GetInstance().LoadAsset<Sprite>(resPath, delegate(UnityEngine.Object[] objs){
                    // Debug.Log("set image : "+resPath+" obj:"+(objs.Length)+" bloodImg:"+(bloodImg!=null));
                    if (bloodImg != null && objs != null && objs.Length>=1)
                    {
                        bloodImg.sprite = objs[0] as Sprite;
                        // if (is_auto_size)
                        //     bloodImg.SetNativeSize();
                    }
                });
            }
            curColorStyle = value; 
        }
    }

    public float MaxHp { 
        get => maxHp; 
        set
        {
            maxHp = value; 
            UpdateBloodBar();
        }
    }
    public float CurHp { 
        get => curHp; 
        set 
        {
            curHp = value;
            UpdateBloodBar();
        }
    }

    public void UpdateBloodBar()
    {
        // Debug.Log("curHp : "+curHp+" maxHp:"+maxHp+ " slider:"+slider!=null);
        if (slider!=null)
        {
            slider.value = Mathf.Clamp01(curHp/maxHp);
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
        slider = transform.Find("blood_con").GetComponent<Slider>();
        bloodObj = transform.Find("blood_con").gameObject;
    }

    private void Update() {
        transform.LookAt(Camera.main.transform.position);
    }

}

}