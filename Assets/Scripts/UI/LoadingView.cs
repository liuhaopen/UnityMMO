using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityMMO
{
    
public class LoadingView : MonoBehaviour
{
    public static LoadingView Instance;
    RectTransform proBar;
    RectTransform butterfly;
    TextMeshProUGUI tip;
    float curPercent;
    float nextPercent;
    float playSpeed = 0.4f;
    const float maxProWidth = 1062;
    
    void Start()
    {
        Instance = this;
        proBar = transform.Find("pro_bar") as RectTransform;
        butterfly = transform.Find("butterfly") as RectTransform;
        tip = transform.Find("tip").GetComponent<TextMeshProUGUI>();
        ResetData();
    }

    public void ResetData()
    {
        curPercent = 0;
        nextPercent = 0;
        playSpeed = 0;
        proBar.sizeDelta = new Vector2(0, proBar.sizeDelta.y);
        var butterPos = butterfly.localPosition;
        butterfly.localPosition = new Vector3(-551, butterPos.y, butterPos.z);
    }

    public void SetPlaySpeed(float speed)
    {
        playSpeed = speed;
    }

    public void SetActive(bool isShow, float delayTime=0.0f)
    {
        CancelInvoke("Show");
        CancelInvoke("Hide");
        if (delayTime <= 0)
        {
            gameObject.SetActive(isShow);
            if (isShow)
                transform.SetAsLastSibling();
        }
        else
        {
            if (isShow)
                Invoke("Show", delayTime);
            else
                Invoke("Hide", delayTime);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetData(float percent, string tipStr)
    {
        nextPercent = Mathf.Clamp01(percent);
        playSpeed = Mathf.Clamp(nextPercent-curPercent, 0.3f, 1);
        // Debug.Log("loading percent : "+percent+" tips:"+tipStr+" playSpeed:"+playSpeed+" "+curPercent+" track:"+ new System.Diagnostics.StackTrace().ToString());
        tip.text = tipStr;
    }

    void Update()
    {
        if (curPercent == nextPercent)
            return;
        float newPercent = curPercent + playSpeed*Time.deltaTime;
        // Debug.Log("newPercent : "+newPercent+" cur:"+curPercent+" speed:"+playSpeed+" clamp:"+Mathf.Clamp(newPercent, 0, nextPercent));
        newPercent = Mathf.Clamp(newPercent, 0, nextPercent);
        proBar.sizeDelta = new Vector2(maxProWidth*newPercent, proBar.sizeDelta.y);
        var butterPos = butterfly.localPosition;
        butterfly.localPosition = new Vector3(-551+maxProWidth*newPercent, butterPos.y, butterPos.z);
        curPercent = newPercent;
    }
}

}
