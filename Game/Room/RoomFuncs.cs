using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomFuncs : MonoBehaviour {
    public static bool FindModel(string id)
    {
        GameObject points = GameObject.Find("Points");
        foreach (Transform point in points.transform)
        {
            foreach (Transform model in point)
            {
                if (model.gameObject.name.Contains(id))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static void DestroyAll()
    {
        GameObject points = GameObject.Find("Points");
        foreach (Transform point in points.transform)
        {
            foreach (Transform model in point)
            {
                Destroy(model.gameObject);
            }
        }
    }
    public static void CreateCharacter(Transform point,string modelPath)
    {
        GameObject selectCharacter = GameFuncs.FindHiden("selectCharacterParent");
        //Instantiate Model
        GameObject pre = (GameObject)GameFuncs.GetResource(modelPath);
        GameObject obj = GameObject.Instantiate(pre);
        obj.transform.position = point.transform.position;
        //MusicManager.PlaySe("click");
        if (point.name == "point1")
        {
            obj.transform.SetParent(point.transform);
        }
        else if (point.name == "point2")
        {
            obj.transform.Rotate(Vector3.up * 90);
            obj.transform.SetParent(point.transform);
        }
        obj.AddComponent<OnModel>();
        //Destroy selectCharacter
        Transform grid = selectCharacter.transform.Find("grid");
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
        //Active False
        selectCharacter.SetActive(false);
    }
    public static void ChangeLove()
    {
        Slider loveSlider = UIManager.GetSlider("loveSlider");
        loveSlider.value = (float)RoomGlobals.loveDic[RoomGlobals.currentId].love / RoomGlobals.loveDic[RoomGlobals.currentId].maxLove;
        UIManager.ChangeText("loveText", RoomGlobals.loveDic[RoomGlobals.currentId].love.ToString() + "/" + RoomGlobals.loveDic[RoomGlobals.currentId].maxLove.ToString());
        UIManager.ChangeText("lvText", RoomGlobals.loveDic[RoomGlobals.currentId].lv.ToString());
    }
    public static void LoadStoryDic()
    {
        RoomGlobals.storyInfos.Clear();
        for (int i = 0; i < Globals.heroes.Count; i++)
        {
            string id = Globals.heroes[i].id;
            foreach (var dialog in Resources.LoadAll("Dialogs"))
            {
                string path = dialog.name;
                if (path.Contains(id) && path.Contains("story"))
                {
                    RoomGlobals.storyInfos.Add(new StoryInfo(path, "羁绊" + path[path.Length - 1],true));

                }
            }
        }
    }
}
