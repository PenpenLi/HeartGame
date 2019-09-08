using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleFuncs : MonoBehaviour
{
    public static BattleFuncs Instance;
    private void Awake()
    {
        Instance = this;
    }
    public static GameObject FindObj(string id)
    {
        foreach (Transform point in BattleGlobals.hpoints)
        {
            foreach (Transform t in point)
            {
                if (t.gameObject.name.Contains(id)) return t.gameObject;
            }
        }
        foreach (Transform point in BattleGlobals.mpoints)
        {
            foreach (Transform t in point)
            {
                if (t.gameObject.name.Contains(id)) return t.gameObject;
            }
        }
        return null;
    }
    public static void DestroyHead(string id)
    {
        foreach (GameObject head in BattleUI.heads)
        {
            string hid = head.transform.Find("Image").GetComponent<Image>().sprite.name;
            if (hid == id)
            {
                Destroy(head);
                BattleUI.heads.Remove(head);
                break;
            }
        }
    }
    public void EnterNextWave()
    {
        if (BattleGlobals.liveMonsters.Count <= 0)//这波敌人死亡
        {
            BattleGlobals.isStop = true;
            if (BattleGlobals.currentWave < BattleGlobals.waves)//进入下一波
            {
                BattleGlobals.currentObj = null;
                BattleGlobals.otherObjs.Clear();
                foreach (Transform mp in GameObject.Find("mpoints").transform)
                {
                    foreach (Transform m in mp)
                    {
                        Destroy(m.gameObject);
                    }
                }
                //wave ui
                BattleGlobals.currentWave++;
                UIManager.ChangeText("waveText", "Battle " +
           BattleGlobals.currentWave.ToString() + "/" + BattleGlobals.waves.ToString());
                //create monsters
                Battle.currentMonsters = BattleGlobals.monsters[BattleGlobals.currentWave - 1];
                Battle.CreateCurrentEnemy();
                BattleUI.CreateHeads();
                //is boss
                if (BattleGlobals.currentWave == BattleGlobals.waves)
                {
                    BattleGlobals.isStop = true;
                    BattleUI.canvas.SetActive(false);
                    //boss(play boss bgm;hp slider;camera anim)
                    MusicManager.PlayBgm("boss");
                    BattleCamera.PlayAnim("_boss");
                    StartCoroutine(DelayToInvoke.DelayToInvokeDo(WaitNotStop, 6));
                }
                else
                {
                    StartCoroutine(DelayToInvoke.DelayToInvokeDo(WaitNotStop, 0.2f));
                }
            }
            else//win
            {
                BattleGlobals.isOver = true;
                BattleUI.HideUI();
                //if current hero else random(live heroes) -> set win bool true
                GameObject winObj = null;
                Hero winHero = null;
                if (BattleGlobals.currentObj.tag == "Hero")
                {
                    winObj = BattleGlobals.currentObj;
                }
                else
                {
                    winObj = BattleFuncs.FindObj
                        (BattleGlobals.liveHeroes[Random.Range(0, BattleGlobals.liveHeroes.Count - 1)]);
                }
                //camera 
                string winIndex = winObj.transform.parent.gameObject.name
                    [winObj.transform.parent.gameObject.name.Length - 1].ToString();
                BattleCamera.SetAnimStop();
                BattleCamera.PlayAnim("h" + winIndex);
                winHero = BattleGlobals.currentObj.GetComponent<Hero>();
                winHero.Win();
                //show win ui(every select heroes exp++;date(attribute) ++;click->return select map)
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(WaitShowWin, 4f));
            }
        }
        if (BattleGlobals.liveHeroes.Count <= 0)//英雄全部死亡，失败
        {
            BattleGlobals.isOver = true;
            BattleGlobals.isStop = true;
            BattleUI.HideUI();
            //show lose ui(click->return select map)
            BattleUI.ShowLosePanel();
        }
    }
    void WaitNotStop()
    {
        BattleUI.canvas.SetActive(true);
        BattleGlobals.isStop = true;
        StartCoroutine(DelayToInvoke.DelayToInvokeDo(() => { BattleGlobals.isStop = false; }, 2));
    }
    void WaitShowWin()
    {
        BattleUI.ShowWinPanel();
    }
    public static void ResetBattle()
    {
        BattleGlobals.liveHeroes.Clear();
        BattleGlobals.liveMonsters.Clear();
        BattleGlobals.monsters = new List<string>[] { };
        BattleGlobals.currentObj = null;
        BattleGlobals.otherObjs.Clear();
        BattleGlobals.isMagic = false;//是否是魔法
        BattleGlobals.isStop = true;//进度条停止
        BattleGlobals.isReturn = false;//返回途中
        BattleGlobals.isSelectEnemy = false;//正在选择敌人
        BattleGlobals.isNearAttack = false;//近战攻击
        BattleGlobals.isOver = false;//进入结算页面
        BattleGlobals.canEnterNext = true;//进入下一波
        BattleGlobals.hpoints = null;
        BattleGlobals.mpoints = null;
        BattleGlobals.currentWave = 1;//现在波数
        BattleGlobals.waves = 2;//总波数
        BattleGlobals.currentSid = "";
        Battle.mpoints = new List<Transform>();
        Battle.mone=null;//only one enemy
        Battle.currentMonsters = new List<string>();

    }
}
