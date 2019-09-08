using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using System;

public class DBFuncs : MonoBehaviour
{

    #region 初始化Globals.List
    /// <summary>
    /// 初始化Globals.skillList
    /// </summary>
    public static void InitSkillList()
    {
        SqlDataReader dr = DBHelper.Select("select * from SkillList");
        while (dr.Read())
        {
            Skill s = new Skill((string)dr["sid"], (string)dr["sname"], (string)dr["ele"], (string)dr["info"],
                (int)dr["cost"], (int)dr["isAoe"] == 1 ? true : false, (int)dr["isCp"] == 1 ? true : false,
                (int)dr["lv"]);
            Globals.skillList.Add(s);
        }
        DBHelper.Closedr(dr);
    }
    /// <summary>
    /// 初始化Globals.itemList
    /// </summary>
    public static void InitItemList()
    {
        SqlDataReader dr = DBHelper.Select("select * from ItemList");
        while (dr.Read())
        {
            Item t = new Item((string)dr["itemId"], (string)dr["iname"], (string)dr["imagePath"],
                (int)dr["count"], (string)dr["info"], (int)dr["itag"], (int)dr["value"]);
            Globals.itemList.Add(t);
        }
        DBHelper.Closedr(dr);
    }
    /// <summary>
    /// 初始化Globals.heroList
    /// </summary>
    public static void InitHeroList()
    {
        SqlDataReader dr = DBHelper.Select("select * from HeroList");
        while (dr.Read())
        {
            int[] tempinfos = new int[6] { (int)dr["hp"], (int)dr["mp"], (int)dr["atk"], (int)dr["def"], (int)dr["ats"], (int)dr["spd"], };
            Skill ss = GameFuncs.GetSkill((string)dr["superSkill"]);
            Hero h = new Hero((string)dr["id"], (string)dr["ename"], (string)dr["imagePath"], (string)dr["ele"],
                tempinfos, (int)dr["isNearAttack"] == 1 ? true : false, ss);
            Globals.heroList.Add(h);
        }
        DBHelper.Closedr(dr);
    }
    /// <summary>
    /// 初始化Globals.enemyList
    /// </summary>
    public static void InitEnemyList()
    {
        SqlDataReader dr = DBHelper.Select("select * from EnemyList");
        while (dr.Read())
        {
            int[] tempinfos = new int[6] { (int)dr["hp"], (int)dr["mp"], (int)dr["atk"], (int)dr["def"], (int)dr["ats"], (int)dr["spd"], };
            Enemy e = new Enemy((string)dr["id"], (string)dr["ename"], (string)dr["imagePath"], (string)dr["ele"],
                tempinfos, (int)dr["isNearAttack"] == 1 ? true : false);
            //load Skills
            List<Skill> tempSkill = new List<Skill>();//json
            e.skills = tempSkill;
            Globals.enemyList.Add(e);
        }
        DBHelper.Closedr(dr);
    }
    /// <summary>
    /// 初始化Globals.dressList
    /// </summary>
    public static void InitDressList()
    {
        SqlDataReader dr = DBHelper.Select("select * from DressList");
        while (dr.Read())
        {
            Dress d = new Dress((string)dr["did"], (string)dr["dname"],
                (string)dr["imagePath"], (string)dr["modelPath"], (string)dr["belong"]);
            Globals.dressList.Add(d);
        }
        DBHelper.Closedr(dr);
    }
    #endregion
    public static void InitAllLists()
    {
        InitSkillList();
        InitItemList();
        InitHeroList();
        InitEnemyList();
        InitDressList();
    }
    /// <summary>
    /// 根据用户名和密码初始化玩家，用于登录
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pwd"></param>
    /// <returns></returns>
    public static bool FindPlayer(string uname, string pwd)
    {
        string s = string.Format("select * from Players where uname='{0}' and pwd='{1}'", uname, pwd);
        if (DBHelper.Selectds(s).Tables[0].Rows.Count > 0)
        {
            DataSet ds = DBHelper.Selectds(s);
            var dr = ds.Tables[0].Rows[0];
            Globals.player = new Player((string)dr["uname"], (string)dr["pwd"], (string)dr["nickname"], (string)dr["sex"],
           (int)dr["energy"], (int)dr["maxEnergy"], (int)dr["exp"], (int)dr["maxExp"],
           (string)dr["characterId"], (string)dr["headImagePath"], (int)dr["gold"], (int)dr["dia"],
           (int)dr["isFirst"] == 1 ? true : false);

            //heroes items(string->json->list)
            List<Hero> hs = JsonFuncs.LoadHeroes();
            List<Item> its = JsonFuncs.LoadItems();
            Globals.heroes = hs;
            Globals.items = its;;
            Globals.bgmVolume = float.Parse(dr["bgmVolume"].ToString());
            Globals.seVolume = float.Parse(dr["seVolume"].ToString());
            Globals.voiceVolume = float.Parse(dr["voiceVolume"].ToString());
            if (JsonFuncs.LoadRoom()!=null)
            {
                RoomGlobals.roomInfos = JsonFuncs.LoadRoom();
            }
            for (int i = 0; i < Globals.heroes.Count; i++)
            {
                foreach (var skill in Globals.skillList)
                {
                    if (skill.ele == Globals.heroes[i].ele)
                    {
                        if (!skill.isCp && skill.lv <= Globals.heroes[i].exps[0])
                        {
                            Globals.heroes[i].skills.Add(skill);
                        }
                        else if (skill.isCp)
                        {
                            if (skill.sid.Contains(Globals.heroes[i].id))
                                Globals.heroes[i].skills.Add(skill);
                        }
                    }
                }
                foreach (var dress in Globals.dressList)
                {
                    if (dress.belong == Globals.heroes[i].id && dress.dname == "初始")
                        Globals.heroes[i].dresses.Add(dress);
                }
            }
            return true;
        }
        else return false;
    }
    /// <summary>
    /// 添加玩家的用户名和密码，用于注册
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pwd"></param>
    public static void AddPlayer(string name, string pwd)
    {
        //dbhelper.connect->insert into Players
        string s = "insert into Players(uname,pwd) values('" + name + "','" + pwd + "')";
        DBHelper.Insert(s);
    }
    /// <summary>
    /// 退出时更新玩家信息
    /// </summary>
    public static void UpdatePlayer()
    {
        //globals.heroes and items to jsonPath
        string heroesPath = "";
        string itemsPath = "";
        //dbhelper.change
        if (Globals.player!=null)
        {
            string sql = string.Format("update Players set energy={0},nickname='{1}',sex='{2}',isFirst={3},exp={4}," +
           "headImagePath='{5}',gold={6},dia={7},maxEnergy={8},maxExp={9},characterId='{10}'," +
           "heroes='{11}',items='{12}',bgmVolume={13},seVolume={14},voiceVolume={15} where uname='{16}'", Globals.player.energy, Globals.player.nickname, Globals.player.sex,
           Globals.player.isFirst ? 1 : 0, Globals.player.exp, Globals.player.headImagePath, Globals.player.gold,
           Globals.player.dia, Globals.player.maxEnergy, Globals.player.maxExp, Globals.player.characterId,
           heroesPath, itemsPath,Globals.bgmVolume,Globals.seVolume,Globals.voiceVolume,Globals.player.uname);
            DBHelper.Change(sql);
        }
    }
}
