using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class JsonHelper : MonoBehaviour {
    static string jsonPath = Application.dataPath + "/Resources/Jsons/";
    public static void WriteJson(string name,string context,string kind)
    {
        string fp = "";
        switch (kind)
        {
            case "player":
                fp = jsonPath + Globals.player.uname;
                break;
            case "map":
                fp = jsonPath + "Maps";
                break;
            default:
                break;
        }
        //file folder
        if (!File.Exists(fp))
        {
            Directory.CreateDirectory(fp);
        }
        //file
        string filePath = fp + "/"+ name + ".json";
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }
        StreamWriter sw = new StreamWriter(filePath,false);
        sw.Write(context);
        if (sw!=null)
        {
            sw.Close();
        }

    }
    public static JsonData ReadJson(string name,string kind)
    {
        string fp = "";
        switch (kind)
        {
            case "player":
                fp = jsonPath + Globals.player.uname;
                break;
            case "map":
                fp = jsonPath + "Maps";
                break;
            default:
                break;
        }
        string filePath = fp + "/" + name + ".json";
        JsonData jd = null;
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string context = sr.ReadToEnd();
            if (context.Contains(":"))
            {
                jd = JsonMapper.ToObject(context);
            }
            else
            {
                jd = null;
            }
        }
        return jd;
    }
}
