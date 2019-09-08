using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfo : MonoBehaviour
{
    Hero currentHero;
    int dressIndex=0;
    Transform panel;
    Transform skillGrid;
    Text infoText;
    // Use this for initialization
    void Start()
    {
        //Load Hero
        string id = HeroGlobals.currentid;
        for (int i = 0; i < Globals.heroes.Count; i++)
        {
            if (Globals.heroes[i].id == id)
            {
                currentHero = Globals.heroes[i];
            }
        }
        Transform point = GameObject.Find("point").transform;
        GameObject hero = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/" + id), point);
        hero.AddComponent<OnHeroInfoModel>();
        hero.GetComponent<Rigidbody>().useGravity = false;
        //Load Toggle
        Toggle[] toggles = GameObject.FindObjectsOfType<Toggle>();
        foreach (Toggle t in toggles)
        {
            t.onValueChanged.AddListener((bool isOn) => { OninfoToggleClick(t, isOn); });
        }
        foreach (Toggle t in toggles)
        {
            string infoName = t.gameObject.name.Replace("Toggle", "");
            string panelParentName = infoName + "PanelParent";
            panel = GameFuncs.FindHiden(panelParentName).transform;
            //Load All
            if (infoName == "all")
            {
                ShowAll(panel);
                //Bind Btns
                Button upBtn = UIManager.GetButton("upBtn");
                Button breakBtn = UIManager.GetButton("breakBtn");
                upBtn.onClick.AddListener(() =>
                {
                    MusicManager.PlaySe("click");
                    if (Globals.player.gold >= 100)
                    {
                        if (HeroGlobals.upItemCount > 0)
                        {
                            //values
                            currentHero.exps[1] += 10;
                            HeroGlobals.upItemCount--;
                            Globals.player.gold -= 100;
                            for (int i = 0; i < Globals.items.Count; i++)
                            {
                                if (Globals.items[i].itemId == "001")//合成剂
                                {
                                    Globals.items[i].count = HeroGlobals.upItemCount;
                                }
                            }
                            if (currentHero.exps[1] >= currentHero.exps[2])//lv up
                            {
                                currentHero.exps[0]++;
                                for (int i = 0; i < currentHero.infos.Length; i++)
                                {
                                    currentHero.infos[i] += (currentHero.exps[0] - 1)*1;
                                }
                                currentHero.exps[1] -= currentHero.exps[2];
                                currentHero.exps[2] += (currentHero.exps[0] - 1) * 100;
                            }
                            ShowAll(panel);
                        }
                        else
                        {
                            GameFuncs.CreateMsg("合成剂不足！");
                        }
                    }
                    else
                    {
                        GameFuncs.CreateMsg("金币不足！");
                    }
                });
                breakBtn.onClick.AddListener(() =>
                {
                    MusicManager.PlaySe("click");
                    if (Globals.player.gold >= 1000)
                    {
                        if (HeroGlobals.breakItemCount > 0)
                        {
                            //values
                            currentHero.exps[0]++;
                            for (int i = 0; i < currentHero.infos.Length; i++)
                            {
                                currentHero.infos[i] += (currentHero.exps[0] - 1)*1;
                            }
                            HeroGlobals.breakItemCount--;
                            for (int i = 0; i < Globals.items.Count; i++)
                            {
                                if (Globals.items[i].itemId == "003")//突破晶石
                                {
                                    Globals.items[i].count = HeroGlobals.breakItemCount;
                                }
                            }
                            Globals.player.gold -= 1000;
                            currentHero.exps[1] = 0;
                            currentHero.exps[2] += (currentHero.exps[0] - 1) * 100;
                            //add skill
                            foreach (Skill s in Globals.skillList)
                            {
                                    if (!currentHero.skills.Contains(s))
                                    {
                                        if (!s.isCp && s.ele == currentHero.ele && s.lv <= currentHero.exps[0])//学会
                                        {
                                            currentHero.skills.Add(s);
                                            currentHero.skills.Sort();
                                            GameFuncs.CreateMsg("学会[" + s.sname + "]技能！");
                                            CreateSkill(s);
                                        }
                                    }
                            }
                            ShowAll(panel);
                        }
                        else
                        {
                            GameFuncs.CreateMsg("突破晶石不足！");
                        }
                    }
                    else
                    {
                        GameFuncs.CreateMsg("金币不足！");
                    }
                });
            }
            //Load Skill
            else if (infoName == "skill")
            {
                skillGrid = panel.Find("list/grid");
                infoText = panel.Find("infoText").GetComponent<Text>();
                List<Skill> skills = currentHero.skills;
                skills.Sort();
                //skills.Add(currentHero.superSkill);
                for (int i = 0; i < skills.Count; i++)
                {
                    Skill s = skills[i];
                    CreateSkill(s);
                }
            }
            //Load Dresses
            else if (infoName == "dress")
            {
                Image image = panel.Find("Image").GetComponent<Image>();
                Text text = panel.Find("Text").GetComponent<Text>();
                Button imageBtn= panel.Find("Image").GetComponent<Button>();
                Button nextBtn = panel.Find("nextBtn").GetComponent<Button>();
                nextBtn.interactable = dressIndex < currentHero.dresses.Count-1 ? true : false;
                Button preBtn = panel.Find("preBtn").GetComponent<Button>();
                preBtn.interactable = dressIndex > 0 ? true : false;
                nextBtn.onClick.AddListener(()=> {
                    MusicManager.PlaySe("click");
                    if (dressIndex < currentHero.dresses.Count-1)
                        dressIndex++;
                });
                nextBtn.onClick.AddListener(() => {
                    MusicManager.PlaySe("click");
                    if (dressIndex >0)
                        dressIndex--;
                });
                image.sprite = Resources.Load<Sprite>("Character/Portrait/"+ currentHero.dresses[0].imagePath);
                text.text = currentHero.dresses[0].dname;
                imageBtn.onClick.AddListener(() => { OndressBtnClick("Prefabs/"+ currentHero.dresses[0].modelPath); });
            }
        }
        Button setHeadBtn = UIManager.GetButton("setHeadBtn");//弹出对话框
        setHeadBtn.onClick.AddListener(() => {
            MusicManager.PlaySe("click");
            GameFuncs.CreateMsg("设为首席成功！");
            Globals.player.characterId = currentHero.id; });
        Button backBtn = UIManager.GetButton("backBtn");
        backBtn.onClick.AddListener(()=> { GameFuncs.GoToScene("Heroes"); });
    }
    void CreateSkill(Skill s)
    {
        GameObject skill = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/skill"));
        skill.transform.SetParent(skillGrid);
        skill.transform.Find("ele").GetComponent<Image>().sprite
            = Resources.Load<Sprite>("Icons/" + s.ele);
        skill.transform.Find("name").GetComponent<Text>().text = s.sname;
        if (s.isCp)
        {
            skill.transform.Find("cost").GetComponent<Text>().text = s.cost.ToString() + "cp";
        }
        else
        {
            skill.transform.Find("cost").GetComponent<Text>().text = s.cost.ToString() + "mp";
        }
        Toggle st = skill.GetComponent<Toggle>();
        st.group = skillGrid.GetComponent<ToggleGroup>();
        string info = s.info + "\n" + (s.isAoe ? "群体" : "单体");
        st.onValueChanged.AddListener((bool isOn) => { OnskillToggleClick(st, isOn, infoText, info, skillGrid); });
    }
    void ShowAll(Transform panel)
    {
        panel = GameFuncs.FindHiden("allPanelParent").transform;
        Text lvText = panel.Find("lvImage/lvText").GetComponent<Text>();
        Slider expSlider = panel.Find("expSlider").GetComponent<Slider>();
        lvText.text = currentHero.exps[0].ToString();
        expSlider.value = (float)currentHero.exps[1] / currentHero.exps[2];
        panel.Find("alls/hp").GetComponent<Text>().text = currentHero.infos[0].ToString();
        panel.Find("alls/mp").GetComponent<Text>().text = currentHero.infos[1].ToString();
        panel.Find("alls/atk").GetComponent<Text>().text = currentHero.infos[2].ToString();
        panel.Find("alls/def").GetComponent<Text>().text = currentHero.infos[3].ToString();
        panel.Find("alls/ats").GetComponent<Text>().text = currentHero.infos[4].ToString();
        panel.Find("alls/spd").GetComponent<Text>().text = currentHero.infos[5].ToString();
        //ui
        UIManager.ChangeText("upItemText", HeroGlobals.upItemCount.ToString());
        UIManager.ChangeText("breakItemText", HeroGlobals.breakItemCount.ToString());
        UIManager.ChangeText("goldText", Globals.player.gold.ToString());
    }
    void OninfoToggleClick(Toggle toggle, bool isOn)
    {
        MusicManager.PlaySe("click");
        string infoName = toggle.gameObject.name.Replace("Toggle", "");
        string panelParentName = infoName + "PanelParent";
        GameObject panel = GameFuncs.FindHiden(panelParentName);
        panel.SetActive(isOn);
    }
    void OnskillToggleClick(Toggle toggle, bool isOn, Text infoText,string info,Transform grid)
    {
        MusicManager.PlaySe("click");
        bool isEmpty = true;
        foreach (Transform t in grid)
        {
            if (t.GetComponent<Toggle>().isOn) { isEmpty = false; break; }
        }
        if (isEmpty) info = "";
        infoText.text = info;
    }
    void OndressBtnClick(string modelPath)
    {
        MusicManager.PlaySe("click");
        Transform point = GameObject.Find("point").transform;
        if (point.childCount>0)
        {
            Destroy(point.GetChild(0).gameObject);
        }
        GameObject hero = Instantiate((GameObject)GameFuncs.GetResource(modelPath), point);
        hero.AddComponent<OnHeroInfoModel>();
    }
}
