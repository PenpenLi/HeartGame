using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LoveInfo{
    public int lv;
    public int love;
    public int maxLove;
    public LoveInfo() { }
    public LoveInfo(int lv,int love)
    {
        this.lv = lv;
        this.love = love;
        this.maxLove = lv * 50;
    }
}
