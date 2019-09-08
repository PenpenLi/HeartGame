using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    GameObject chatWindow;
    string characterId;
    GameObject headMenu;
    GameObject changeName;
    GameObject changeSex;
    GameObject changeHeadImage;
    Transform headImageGrid = null;
    private void Awake()
    {

    }
    // Use this for initialization
    void Start () {
        //Load Head(Image,name,energy,exp)
        string headImagePath = "HeadImage/" + Globals.player.headImagePath;
        string name = Globals.player.nickname;
        int energy = Globals.player.energy;
        int maxEnergy = Globals.player.maxEnergy;
        int exp = Globals.player.exp;
        int maxExp = Globals.player.maxExp;
        characterId = Globals.player.characterId;
        //Find UIs
        //Head
        headMenu = GameFuncs.FindHiden("headMenuParent");
        changeName = GameFuncs.FindHiden("changeNameParent");
        changeSex = GameFuncs.FindHiden("changeSexParent");
        changeHeadImage = GameFuncs.FindHiden("changeHeadImageParent");
        Image headImage = UIManager.GetImage("headImage");
        Text energyText = UIManager.GetText("energyText");
        Text expText = UIManager.GetText("expText");
        Slider energySlider = UIManager.GetSlider("energySlider");
        Slider expSlider = UIManager.GetSlider("expSlider");
        Text nameText = UIManager.GetText("nameText");
        Button headImageBtn = UIManager.GetButton("headImageBtn");
            //Character
        Button character = UIManager.GetButton("character");
            //Items
        Text goldText = UIManager.GetText("goldText");
        Text diaText = UIManager.GetText("diaText");
            //Menus
        Button battleBtn = UIManager.GetButton("battleBtn");
        Button heroesBtn = UIManager.GetButton("heroesBtn");
        Button roomBtn = UIManager.GetButton("roomBtn");
        Button summonBtn = UIManager.GetButton("summonBtn");
        Button packBtn = UIManager.GetButton("packBtn");
        Button settingBtn = UIManager.GetButton("settingBtn");
            //ChatWindow
        chatWindow = GameFuncs.FindHiden("chatWindowParent");
        //UIs'value
        //Head
        UIManager.ChangeImage("headImage", headImagePath);
        energyText.text = energy.ToString() + "/" + maxEnergy;
        expText.text = exp.ToString() + "/" + maxExp;
        energySlider.value = (float)energy / maxEnergy;
        expSlider.value = (float)exp / maxExp;
        nameText.text = name;
        //Items
        goldText.text = Globals.player.gold.ToString();
        diaText.text = Globals.player.dia.ToString();
        //Load Hero(Image,Dialog)
        string characterPath = "Character/Image/"+characterId +"_001";
        UIManager.ChangeImage("character", characterPath);
        string dialogPath = characterId + "_click";
        ChatFuncs.LoadDialogs(dialogPath);
        //Bind Buttons
        headImageBtn.onClick.AddListener(OnheadImageBtnClick);
        battleBtn.onClick.AddListener(OnbattleBtnClick);
        heroesBtn.onClick.AddListener(OnheroesBtnClick);
        roomBtn.onClick.AddListener(OnroomBtnClick);
        summonBtn.onClick.AddListener(OnsummonBtnClick);
        packBtn.onClick.AddListener(OnpackBtnClick);
        settingBtn.onClick.AddListener(OnsettingBtnClick);

        character.onClick.AddListener(OncharacterBtnClick);
        //Play BGM
        MusicManager.SetVolume();
        MusicManager.PlayBgm("main");
    }
    void OnheadImageBtnClick()
    {
        if (!headMenu.activeInHierarchy)
        {
            headMenu.SetActive(true);
            UIManager.GetButton("changeNameBtn").onClick.AddListener(OnchangeNameBtnClick);
            UIManager.GetButton("changeSexBtn").onClick.AddListener(OnchangeSexBtnClick);
            UIManager.GetButton("changeHeadBtn").onClick.AddListener(OnchangeHeadBtnClick);
        }
        else
        {
            headMenu.SetActive(false);
            if (changeName.activeInHierarchy) changeName.SetActive(false);
            if (changeSex.activeInHierarchy) changeSex.SetActive(false);
            if (changeHeadImage.activeInHierarchy) changeHeadImage.SetActive(false);
        }
    }
    void OnchangeNameBtnClick()
    {
        if (!changeName.activeInHierarchy)
        {
            changeName.SetActive(true);
            InputField nameInput = GameObject.Find("nameInput").GetComponent<InputField>();
            UIManager.GetButton("okNameBtn").onClick.AddListener(()=>
            {
                Globals.player.nickname = nameInput.text;
                if (nameInput.text != "") UIManager.ChangeText("nameText", nameInput.text);
                nameInput.text = "";
                changeName.SetActive(false);
                if (changeSex.activeInHierarchy) changeSex.SetActive(false);
                if (changeHeadImage.activeInHierarchy) changeHeadImage.SetActive(false);
                if (headMenu.activeInHierarchy) headMenu.SetActive(false);
            });
        }
    }
    void OnchangeSexBtnClick()
    {
        if (!changeSex.activeInHierarchy)
        {
            changeSex.SetActive(true);
            Toggle[] sexToggles = changeSex.GetComponentsInChildren<Toggle>();
            for (int i = 0; i < sexToggles.Length; i++)
            {
                string sex = sexToggles[i].GetComponentInChildren<Text>().text.Trim();
                if (Globals.player.sex == sex)
                {
                    sexToggles[i].isOn = true;
                }
            }
            UIManager.GetButton("okSexBtn").onClick.AddListener(() =>
            {
                for (int i = 0; i < sexToggles.Length; i++)
                {
                    string sex = sexToggles[i].GetComponentInChildren<Text>().text.Trim();
                    if (sexToggles[i].isOn)
                    {
                        Globals.player.sex = sex;
                    }
                }
                changeSex.SetActive(false);
                if (changeName.activeInHierarchy) changeName.SetActive(false);
                if (changeHeadImage.activeInHierarchy) changeHeadImage.SetActive(false);
                if (headMenu.activeInHierarchy) headMenu.SetActive(false);
            });
        }
    }
    void OnchangeHeadBtnClick()
    {
        if (!changeHeadImage.activeInHierarchy)
        {
            changeHeadImage.SetActive(true);
            headImageGrid = changeHeadImage.transform.Find("headImages/headImageGrid");
            //create headImage
            string path="0";
            foreach (var item in Resources.LoadAll<Sprite>("HeadImage"))
            {
                GameObject headImage = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/HeadImage"));
                headImage.transform.SetParent(headImageGrid);
                headImage.transform.Find("Image").GetComponent<Image>().sprite = item;
                Toggle t = headImage.GetComponent<Toggle>();
                t.onValueChanged.AddListener((bool isOn)=>
                {
                    if (t.isOn) path = headImage.transform.Find("Image").GetComponent<Image>().sprite.name;
                });
                t.group = headImageGrid.GetComponent<ToggleGroup>();
                if (Globals.player.headImagePath == item.name) t.isOn = true;
            }
            UIManager.GetButton("okHeadImageBtn").onClick.AddListener(() =>
            {
                Globals.player.headImagePath = path;
                UIManager.ChangeImage("headImage", "HeadImage/"+path);
                foreach (Transform hi in headImageGrid)
                {
                    Destroy(hi.gameObject);
                }
                changeHeadImage.SetActive(false);
                if (changeName.activeInHierarchy) changeName.SetActive(false);
                if (changeSex.activeInHierarchy) changeSex.SetActive(false);
                if (headMenu.activeInHierarchy) headMenu.SetActive(false);
            });
        }
    }
    void OnbattleBtnClick()
    {
        MusicManager.PlaySe("click");
        GameFuncs.GoToSceneAsync("SelectBattleMap");
    }
    void OnheroesBtnClick()
    {
        MusicManager.PlaySe("click");
        GameFuncs.GoToSceneAsync("Heroes");
    }
    void OnroomBtnClick()
    {
        MusicManager.PlaySe("click");
        GameFuncs.GoToSceneAsync("Room");
    }
    void OnsummonBtnClick()
    {
        MusicManager.PlaySe("click");
        GameFuncs.GoToSceneAsync("Summon");
    }
    void OnpackBtnClick()
    {
        MusicManager.PlaySe("click");
        GameFuncs.GoToScene("Pack");
    }
    void OnsettingBtnClick()
    {
        MusicManager.PlaySe("click");
        GameFuncs.GoToScene("Setting");
    }
    void OncharacterBtnClick()
    {
        chatWindow.SetActive(true);
        OnSmallChatWindowClick.ShowSmallDialog(0);
        MusicManager.PlayVoice(characterId + "_click");
    }
}
