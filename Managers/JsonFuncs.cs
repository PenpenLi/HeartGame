using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using LitJson;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class JsonFuncs : MonoBehaviour
{
    static string ParseJsonData(string json)
    {
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        string targetJson = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        return targetJson;
    }
    public static List<Hero> LoadHeroes()
    {
        List<Hero> hList = new List<Hero>();
        
        JsonData jd = JsonHelper.ReadJson("hero", "player");
        if (jd == null) return hList;
        for (int i = 0; i < jd.Count; i++)
        {
            int[] infos = new int[6] { int.Parse(jd[i]["hp"].ToString()), int.Parse(jd[i]["mp"].ToString()),
                int.Parse(jd[i]["atk"].ToString()),int.Parse(jd[i]["def"].ToString()),
            int.Parse(jd[i]["ats"].ToString()),int.Parse(jd[i]["spd"].ToString()),};
            int[] exps = new int[3] { int.Parse(jd[i]["lv"].ToString()), int.Parse(jd[i]["exp"].ToString()), int.Parse(jd[i]["maxExp"].ToString()) };
            LoveInfo li = new LoveInfo(int.Parse(jd[i]["loveLv"].ToString()), int.Parse(jd[i]["loveExp"].ToString()));
            Hero h = new Hero(jd[i]["id"].ToString(), jd[i]["ename"].ToString(), jd[i]["imagePath"].ToString(),
                jd[i]["ele"].ToString(), infos, int.Parse(jd[i]["isNearAttack"].ToString()) == 1 ? true : false,
                null);
            JsonData jdd = JsonHelper.ReadJson(h.id + "d", "player");
            if (jdd!=null)
            {
                for (int j = 0; j < jdd.Count; j++)
                {
                    Dress d = new Dress(jdd[j]["did"].ToString(), jdd[j]["dname"].ToString(),
                        jdd[j]["imagePath"].ToString(), jdd[j]["modelPath"].ToString(),
                        jdd[j]["belong"].ToString());
                    h.dresses.Add(d);
                }
            }
            
            h.exps = exps;
            h.li = li;
            hList.Add(h);
        }
        return hList;
    }
    public static List<Item> LoadItems()
    {
        List<Item> iList = new List<Item>();
        JsonData jd = JsonHelper.ReadJson("item", "player");
        if (jd == null) return iList;
        for (int i = 0; i < jd.Count; i++)
        {
            Item item = new Item(jd[i]["itemId"].ToString(), 
                jd[i]["iname"].ToString(),
                jd[i]["imagePath"].ToString(),
                int.Parse(jd[i]["count"].ToString()),
                jd[i]["info"].ToString(), 
                int.Parse(jd[i]["itag"].ToString()),
                int.Parse(jd[i]["value"].ToString()));
            iList.Add(item);
        }

        return iList;
    }
    public static List<string>[] LoadEnemys(string placeName)
    {
        JsonData jd = JsonHelper.ReadJson(placeName, "map");
        if (jd == null) return null;
        List<string>[] enemys = new List<string>[jd.Count];
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i] = new List<string>();
        }
        for (int i = 0; i < jd.Count; i++)
        {
            for (int j = 0; j < jd[i].Count; j++)
            {
                enemys[i].Add(jd[i][j]["id"].ToString());
            }
        }
        return enemys;
    }
    public static Dictionary<string, string> LoadRoom()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        JsonData jd = JsonHelper.ReadJson("room", "player");
        if (jd == null) return null;
        for (int i = 0; i < jd.Count; i++)
        {
            dic.Add(jd[i]["point"].ToString(), jd[i]["model"].ToString());
        }
        return dic;
    }
    public static void StoreHeroes()
    {
        //Globals.Heroes->player.heroes(string)(json path)
        //json
        string context = "[";
        for (int i = 0; i < Globals.heroes.Count; i++)
        {
            Hero h = Globals.heroes[i];
            JsonWriter jw = new JsonWriter();
            jw.WriteObjectStart();
            jw.WritePropertyName("id");
            jw.Write(h.id);
            jw.WritePropertyName("ename");
            jw.Write(h.ename);
            jw.WritePropertyName("imagePath");
            jw.Write(h.imagePath);
            jw.WritePropertyName("ele");
            jw.Write(h.ele);
            jw.WritePropertyName("isNearAttack");
            jw.Write(h.isNearAttack ? 1 : 0);
            jw.WritePropertyName("hp");
            jw.Write(h.infos[0]);
            jw.WritePropertyName("mp");
            jw.Write(h.infos[1]);
            jw.WritePropertyName("atk");
            jw.Write(h.infos[2]);
            jw.WritePropertyName("def");
            jw.Write(h.infos[3]);
            jw.WritePropertyName("ats");
            jw.Write(h.infos[4]);
            jw.WritePropertyName("spd");
            jw.Write(h.infos[5]);
            jw.WritePropertyName("lv");
            jw.Write(h.exps[0]);
            jw.WritePropertyName("exp");
            jw.Write(h.exps[1]);
            jw.WritePropertyName("maxExp");
            jw.Write(h.exps[2]);
            jw.WritePropertyName("loveLv");
            jw.Write(h.li.lv);
            jw.WritePropertyName("loveExp");
            jw.Write(h.li.love);
            jw.WriteObjectEnd();
            context += jw.ToString();
            if(i!= Globals.heroes.Count-1) context += ",";
        }
        context=ParseJsonData(context)+"]";
        JsonHelper.WriteJson("hero", context, "player");
    }
    public static void StoreSkills()
    {
        //for (int j = 0; j < h.skills.Count; j++)
        //{
        //    Skill s = h.skills[j];
        //    jw.WriteObjectStart();
        //    jw.WritePropertyName("sid");
        //    jw.Write(s.sid);
        //    jw.WritePropertyName("sname");
        //    jw.Write(s.sname);
        //    jw.WritePropertyName("ele");
        //    jw.Write(s.ele);
        //    jw.WritePropertyName("info");
        //    jw.Write(s.info);
        //    jw.WritePropertyName("cost");
        //    jw.Write(s.cost);
        //    jw.WritePropertyName("isAoe");
        //    jw.Write(s.isAoe ? 1 : 0);
        //    jw.WritePropertyName("isCp");
        //    jw.Write(s.isCp ? 1 : 0);
        //    jw.WritePropertyName("lv");
        //    jw.Write(s.lv);
        //    jw.WriteObjectEnd();
        //}
    }
    public static void StoreDresses()
    {
        for (int i = 0; i < Globals.heroes.Count; i++)
        {
            Hero h = Globals.heroes[i];
            string context = "[";
            for (int j = 0; j < h.dresses.Count; j++)
            {
                Dress d = h.dresses[j];
                JsonWriter jw = new JsonWriter();
                jw.WriteObjectStart();
                jw.WritePropertyName("did");
                jw.Write(d.did);
                jw.WritePropertyName("dname");
                jw.Write(d.dname);
                jw.WritePropertyName("imagePath");
                jw.Write(d.imagePath);
                jw.WritePropertyName("modelPath");
                jw.Write(d.modelPath);
                jw.WritePropertyName("belong");
                jw.Write(d.belong);
                jw.WriteObjectEnd();
                context += jw.ToString();
                if (j != h.dresses.Count - 1) context += ",";
            }
            context = ParseJsonData(context)+"]";
            JsonHelper.WriteJson(h.id+"d", context, "player");
        }
    }
    public static void StoreItems()
    {
        //Globals.Items->player.items(string)(json path)
        string context = "[";
        for (int i = 0; i < Globals.items.Count; i++)
        {
            Item item = Globals.items[i];
            JsonWriter jw = new JsonWriter();
            jw.WriteObjectStart();
            jw.WritePropertyName("itemId");
            jw.Write(item.itemId);
            jw.WritePropertyName("iname");
            jw.Write(item.iname);
            jw.WritePropertyName("imagePath");
            jw.Write(item.imagePath);
            jw.WritePropertyName("count");
            jw.Write(item.count);
            jw.WritePropertyName("info");
            jw.Write(item.info);
            jw.WritePropertyName("itag");
            jw.Write(item.itag);
            jw.WritePropertyName("value");
            jw.Write(item.value);
            jw.WriteObjectEnd();
            context += jw.ToString();
            if (i != Globals.items.Count - 1) context += ",";
        }
        context = ParseJsonData(context)+"]";
        JsonHelper.WriteJson("item", context, "player");
    }
    public static void StoreRoom()
    {
        string context = "[";
        List<string> keys = new List<string>(RoomGlobals.roomInfos.Keys);
        for (int i = 0; i < RoomGlobals.roomInfos.Count; i++)
        {
            string key = keys[i];
            string value = RoomGlobals.roomInfos[key];
            JsonWriter jw = new JsonWriter();
            jw.WriteObjectStart();
            jw.WritePropertyName("point");
            jw.Write(key);
            jw.WritePropertyName("model");
            jw.Write(value);
            jw.WriteObjectEnd();
            context += jw.ToString();
            if (i != RoomGlobals.roomInfos.Count - 1) context += ",";
        }
        context = ParseJsonData(context) + "]";
        JsonHelper.WriteJson("room", context, "player");
    }
}
