using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHero : MonoBehaviour
{

    GameObject heroPre;
    Transform panelGrid;
    Toggle currentToggle;
    Toggle[] selectToggles;
    List<Transform> grids=new List<Transform>();
    // Use this for initialization
    void Start()
    {
        heroPre = (GameObject)GameFuncs.GetResource("Prefabs/hero");
        Transform selectGrid = GameObject.Find("selectGrid").transform;
        Transform heroes = GameObject.Find("heroes").transform;
        //Load selectToggles
        selectToggles = selectGrid.GetComponentsInChildren<Toggle>(false);
        foreach (Toggle t in selectToggles)
        {
            t.onValueChanged.AddListener((bool isOn) => { OnselectToggleClick(t, isOn); });
        }
        currentToggle = selectToggles[0];
        //Load eleToggles
        Toggle[] eleToggles = heroes.GetComponentsInChildren<Toggle>(false);
        foreach (Toggle t in eleToggles)
        {
            t.onValueChanged.AddListener((bool isOn) => { OneleToggleClick(t, isOn); });
        }
        //Show All Heroes
        foreach (Toggle t in eleToggles)
        {
            string eleName = t.gameObject.name.Replace("Toggle", "");
            string panelParentName = eleName + "PanelParent";
            GameObject panel = GameFuncs.FindHiden(panelParentName);
            Transform grid = panel.transform.Find(eleName + "Grid");
            ShowHeroes(eleName, grid);
            grids.Add(grid);
        }
        UIManager.ChangeText("energyText", Globals.player.energy.ToString() + "/" + Globals.player.maxEnergy);
        UIManager.ChangeSlider("energySlider", (float)Globals.player.energy / Globals.player.maxEnergy);
        Button goBtn = UIManager.GetButton("goBtn");
        goBtn.onClick.AddListener(OngoBtnClick);
        Button backBtn = UIManager.GetButton("backBtn");
        backBtn.onClick.AddListener(()=> {

            GameFuncs.GoToScene("SelectBattleMap");
        });
    }
    void OneleToggleClick(Toggle toggle, bool isOn)
    {
        //MusicManager.PlaySe("click");
        string eleName = toggle.gameObject.name.Replace("Toggle", "");
        string panelParentName = eleName + "PanelParent";
        GameObject panel = GameFuncs.FindHiden(panelParentName);
        panel.SetActive(isOn);
    }
    void OnselectToggleClick(Toggle toggle, bool isOn)
    {
        currentToggle = toggle;
        if (toggle.isOn && IsToggleHasHero(toggle))//换下来
        {
            string id = currentToggle.transform.Find("Image").GetComponent<Image>().sprite.name;
            if (GetHero(id).Count>0)
            {
                foreach (var h in GetHero(id))
                {
                    h.GetComponent<Button>().interactable = true;
                }
            }
            currentToggle.transform.Find("Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>("Character/Image/000_000");
            if (AllTogglesEmpty())
            {
                currentToggle = selectToggles[0];
                selectToggles[0].isOn = true;
            }
        }
    }
    void ShowHeroes(string ele, Transform grid)
    {
        panelGrid = grid;
        if (ele != "all")//对应元素
        {
            for (int i = 0; i < Globals.heroes.Count; i++)
            {
                Hero hero = Globals.heroes[i];
                if (hero.ele == ele)
                {
                    CreateHero(hero);
                }
            }
        }
        else
        {
            for (int i = 0; i < Globals.heroes.Count; i++)
            {
                CreateHero(Globals.heroes[i]);
            }
        }
    }
    void CreateHero(Hero hero)
    {
        GameObject heroObj = Instantiate(heroPre);
        heroObj.transform.SetParent(panelGrid);
        heroObj.transform.Find("Text").GetComponent<Text>().text = hero.ename;
        heroObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Character/Portrait/" + hero.imagePath);
        heroObj.transform.Find("ele/Image").GetComponent<Image>().sprite
            = Resources.Load<Sprite>("Icons/" + hero.ele);
        heroObj.GetComponent<Button>().onClick.AddListener(() => { OnheroBtnClick(hero.id, heroObj); });
    }
    void OnheroBtnClick(string id, GameObject obj)
    {
        if (IsToggleHasHero(currentToggle))//空时可上,否则更换
        {
            string lastId = currentToggle.transform.Find("Image").GetComponent<Image>().sprite.name;
            if (GetHero(lastId).Count>0)
            {
                foreach (var h in GetHero(lastId))
                {
                   h.GetComponent<Button>().interactable = true;
                }
            }
        }
        if (GetHero(id).Count>0)
        {
            currentToggle.transform.Find("Image").GetComponent<Image>().sprite =
Resources.Load<Sprite>("Character/Portrait/" + id);
            foreach (var h in GetHero(id))
            {
                h.GetComponent<Button>().interactable = false;
            }
            MusicManager.PlayVoice(id+"_select");
        }
        //在后面挑选一个空的
        foreach (Toggle t in selectToggles)
        {
            if (t != currentToggle)
            {
                if (!IsToggleHasHero(t))
                {
                    currentToggle = t;
                    t.isOn = true;
                    break;
                }

            }
        }
    }
    bool IsToggleHasHero(Toggle t)
    {
        if (t.transform.Find("Image").GetComponent<Image>().sprite.name != "000_000")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    List<GameObject> GetHero(string id)
    {
        List<GameObject> heroes = new List<GameObject>();
        foreach (Transform g in grids)
        {
            foreach (Transform t in g)
            {
                if (t.GetComponent<Image>().sprite.name == id)
                {
                    heroes.Add(t.gameObject);
                }
            }
        }
        return heroes;
    }
    void OngoBtnClick()
    {
        bool canGo = false;
        foreach (Toggle t in selectToggles)
        {
            if (IsToggleHasHero(t))
            {
                canGo = true;
                break;
            }
        }
        if (canGo)
        {
            BattleGlobals.selectHeroes=new List<string>();
            foreach (Toggle t in selectToggles)
            {
                if (IsToggleHasHero(t))
                {
                    BattleGlobals.selectHeroes.Add(t.transform.Find("Image").GetComponent<Image>().sprite.name);
                }
            }
            if (Globals.player.energy >= int.Parse(BattleGlobals.placeName) * 10)
            {
                Globals.player.energy -= int.Parse(BattleGlobals.placeName) * 10;
                UIManager.ChangeText("energyText", Globals.player.energy.ToString() + "/" + Globals.player.maxEnergy);
                UIManager.ChangeSlider("energySlider", (float)Globals.player.energy / Globals.player.maxEnergy);
                StartCoroutine(Wait(1));
            }
            else
            {
                GameFuncs.CreateMsg("体力不足!");
            }
        }
        else//弹出对话框“所选英雄不能为空”
        {
            GameFuncs.CreateMsg("所选英雄不能为空!");
        }
    }
    bool AllTogglesEmpty()
    {
        bool isEmpty = true;
        foreach (Toggle t in selectToggles)
        {
            if (IsToggleHasHero(t))
            {
                isEmpty = false;
                break;
            }
        }
        return isEmpty;
    }
    IEnumerator Wait(int time)
    {
        yield return new WaitForSeconds(time);
        GameFuncs.GoToSceneAsync("Battle");
    }
}
