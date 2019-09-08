using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heroes : MonoBehaviour {
    GameObject heroPre;
    Transform panelGrid;
    void Start()
    {
        heroPre = (GameObject)GameFuncs.GetResource("Prefabs/hero");
        //Load eleToggles
        Toggle[] toggles = GameObject.FindObjectsOfType<Toggle>();
        foreach (Toggle t in toggles)
        {
            t.onValueChanged.AddListener((bool isOn) => { OneleToggleClick(t, isOn); });
        }
        //Items
        int upItemCount = 0;
        int breakItemCount = 0;
        for (int i = 0; i < Globals.items.Count; i++)
        {
            if (Globals.items[i].itemId == "001")//合成剂
            {
                upItemCount = Globals.items[i].count;
            }
            if (Globals.items[i].itemId == "003")//突破晶石
            {
                breakItemCount = Globals.items[i].count;
            }
        }
        UIManager.ChangeText("upItemText", upItemCount.ToString());
        UIManager.ChangeText("breakItemText", breakItemCount.ToString());
        UIManager.ChangeText("goldText", Globals.player.gold.ToString());
        HeroGlobals.upItemCount = upItemCount;
        HeroGlobals.breakItemCount = breakItemCount;
        //Show All Heroes
        foreach (Toggle t in toggles)
        {
            string eleName = t.gameObject.name.Replace("Toggle", "");
            string panelParentName = eleName + "PanelParent";
            GameObject panel = GameFuncs.FindHiden(panelParentName);
            Transform grid = panel.transform.Find(eleName+"Grid");
            ShowHeroes(eleName,grid);
        }
    }

    void OneleToggleClick(Toggle toggle,bool isOn)
    {
        MusicManager.PlaySe("click");
        string eleName = toggle.gameObject.name.Replace("Toggle", "");
        string panelParentName =  eleName+ "PanelParent";
        GameObject panel = GameFuncs.FindHiden(panelParentName);
        panel.SetActive(isOn);
    }
    void ShowHeroes(string ele,Transform grid)
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
        heroObj.transform.Find("Text").GetComponent<Text>().text=hero.ename;
        heroObj.GetComponent<Image>().sprite=Resources.Load<Sprite>("Character/Portrait/"+hero.imagePath);
        heroObj.transform.Find("ele/Image").GetComponent<Image>().sprite
            = Resources.Load<Sprite>("Icons/" + hero.ele);
        heroObj.GetComponent<Button>().onClick.AddListener(()=> { OnheroBtnClick(hero.id); });
    }
    void OnheroBtnClick(string id)
    {
        MusicManager.PlaySe("click");
        HeroGlobals.currentid=id;
        GameFuncs.GoToScene("HeroInfo");
    }
}
