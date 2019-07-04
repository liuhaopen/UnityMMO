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
        Debug.Log("loading view start");
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
        proBar.sizeDelta = new Vector2(0, proBar.sizeDelta.y);
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
        playSpeed = Mathf.Min(0.4f, nextPercent-curPercent);
        tip.text = tipStr;
    }

    void Update()
    {
        if (curPercent == nextPercent)
            return;
        float newPercent = curPercent + playSpeed*Time.deltaTime;
        newPercent = Mathf.Min(newPercent, nextPercent);
        newPercent = Mathf.Clamp01(newPercent);
        proBar.sizeDelta = new Vector2(maxProWidth*newPercent, proBar.sizeDelta.y);
        var butterPos = butterfly.localPosition;
        butterfly.localPosition = new Vector3(-551+maxProWidth*newPercent, butterPos.y, butterPos.z);
        curPercent = newPercent;
    }
}

}
