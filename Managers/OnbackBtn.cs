using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnbackBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameObject.Find("backBtn") !=null)
        {
            Button backBtn = UIManager.GetButton("backBtn");
            backBtn.onClick.AddListener(OnbackBtnClick);
        }
    }

    void OnbackBtnClick()
    {
        GameFuncs.GoToSceneAsync("Main");
    }
}
