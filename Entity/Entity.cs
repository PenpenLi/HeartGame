using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public string id;
    public string ename;
    public string imagePath;
    public string ele;
    public int[] infos = new int[6]; //0:hp;1:mp;2:atk;3:def;4:ats;5:spd
    public List<Skill> skills = new List<Skill>();//json
    public bool isNearAttack;//是否近战(int)

    //Battle
    public string battleId;//战斗中的name
    public int currentHp;
    public Animator anim;
    public bool isDie = false;
    public Slider hpSlider;
    public float aeTime;//animator event time
    public Entity(string id, string ename, string imagePath, string ele, int[] infos,bool isNearAttack)
    {
        this.id = id;
        this.ename = ename;
        this.imagePath = imagePath;
        this.ele = ele;
        this.infos = infos;
        this.isNearAttack = isNearAttack;

        this.currentHp = infos[0];
        
    }
    int GetRestrict(Entity e,ref float value)
    {
        int restrict = 0;//相克
        //相克
        if (this.ele == "fire" && e.ele == "wind"
            || this.ele == "wind" && e.ele == "ice"
            || this.ele == "ice" && e.ele == "fire")
        {
            value *= 1.5f;
            restrict = 1;
        }
        if (e.ele == "fire" && this.ele == "wind"
            || e.ele == "wind" && this.ele == "ice"
            || e.ele == "ice" && this.ele == "fire")
        {
            value /= 1.5f;
            restrict = -1;
        }
        return restrict;
    }
    public virtual void Attack(Entity e)
    {
        
        float value = this.infos[2] - e.infos[3];//atk-def
        int restrict = GetRestrict(e,ref value);
        if (value <= 1) value = 1;
        e.GetHurt(value,restrict);
        AnimationClip ac = GameFuncs.GetAnim(anim, "attack");
        if (ac.events.Length<=0)
        {
            Invoke("SetReturn", GameFuncs.GetAnim(anim, "attack").length);
        }
    }
    public virtual void Magic(string sid,Entity other=null)
    {
        Skill s = GameFuncs.GetSkill(sid);
        Hero h = BattleGlobals.currentObj.GetComponent<Hero>();
        GameObject effect = Instantiate((GameObject)GameFuncs.GetResource("Effect/" + sid));
        Destroy(effect, 2);
        BattleCamera.Shake();
        int cost = s.isCp ? s.cost : s.cost / 5;
        float ats = s.isCp ? h.infos[4] : (h.infos[2] + h.infos[4]) / 2;
        float value = cost * ats;
        if (s.isAoe)
        {
            if (BattleGlobals.currentObj.tag == "Hero")
            {
                effect.transform.position = Battle.mone.position;
                List<string> temp = new List<string>();
                foreach (string lm in BattleGlobals.liveMonsters)
                {
                    temp.Add(lm);
                }
                for (int i = 0; i < temp.Count; i++)
                {
                    string es = temp[i];                    
                    Enemy en = BattleFuncs.FindObj(es).GetComponent<Enemy>();
                    en.GetHurt(value, GetRestrict(en, ref value));                    
                }
            }
            else if (BattleGlobals.currentObj.tag == "Enemy")
            {
                List<string> temp = BattleGlobals.liveHeroes;
                for (int i = 0; i < temp.Count; i++)
                {
                    string hs = temp[i];
                    Hero he = BattleFuncs.FindObj(hs).GetComponent<Hero>();
                    he.GetHurt(value,GetRestrict(he,ref value));
                }
            }
        }
        else
        {
            effect.transform.position = other.gameObject.transform.position;
            other.GetHurt(value,GetRestrict(other,ref value));
        }
        BattleGlobals.currentSid = "";
        Invoke("SetTurnOver", 2);
    }
    public virtual void GetHurt(float value,int restrict = 0)
    {
        //create damage in Canvas
        MusicManager.PlaySe("hit");
        Transform canvas = transform.Find("Canvas");
        GameObject damage = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/damage"),canvas);
        Text dt = damage.GetComponent<Text>();
        dt.text = "-" + (int)value;
        switch (restrict)
        {
            case 1:
                dt.color = Color.yellow;
                break;
            case -1:
                dt.color = Color.blue;
                break;
            default:
                dt.color = Color.white;
                break;
        }
        currentHp -= Mathf.RoundToInt(value);
        if (currentHp <= 0)
        {
            currentHp = 0;
            //Die
            Die();
        }
        hpSlider.value = (float)currentHp / infos[0];
    }
    public virtual void Die()
    {
        isDie = true;
        BattleFuncs.DestroyHead(id);
        if (BattleGlobals.canEnterNext && BattleGlobals.currentSid!="")
        {
            if (BattleGlobals.currentSid == "")
            {
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
                {
                    BattleFuncs.Instance.EnterNextWave();
                    BattleGlobals.canEnterNext = false;
                }, 1.9f));
            }
            else
            {
                BattleGlobals.canEnterNext = false;
                StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
                {
                    BattleFuncs.Instance.EnterNextWave();    
                }, 1.9f));
            }
        }
    }

    public virtual void Win()
    {
        anim.SetBool("win", true);
    }
    void SetReturn()
    {
        BattleGlobals.isReturn = true;//近战
    }
    void SetTurnOver()
    {
        BattleGlobals.isNearAttack = true;
        BattleGlobals.isStop = false;
        BattleGlobals.currentObj = null;
        BattleGlobals.otherObjs.Clear();
    }
}
