using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    public static BattleCamera Instance;
    List<Transform> positions = new List<Transform>();
    public static Animation anim;
    string currentIndex;
    static bool isStop;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        anim = GetComponent<Animation>();
        Transform cpoints = GameObject.Find("cpoints").transform;
        foreach (Transform p in cpoints)
        {
            if (!p.gameObject.name.Contains("ch"))
            {
                positions.Add(p);
            }
        }
        StartCoroutine(ChangeTransform());

    }
    IEnumerator ChangeTransform()
    {
        yield return new WaitForSeconds(6);
        SetPos(0);
    }
    private void Update()
    {
        if (BattleGlobals.isStop && BattleGlobals.currentObj != null)
        {
            if (BattleGlobals.currentObj.tag == "Enemy")//敌人视角
            {
                SetPos(1);
            }
            else if (BattleGlobals.currentObj.tag == "Hero")//英雄视角(animation)
            {
                //rotate to currentHero(play anim)
                if (!isStop)
                {
                    currentIndex = BattleGlobals.currentObj.transform.parent.gameObject.name[BattleGlobals.currentObj.transform.parent.gameObject.name.Length - 1].ToString();
                    anim.Play("camera_battleh" + currentIndex);
                    isStop = true;
                }
                if (BattleGlobals.isSelectEnemy)
                {
                    SetPos(0);
                }
            }
        }
        else if (!BattleGlobals.isStop)
        {
            SetPos(0);
        }
    }
    public void SetPos(int index)
    {
        transform.position = positions[index].position;
        transform.rotation = positions[index].rotation;
    }
    public static void SetIsStop()
    {
        isStop = false;
    }
    public static void SetAnimStop()
    {
        anim.Stop();
        isStop = true;
    }
    public static void PlayAnim(string name)
    {
        isStop = true;
        anim.Play("camera_battle" + name);
    }
    public static void Shake()
    {
        PlayAnim("_shake");
    }
}
