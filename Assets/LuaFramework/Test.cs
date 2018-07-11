using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Button btn = GameObject.Find("login").GetComponent<Button>();
        //btn.onClick.AddListener(delegate () { OnClick(); });

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        Debug.Log("on click");
    }
}
