using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    Slider loadingSlider;
    Text loadingText;
    int load;//now loading value
    AsyncOperation ao;
	// Use this for initialization
	void Start () {
        loadingSlider = UIManager.GetSlider("loadingSlider");
        loadingText = UIManager.GetText("loadingText");
        StartCoroutine(AsyncLoading());
        //load bg
        UIManager.ChangeImage("BG", "Background/" + Resources.LoadAll<Sprite>("Background")
            [Random.Range(0, Resources.LoadAll<Sprite>("Background").Length - 1)].name);
    }
    IEnumerator AsyncLoading()
    {
        ao = SceneManager.LoadSceneAsync(Globals.nextScene);
        ao.allowSceneActivation = false;
        yield return ao;
    }
	// Update is called once per frame
	void Update () {
        if (load<100)
        {
            load++;
        }
        loadingSlider.value = (float)load / 100;
        loadingText.text = load + "%";
        if (load >= 100)
        {
            ao.allowSceneActivation = true;
        }
	}
}
