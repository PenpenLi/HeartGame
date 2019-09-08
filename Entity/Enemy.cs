using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Enemy : Entity {

    public Enemy(string id, string ename, string imagePath, string ele, int[] infos,bool isNearAttack)
       : base(id, ename, imagePath, ele, infos,isNearAttack)
    {
        this.id = id;
        this.ename = ename;
        this.imagePath = imagePath;
        this.ele = ele;
        this.infos = infos;
        this.isNearAttack = isNearAttack;
    }
    public override void Attack(Entity other)
    {
        anim.SetBool("attack",true);
        base.Attack(other);
        StartCoroutine(WaitSetBool(GameFuncs.GetAnim(anim, "attack").length, "attack"));
    }
    public override void Magic(string sid, Entity other = null)//不用
    {
        anim.SetBool("magic", true);
        base.Magic(sid,other);
        StartCoroutine(WaitSetBool(GameFuncs.GetAnim(anim, "magic").length, "magic"));
    }
    public override void GetHurt(float value,int restrict=0)
    {
        anim.SetBool("damage", true);
        base.GetHurt(value,restrict);
        StartCoroutine(WaitSetBool(GameFuncs.GetAnim(anim,"damage").length,"damage"));
    }
    public override void Die()
    {
        anim.SetBool("dead", true);
        BattleGlobals.liveMonsters.Remove(battleId);
        Destroy(GetComponent<OnEnemy>());
        base.Die();
    }
    public override void Win()
    {
        base.Win();
    }
    IEnumerator WaitSetBool (float time,string name)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool(name,false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Hero")
        {
            if (BattleGlobals.currentObj == gameObject)//attack
            {
                anim.SetBool("run", false);
                Hero hero = collision.gameObject.GetComponent<Hero>();
                Attack(hero);
            }
        }
    }
}
