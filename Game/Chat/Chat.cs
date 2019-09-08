using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    GameObject options;

    GameObject log;
    Button closeLogBtn;

    GameObject set;
    Slider bgmSlider;
    Slider seSlider;
    Button closeSetBtn;
    private void Awake()
    {
        ChatFuncs.LoadDialogs(Globals.currentDialog);
    }
    // Use this for initialization
    void Start()
    {
        options = GameObject.Find("options");
        log = GameFuncs.FindHiden("logParent");
        set = GameFuncs.FindHiden("setParent");
        //绑定Buttons
        Button setBtn = UIManager.GetButton("setBtn");
        setBtn.onClick.AddListener(OnsetBtnClick);
        Button skipBtn = UIManager.GetButton("skipBtn");
        skipBtn.onClick.AddListener(OnskipBtnClick);
        Button logBtn = UIManager.GetButton("logBtn");
        logBtn.onClick.AddListener(OnlogBtnClick);
        //更换背景
        UIManager.ChangeImage("BG", "Background/" + Globals.currentBg);
        //显示首句
        ChatFuncs.Instance.ShowDialog(0);
        //PlayBGM
        MusicManager.PlayBgm("normal");
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnBgmSliderChange()
    {
        if (bgmSlider != null)
        {
            if (bgmSlider.gameObject.activeInHierarchy)
            {
                Globals.bgmVolume = bgmSlider.value;
            }
        }
        MusicManager.SetVolume();
    }
    void OnSeSliderChange()
    {
        if (seSlider != null)
        {
            if (seSlider.gameObject.activeInHierarchy)
            {
                Globals.seVolume = seSlider.value;
            }
        }
        MusicManager.SetVolume();
    }
    void OnsetBtnClick()
    {
        //PlaySE
        MusicManager.PlaySe("click");
        //UI:BGM SE(Global)
        set.SetActive(true);

        bgmSlider = set.transform.Find("bgmSlider").GetComponent<Slider>();
        seSlider = set.transform.Find("seSlider").GetComponent<Slider>();
        //绑定Sliders
        bgmSlider.onValueChanged.AddListener(delegate { OnBgmSliderChange(); });
        seSlider.onValueChanged.AddListener(delegate { OnSeSliderChange(); });

        closeSetBtn = set.transform.Find("closeSetBtn").GetComponent<Button>();
        closeSetBtn.onClick.AddListener(OncloseSetBtnClick);
    }
    void OncloseSetBtnClick()
    {
        //PlaySE
        MusicManager.PlaySe("click");
        if (closeSetBtn != null && set.activeInHierarchy)
        {
            set.SetActive(false);
        }
    }
    void OnskipBtnClick()
    {
        //PlaySE
        MusicManager.PlaySe("click");
        //Go to next scene(stop at options)
        if (Globals.lastScene == "Login") Globals.nextScene = "Main";
        if (Globals.lastScene == "Room") GameFuncs.GoToScene("Room"); 
        else GameFuncs.GoToSceneAsync(Globals.nextScene);
    }
    void OnlogBtnClick()
    {
        //PlaySE
        MusicManager.PlaySe("click");
        //UI:name:context
        log.SetActive(true);
        Text logText = UIManager.GetText("logText");
        logText.text = "";
        for (int i = 0; i <= Globals.logIndex; i++)
        {
            logText.text += Globals.dialogs[i].dname + "\n" + "  "+Globals.dialogs[i].context + "\n";
        }
        closeLogBtn = log.transform.Find("closeLogBtn").GetComponent<Button>();
        closeLogBtn.onClick.AddListener(OncloseLogBtnClick);
    }
    void OncloseLogBtnClick()
    {
        //PlaySE
        MusicManager.PlaySe("click");
        if (closeLogBtn != null && log.activeInHierarchy)
        {
            log.SetActive(false);
        }
    }
    public void CreateOptions(int count)
    {
        GameObject optionPre = GameFuncs.GetResource("Prefabs/option") as GameObject;
        for (int i = 0; i < count; i++)
        {
            GameObject optionObj = GameObject.Instantiate(optionPre);
            optionObj.transform.SetParent(options.transform);
        }
    }
}
