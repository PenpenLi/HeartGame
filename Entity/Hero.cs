using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class Hero : Entity {
    public int[] exps = new int[3];//0:lv;1:exp;2:maxExp
    public LoveInfo li;
    public List<Dress> dresses = new List<Dress>();
    public Skill superSkill;
    //Battle
    public int currentMp;
    public int cp;
    public Slider mpSlider;
    public Slider cpSlider;
    public Text hpText;
    public Text mpText;
    public Text cpText;
    public bool isDef;
    Enemy en;
    public Hero(string id, string ename, string imagePath, string ele, int[] infos,bool isNearAttack,
        Skill superSkill)
        :base(id, ename, imagePath, ele, infos,isNearAttack)
    {
        this.id = id;
        this.ename = ename;
        this.imagePath = imagePath;
        this.ele = ele;
        this.infos = infos;
        this.isNearAttack = isNearAttack;
        this.superSkill = superSkill;

        this.currentMp = infos[1];
        this.exps = new int[3] {1,0,100 };
        this.li = new LoveInfo(1,0); 
    }
    public override void Attack(Entity other)
    {
        MusicManager.PlayVoice(id + "_attack");
        base.Attack(other);
        cp += 5;
        if (cp > 100) cp -= (cp - 100);
        cpSlider.value = (float)cp / 100;
        cpText.text = cp.ToString() + "/100";
    }
    public override void Magic(string sid, Entity other = null)
    {
        BattleGlobals.isNearAttack = false;
        if (sid == "") return;
        Skill s = GameFuncs.GetSkill(sid);
        if (!s.isCp)
        {
            currentMp -= s.cost;
            mpSlider.value = (float)currentMp / infos[1];
            mpText.text = currentMp.ToString() + "/" + infos[1].ToString();
        }
        else
        {
            cp -= s.cost;
            if (cp <= 0) cp = 0;
            cpSlider.value = (float)cp / 100;
            cpText.text = cp.ToString() + "/100";
        }
        anim.SetTrigger("magic");
        if(s.isCp) MusicManager.PlayVoice(id + "_magic");
        else MusicManager.PlayVoice(id + "_skill");
        base.Magic(sid,other);
    }
    public override void GetHurt(float value, int restrict=0)
    {
        anim.SetTrigger("hurt");
        MusicManager.PlayVoice(id + "_hurt");
        if (isDef) value /= 2;
        base.GetHurt(value,restrict);
        hpText.text= currentHp.ToString() + "/" + infos[0].ToString();
        cp += 10;
        if (cp > 100) cp -= (cp - 100);
        cpSlider.value = (float)cp / 100;
        cpText.text=cp.ToString() + "/100";
        isDef = false;
    }
    public override void Die()
    {
        anim.SetTrigger("die");
        BattleGlobals.liveHeroes.Remove(battleId);
        base.Die();
    }
    public override void Win()
    {
        MusicManager.PlayBgm("win");
        MusicManager.PlayVoice(id + "_win");
        base.Win();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Enemy")
        {
            if (BattleGlobals.currentObj == gameObject)//attack
            {
                anim.SetTrigger("attack");
                en = collision.gameObject.GetComponent<Enemy>();
                if (id == "002") { Attack(en); }
                else {
                    StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
             {
                 BattleGlobals.isReturn = true;
             }, GameFuncs.GetAnim(anim, "attack").length)); }
            }
        }
    }
    public void AtkOther()
    {
        Attack(en);
    }
}
