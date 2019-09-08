using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Skill :IComparable{
    public string sid;
    public string sname;
    public string ele;
    public string info;
    public int cost;
    public bool isAoe;//(int)
    public bool isCp;//(int)
    public int lv;//达到等级
    public Skill(string sid,string sname,string ele,string info,int cost,bool isAoe,bool isCp,int lv)
    {
        this.sid = sid;
        this.sname = sname;
        this.ele = ele;
        this.info = info;
        this.cost = cost;
        this.isAoe = isAoe;
        this.isCp = isCp;
        this.lv = lv;
    }

    public int CompareTo(object obj)
    {
        Skill s = (Skill)obj;
        return sid.CompareTo(s.sid);
    }
}
