using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGlobals : MonoBehaviour {
    public static string placeName="1";//加载场景名称
    public static int currentWave = 1;//现在波数
    public static int waves = 2;//总波数
    public static List<string> selectHeroes;//选择英雄列表
    public static List<string> liveHeroes = new List<string>();//存活的英雄
    public static List<string>[] monsters=new List<string>[waves];//根据关卡信息的怪物列表
    public static List<string> liveMonsters=new List<string>();//存活的敌人
    public static Transform hpoints;//英雄集
    public static Transform mpoints;//敌人集
    public static GameObject currentObj;//攻击者
    public static List<GameObject> otherObjs=new List<GameObject>();//被攻击目标列表
    public static string currentSid;//现在的技能Id
    public static bool isMagic = false;//是否是魔法
    public static bool isStop = true;//进度条停止
    public static bool isReturn = false;//返回途中
    public static bool isSelectEnemy = false;//正在选择敌人
    public static bool isNearAttack = false;//近战攻击
    public static bool isOver = false;//进入结算页面
    public static bool canEnterNext = true;//进入下一波

}
