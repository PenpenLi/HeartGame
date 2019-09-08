using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public static GameObject canvas;
    GameObject set;
    public static GameObject heroPanel;
    GameObject skillPanel;
    Slider bgmSlider;
    Slider seSlider;
    Slider voiceSlider;
    Button closeSetBtn;
    public static List<GameObject> heads = new List<GameObject>();
    public static Transform bar;
    Transform start;
    Transform end;
    public static GameObject winPanel;
    public static GameObject losePanel;
    public static GameObject tip;
    public static GameObject getsBtn;
    public static GameObject gets;
    public static GameObject summonGets;
    // Use this for initialization
    void Start()
    {
        canvas = transform.Find("canvas").gameObject;
        canvas.SetActive(false);
        winPanel = GameFuncs.FindHiden("winPanelParent");
        losePanel = GameFuncs.FindHiden("losePanelParent");
        tip = transform.Find("tipText").gameObject;
        getsBtn = transform.Find("getsBtn").gameObject;
        gets = GameFuncs.FindHiden("GetsParent");
        StartCoroutine(IShowUI());
    }
    IEnumerator IShowUI()
    {
        yield return new WaitForSeconds(6);
        canvas.SetActive(true);
        UIManager.ChangeText("waveText", "Battle " +
            BattleGlobals.currentWave.ToString() + "/" + BattleGlobals.waves.ToString());
        //Set
        set = GameFuncs.FindHiden("setParent");
        heroPanel = GameFuncs.FindHiden("heroPanelParent");
        skillPanel = GameFuncs.FindHiden("skillPanelParent");
        Button setBtn = UIManager.GetButton("setBtn");
        setBtn.onClick.AddListener(OnsetBtnClick);
        //create state(image,hp,mp,cp)
        for (int i = 0; i < BattleGlobals.selectHeroes.Count; i++)
        {
            string hid = BattleGlobals.selectHeroes[i];
            string id = "h" + i;
            GameObject heroObj = BattleFuncs.FindObj(id);
            Hero hero = heroObj.GetComponent<Hero>();
            Transform states = GameObject.Find("States").transform;
            GameObject state = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/state"));
            state.transform.SetParent(states);
            state.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Character/Image/" + hid + "_001");
            Slider hpSlider = state.transform.Find("hpSlider").GetComponent<Slider>();
            Slider mpSlider = state.transform.Find("mpSlider").GetComponent<Slider>();
            Slider cpSlider = state.transform.Find("cpSlider").GetComponent<Slider>();
            hpSlider.value = (float)hero.currentHp / hero.infos[0];
            mpSlider.value = (float)hero.currentMp / hero.infos[1];
            cpSlider.value = (float)hero.cp / 100;
            Text hpText = state.transform.Find("hpText").GetComponent<Text>();
            Text mpText = state.transform.Find("mpText").GetComponent<Text>();
            Text cpText = state.transform.Find("cpText").GetComponent<Text>();
            hpText.text = hero.currentHp.ToString() + "/" + hero.infos[0].ToString();
            mpText.text = hero.currentMp.ToString() + "/" + hero.infos[1].ToString();
            cpText.text = hero.cp.ToString() + "/100";
            hero.hpSlider = hpSlider;
            hero.mpSlider = mpSlider;
            hero.cpSlider = cpSlider;
            hero.hpText = hpText;
            hero.mpText = mpText;
            hero.cpText = cpText;
        }
        //create heads
        bar = canvas.transform.Find("Bar");
        start = bar.Find("start");
        end = bar.Find("end");
        CreateHeads();
        StartCoroutine(IStartBattle());
    }
    IEnumerator IStartBattle()
    {
        yield return new WaitForSeconds(2);
        BattleGlobals.isStop = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!BattleGlobals.isStop && !BattleCamera.anim.isPlaying)
        {
            BattleGlobals.canEnterNext = true;
            BattleGlobals.currentObj = null;
            BattleGlobals.otherObjs.Clear();
            //heads move by speed
            foreach (GameObject head in heads)
            {
                string id = head.name;
                Entity e =BattleFuncs.FindObj(id).GetComponent<Entity>() ;
                //move
                head.transform.Translate(Vector3.right * e.infos[5]);
                head.transform.position = new Vector2(Mathf.Clamp(
                    head.transform.position.x, start.position.x, end.position.x), head.transform.position.y);
                //turn
                if (Vector2.Distance(head.transform.position, end.position) <= 1)
                {
                    BattleGlobals.currentObj = null;
                    BattleGlobals.otherObjs.Clear();
                    BattleGlobals.isStop = true;
                    BattleGlobals.currentObj = BattleFuncs.FindObj(id);
                    if (BattleGlobals.currentObj.tag == "Enemy")
                    {
                        if (true) BattleGlobals.isNearAttack = true;//近战
                        if (BattleGlobals.liveHeroes.Count > 0)
                        {
                            GameObject ranHero = BattleFuncs.FindObj(BattleGlobals.liveHeroes
                        [Random.Range(0, BattleGlobals.liveHeroes.Count)]);
                            BattleGlobals.otherObjs.Clear();
                            BattleGlobals.otherObjs.Add(ranHero);
                        }
                    }
                    else if (BattleGlobals.currentObj.tag == "Hero")
                    {
                        if (!BattleGlobals.isOver)
                        {
                            BattleCamera.SetIsStop();
                            //active hero ui
                            heroPanel.SetActive(true);
                            //Bind heroPanelBtns
                            Button atkBtn = UIManager.GetButton("atkBtn");
                            Button skillBtn = UIManager.GetButton("skillBtn");
                            Button defBtn = UIManager.GetButton("defBtn");
                            atkBtn.onClick.AddListener(OnatkBtnClick);
                            skillBtn.onClick.AddListener(OnskillBtnClick);
                            defBtn.onClick.AddListener(OndefBtnClick);
                        }
                    }
                    head.transform.position = start.position;
                }
            }
            if (heroPanel.activeInHierarchy)
            {
                BattleGlobals.isStop = true;
            }
        }
        else//停止时获得当前对象
        {
            if (heroPanel != null && heroPanel.activeInHierarchy && !BattleCamera.anim.isPlaying)
            {
                foreach (GameObject head in heads)
                {
                    string id = head.name;
                    if (!id.Contains("m"))
                    {
                        if (Vector2.Distance(head.transform.position, start.position) <= 1)
                        {
                            BattleGlobals.currentObj = null;
                            BattleGlobals.otherObjs.Clear();
                            BattleGlobals.currentObj = BattleFuncs.FindObj(id);
                        }
                    }
                    else
                    {
                        BattleFuncs.FindObj(id).GetComponent<Animator>().SetBool("run", false);
                    }
                }
            }
        }
    }
    #region Set
    void OnsetBtnClick()
    {
        //PlaySE
        MusicManager.PlaySe("click");
        //UI:BGM SE(Global)
        set.SetActive(true);

        bgmSlider = UIManager.GetSlider("bgmSlider");
        seSlider = UIManager.GetSlider("seSlider");
        voiceSlider = UIManager.GetSlider("voiceSlider");

        bgmSlider.onValueChanged.AddListener(delegate { OnBgmSliderChange(); });
        seSlider.onValueChanged.AddListener(delegate { OnSeSliderChange(); });
        voiceSlider.onValueChanged.AddListener(delegate { OnVoiceSliderChange(); });

        bgmSlider.value = Globals.bgmVolume;
        seSlider.value = Globals.seVolume;
        voiceSlider.value = Globals.voiceVolume;

        closeSetBtn = set.transform.Find("closeSetBtn").GetComponent<Button>();
        closeSetBtn.onClick.AddListener(OncloseSetBtnClick);

        UIManager.GetButton("backBtn").onClick.AddListener(() =>
        {
            BattleGlobals.selectHeroes.Clear();
            GameFuncs.GoToSceneAsync("Main");
        });
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
    void OnVoiceSliderChange()
    {
        if (voiceSlider != null)
        {
            if (voiceSlider.gameObject.activeInHierarchy)
            {
                Globals.voiceVolume = voiceSlider.value;
            }
        }
        MusicManager.SetVolume();
    }
    void OncloseSetBtnClick()
    {
        //PlaySE
        //MusicManager.PlaySe("click");
        if (closeSetBtn != null && set.activeInHierarchy)
        {
            set.SetActive(false);
        }
    }
    #endregion
    #region heroPanel
    void OnatkBtnClick()
    {
        MusicManager.PlaySe("click");
        BattleCamera.SetAnimStop();
        if (true) BattleGlobals.isNearAttack = true;//近战
        BattleGlobals.isSelectEnemy = true;
        heroPanel.SetActive(false);
    }
    void OnskillBtnClick()
    {
        MusicManager.PlaySe("click");
        //show skill panel
        skillPanel.SetActive(true);
        Transform panel = skillPanel.transform;
        Button closeSkillBtn = UIManager.GetButton("closeSkillBtn");
        closeSkillBtn.onClick.AddListener(() => { OncloseSkillBtn(panel); });
        Button okSkillBtn = UIManager.GetButton("okSkillBtn");
        okSkillBtn.onClick.AddListener(() => { OnokSkillBtn(panel); });
        //create skill list
        Hero currentHero = BattleGlobals.currentObj.GetComponent<Hero>();
        List<Skill> skills = currentHero.skills;
        for (int i = 0; i < skills.Count; i++)
        {
            Skill s = skills[i];
            if (s.ele == currentHero.ele)
            {
                Transform grid = panel.Find("list/grid");
                Text infoText = panel.Find("infoText").GetComponent<Text>();
                GameObject skill = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/skill"));
                skill.transform.SetParent(grid);
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
                st.group = grid.GetComponent<ToggleGroup>();
                string info = s.info + "\n" + (s.isAoe ? "群体" : "单体");
                st.onValueChanged.AddListener((bool isOn) => { OnskillToggleClick(st, isOn, infoText, s, grid); });
            }
        }
        for (int i = skills.Count; i < panel.Find("list/grid").childCount; i++)
        {
            Destroy(panel.Find("list/grid").GetChild(i).gameObject);
        }
    }
    void OnskillToggleClick(Toggle toggle, bool isOn, Text infoText, Skill s, Transform grid)
    {
        MusicManager.PlaySe("click");
        //MusicManager.PlaySe("click");
        bool isEmpty = true;
        foreach (Transform t in grid)
        {
            if (t.GetComponent<Toggle>().isOn) { isEmpty = false; break; }
        }
        string info = s.info + "\n" + (s.isAoe ? "群体" : "单体");
        if (isEmpty) info = "";
        infoText.text = info;
        BattleGlobals.currentSid = isOn ? s.sid : "";
    }
    void OncloseSkillBtn(Transform panel)
    {
        MusicManager.PlaySe("click");
        foreach (Transform s in panel.Find("list/grid"))
        {
            Destroy(s.gameObject);
        }
        skillPanel.SetActive(false);
    }
    void OnokSkillBtn(Transform panel)
    {
        MusicManager.PlaySe("click");
        if (BattleGlobals.currentSid != "")
        {
            Skill s = GameFuncs.GetSkill(BattleGlobals.currentSid);
            Hero h = BattleGlobals.currentObj.GetComponent<Hero>();
            if ((!s.isCp && h.currentMp >= s.cost) || (s.isCp && h.cp >= s.cost))//可消耗
            {
                BattleCamera.SetAnimStop();
                skillPanel.SetActive(false);
                heroPanel.SetActive(false);
                if (!s.isAoe)//单体
                {
                    BattleGlobals.isSelectEnemy = true;
                }
                else
                {
                    BattleCamera.Instance.SetPos(0);
                    BattleGlobals.currentObj.GetComponent<Hero>().Magic(BattleGlobals.currentSid);
                }
                foreach (Transform so in panel.Find("list/grid"))
                {
                    Destroy(so.gameObject);
                }
                BattleGlobals.isMagic = true;
            }
            else//mp(cp)不足
            {
                string msg = (s.isCp ? "cp" : "mp") + "不足！";
                GameFuncs.CreateMsg(msg);
            }
        }
    }
    void OndefBtnClick()
    {
        MusicManager.PlaySe("click");
        if (BattleGlobals.currentObj != null && BattleGlobals.currentObj.tag == "Hero")
        {
            BattleCamera.SetAnimStop();
            Hero hero = BattleGlobals.currentObj.GetComponent<Hero>();
            hero.isDef = true;
            heroPanel.SetActive(false);
            BattleGlobals.isStop = false;
            BattleGlobals.currentObj = null;
            BattleGlobals.otherObjs.Clear();
        }
    }
    #endregion
    public static GameObject CreateHead(string id, bool isHero, int index)
    {
        Sprite sprite = Resources.Load<Sprite>("Character/Portrait/" + id);
        GameObject head = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/head"), bar);
        head.transform.Find("Image").GetComponent<Image>().sprite = sprite;
        if (isHero) head.name = "h" + index;
        else head.name = "m" + index;
        return head;
    }
    public static void CreateHeads()
    {
        heads.Clear();
        foreach (Transform head in bar)
        {
            if (!head.gameObject.name.Contains("start") && !head.gameObject.name.Contains("end"))
            {
                Destroy(head.gameObject);
            }
        }
        //create new heads
        for (int i = 0; i < BattleGlobals.liveHeroes.Count; i++)
        {
            string id = BattleFuncs.FindObj(BattleGlobals.liveHeroes[i]).GetComponent<Entity>().id;
            heads.Add(CreateHead(id, true, i));
        }

        for (int i = 0; i < BattleGlobals.monsters[BattleGlobals.currentWave - 1].Count; i++)
        {
            string id = BattleGlobals.monsters[BattleGlobals.currentWave - 1][i];
            heads.Add(CreateHead(id, false, i));
        }


    }
    public static void HideUI()
    {
        canvas.SetActive(false);
    }
    public static void ShowWinPanel()
    {
        //BattleGlobals.selectHeroes.Clear();
        //foreach (Transform t in Battle.hpoints)
        //{
        //    foreach (Transform h in t)
        //    {
        //        string hid = h.gameObject.name.Substring(0, 3);
        //        BattleGlobals.selectHeroes.Add(hid);
        //    }
        //}
        winPanel.SetActive(true);
        tip.SetActive(true);
        getsBtn.SetActive(true);
        Transform win = winPanel.transform;
        winPanel.GetComponent<Button>().onClick.AddListener(() => { GameFuncs.GoToSceneAsync("Main"); });
        Transform grid = win.Find("winGrid");
        //gets
        getsBtn.GetComponent<Button>().onClick.AddListener(OngetsBtnClick);
        //create heroWin
        for (int i = 0; i < Globals.heroes.Count; i++)
        {
            string id = Globals.heroes[i].id;
            if (BattleGlobals.selectHeroes.Contains(id))
            {
                Hero h = Globals.heroes[i];
                GameObject winHero = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/heroWin"));
                winHero.transform.SetParent(grid);
                winHero.GetComponent<Image>().sprite = Resources.Load<Sprite>("Character/Portrait/" + id);
                winHero.transform.Find("name").GetComponent<Text>().text = h.ename;
                Slider expSlider = winHero.transform.Find("expSlider").GetComponent<Slider>();
                Text lvText = winHero.transform.Find("lvImage/lvText").GetComponent<Text>();
                Slider loveSlider = winHero.transform.Find("loveSlider").GetComponent<Slider>();
                //exp
                h.exps[1] += 10;
                if (h.exps[1] >= h.exps[2])//lv up
                {
                    h.exps[0]++;
                    for (int j = 0; j < h.infos.Length; j++)
                    {
                        h.infos[j] += (h.exps[0] - 1) * 10;
                    }
                    h.exps[1] -= h.exps[2];
                    h.exps[2] += (h.exps[0] - 1) * 100;
                    //add skill
                    foreach (Skill s in Globals.skillList)
                    {
                        if (!s.isCp && s.ele == h.ele && s.lv <= h.exps[0])//学会
                        {
                            h.skills.Add(s);
                            h.skills.Sort();
                        }
                    }
                }
                expSlider.value = (float)h.exps[1] / h.exps[2];
                lvText.text = h.exps[0].ToString();
                //love
                h.li.love++;
                if (h.li.love >= h.li.maxLove)
                {
                    h.li.lv++;
                    h.li.love -= h.li.maxLove;
                }
                loveSlider.value = (float)h.li.love / h.li.maxLove;
                loveSlider.transform.Find("loveText").GetComponent<Text>().text =
                    h.li.love.ToString() + "/" + h.li.maxLove.ToString();
                loveSlider.transform.Find("Icon/lvText").GetComponent<Text>().text = h.li.lv.ToString();
            }
        }
        BattleGlobals.selectHeroes.Clear();
    }
    public static void ShowLosePanel()
    {
        MusicManager.StopBgm();
        losePanel.SetActive(true);
        tip.SetActive(true);
        Transform lose = losePanel.transform;
        losePanel.GetComponent<Button>().onClick.AddListener(() =>
        { GameFuncs.GoToSceneAsync("Main"); });
        BattleGlobals.selectHeroes.Clear();
    }
    static void OngetsBtnClick()
    {
        MusicManager.PlaySe("click");
        gets.SetActive(true);
        summonGets = GameObject.Find("summonGets");
        for (int i = 0; i < Random.Range(0, 11); i++)
        {
            Item item = Globals.itemList[Random.Range(0, Globals.itemList.Count)];
            bool isGetItem = false;
            for (int j = 0; j < Globals.items.Count; j++)
            {
                if (Globals.items[j].itemId == item.itemId)
                {
                    isGetItem = true;
                    Globals.items[j].count++;
                    break;
                }
            }
            if (!isGetItem)
            {
                Globals.items.Add(item);
            }
            GameObject summonGet = GameObject.Instantiate(
                (GameObject)GameFuncs.GetResource("Prefabs/summonGet"),
                summonGets.transform);
            string imagePath = "Icons/" + item.imagePath;
            summonGet.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
        }
    }
}
