using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{
    public string uname;//primary key
    public string pwd;
    public string nickname;
    public string sex;
    public int energy;
    public int maxEnergy;
    public int exp;
    public int maxExp;
    public string characterId;
    public string headImagePath;
    public int gold;
    public int dia;
    public bool isFirst;
    public List<Heroes> heroes=new List<Heroes>();
    public List<Item> items=new List<Item>();

    public Player() { }
    public Player(string uname,string pwd,string nickname,string sex,
        int energy,int maxEnergy,int exp,int maxExp,string characterId,string headImagePath,
        int gold,int dia,bool isFirst)
    {
        this.uname = uname;
        this.pwd = pwd;
        this.nickname = nickname;
        this.sex = sex;
        this.energy = energy;
        this.maxEnergy = maxEnergy;
        this.exp = exp;
        this.maxExp = exp;
        this.characterId = characterId;
        this.headImagePath = headImagePath;
        this.gold = gold;
        this.dia = dia;
        this.isFirst = isFirst;
    }
}
