using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public static List<Transform> hpoints = new List<Transform>();
    public static List<Transform> mpoints = new List<Transform>();
    public static Transform mone;//only one enemy
    public static List<string> currentMonsters = new List<string>();
    public static bool canEnter = true;
    // Use this for initialization
    void Start()
    {
        BattleFuncs.ResetBattle();
        BattleGlobals.monsters = JsonFuncs.LoadEnemys(BattleGlobals.placeName);
        MusicManager.PlayBgm("battle");
        MusicManager.SetVolume();
        //Load Infos
        string place = BattleGlobals.placeName;
        BattleGlobals.currentWave = 1;
        currentMonsters = BattleGlobals.monsters[BattleGlobals.currentWave - 1];

        RenderSettings.skybox = (Material)GameFuncs.GetResource("SkyBox/" + place);
        GameFuncs.FindHiden(place + "Parent").SetActive(true);
        //Load Transforms
        BattleGlobals.hpoints = GameObject.Find("hpoints").transform;
        BattleGlobals.mpoints = GameObject.Find("mpoints").transform;
        foreach (Transform t in BattleGlobals.hpoints)
        {
            hpoints.Add(t);
        }
        foreach (Transform t in BattleGlobals.mpoints)
        {
            if (!t.gameObject.name.Contains("one"))
            {
                mpoints.Add(t);
            }
            else
            {
                mone = t;
            }
        }
        //Create Hero
        for (int i = 0; i < BattleGlobals.selectHeroes.Count; i++)
        {
            //create hero
            GameObject hero = Instantiate((GameObject)GameFuncs.GetResource
                ("Prefabs/" + BattleGlobals.selectHeroes[i]), hpoints[i]);
            Hero h = hero.AddComponent<Hero>();
            GameFuncs.CopyHero(h, BattleGlobals.selectHeroes[i]);
            h.anim = hero.GetComponent<Animator>();
            h.currentMp = h.infos[1];
            h.cp = 100;
            hero.name = "h" + i;
            h.battleId = hero.name;
            BattleGlobals.liveHeroes.Add(hero.name);
        }
        CreateCurrentEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleGlobals.currentObj != null && BattleGlobals.otherObjs.Count > 0)//准备攻击
        {
            GameObject co = BattleGlobals.currentObj;
            GameObject oo = BattleGlobals.otherObjs[0];
            if (BattleGlobals.isNearAttack)//移动到对方处攻击（近战）
            {
                Transform point = co.transform.parent;
                Vector3 atkPos = co.tag == "Enemy" ? oo.transform.position + Vector3.right * 2
                    : oo.transform.position - Vector3.right * 2;
                if (BattleGlobals.isReturn == false)
                {
                    co.GetComponent<Animator>().SetBool("run", true);
                    co.transform.position = Vector3.MoveTowards
                        (co.transform.position, atkPos, Time.deltaTime * 20);
                    if (Vector3.Distance(co.transform.position, atkPos) <= 1)
                    {
                        co.GetComponent<Animator>().SetBool("run", false);
                    }
                }
                else
                {
                    //转身
                    co.transform.rotation = Quaternion.Slerp(co.transform.rotation,
                        Quaternion.LookRotation(point.position - co.transform.position),
                        Time.deltaTime * 30);
                    co.GetComponent<Animator>().SetBool("run", true);
                    co.transform.position = Vector3.MoveTowards
                    (co.transform.position, point.position, Time.deltaTime * 20);
                    if (Vector3.Distance(co.transform.position, point.position) <= 1)
                    {
                        //co.transform.eulerAngles = point.transform.eulerAngles;
                        co.transform.localEulerAngles = Vector3.zero;
                        co.transform.localPosition = Vector3.zero;
                        co.GetComponent<Animator>().SetBool("run", false);
                        BattleGlobals.isReturn = false;
                        if (BattleGlobals.canEnterNext)
                        {
                            BattleFuncs.Instance.EnterNextWave();
                            BattleGlobals.canEnterNext = false;
                        }
                        BattleGlobals.isStop = false;
                        BattleGlobals.currentObj = null;
                        BattleGlobals.otherObjs.Clear();
                    }
                }
            }
        }
    }
    public static void CreateCurrentEnemy()
    {
        //Create Enemy
        for (int i = 0; i < currentMonsters.Count; i++)
        {
            //create enemy
            Enemy e = (Enemy)GameFuncs.GetEntity(currentMonsters[i]);
            GameObject monster = null;
            if (currentMonsters.Count > 1)
            {
                monster = Instantiate((GameObject)GameFuncs.GetResource
               ("Prefabs/" + currentMonsters[i]), mpoints[i]);
            }
            else
            {
                monster = Instantiate((GameObject)GameFuncs.GetResource
               ("Prefabs/" + currentMonsters[i]), mone);
            }
            monster.name = "m"+i;
            Enemy en = monster.AddComponent<Enemy>();
            monster.AddComponent<OnEnemy>();
            GameFuncs.CopyEntity(en, currentMonsters[i]);
            en.anim = monster.GetComponent<Animator>();
            en.battleId = monster.name;
            BattleGlobals.liveMonsters.Add(monster.name);
            //enemys'ui
            Transform canvas = monster.transform.Find("Canvas");
            canvas.Find("name").GetComponent<Text>().text = e.ename;
            Image ele = canvas.Find("ele/Image").GetComponent<Image>();
            ele.sprite = Resources.Load<Sprite>("Icons/"+e.ele);
            Slider hpSlider = canvas.Find("hp").GetComponent<Slider>();
            hpSlider.value = (float)e.currentHp / e.infos[0];
            en.hpSlider = hpSlider;
        }
    }
}
