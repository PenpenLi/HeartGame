using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {
    //DB
    //SkillList
    public static List<Skill> skillList = new List<Skill>();//所有普通技能
    public static List<Skill> superSkills = new List<Skill>();//从SkillList中取
    //所有特技
    //ItemList
    public static List<Item> itemList = new List<Item>();//所有物品表
    //HeroList
    public static List<Hero> heroList = new List<Hero>();//所有英雄表
    //EnemyList
    public static List<Enemy> enemyList = new List<Enemy>();//所有敌人表
    //DressList
    public static List<Dress> dressList = new List<Dress>();//所有服装表
    //Player
    public static Player player;//当前用户
    public static List<Item> items = new List<Item>();//持有物品列表
    public static List<Hero> heroes = new List<Hero>();//持有英雄列表
    //Music
    public static float bgmVolume = 0.8f;//全局背景音量
    public static float seVolume = 0.8f;//全局音效音量
    public static float voiceVolume = 0.8f;//全局语音音量
    //Game
    //Chat
    public static string currentDialog = "test";//现在的Dialog.txt
    public static List<Dialog> dialogs;//一段对话(1个txt)
    public static int logIndex;//当前进行到的Dialog
    public static string currentBg = "bg1";//Chat当前背景，进入Chat时更改
    //Scene
    public static string lastScene = "Login";//上个场景
    public static string nextScene = "Main";//跳转的场景

}
