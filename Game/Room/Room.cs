using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    GameObject selectCharacter;
    void Start()
    {
        Globals.lastScene = "Room";
        RoomFuncs.LoadStoryDic();
        if (GameObject.Find("point1Btn"))
        {
            Button point1Btn = UIManager.GetButton("point1Btn");
            RoomGlobals.point1Btn = point1Btn.gameObject;
            point1Btn.onClick.AddListener(() => { OnpointBtn(point1Btn.gameObject.name); });
        }
        if (GameObject.Find("point1Btn"))
        {
            Button point2Btn = UIManager.GetButton("point2Btn");
            RoomGlobals.point2Btn = point2Btn.gameObject;
            point2Btn.onClick.AddListener(() => { OnpointBtn(point2Btn.gameObject.name); });
        }
        RoomGlobals.backBtn = GameObject.Find("backBtn");
        GameObject point1 = GameObject.Find("point1");
        RoomGlobals.point1 = point1;
        GameObject point2 = GameObject.Find("point2");
        RoomGlobals.point2 = point2;
        selectCharacter = GameFuncs.FindHiden("selectCharacterParent");
        //Load RoomImfos
        if (RoomGlobals.roomInfos.Count>0)
        {
            foreach (var info in RoomGlobals.roomInfos)
            {
                Transform point = GameObject.Find(info.Key).transform;
                RoomFuncs.CreateCharacter(point, info.Value);
                string id = info.Value.Replace("Prefabs/", "");
                if (!RoomGlobals.loveDic.ContainsKey(id))
                {
                    Hero h = GameFuncs.GetHero(id);
                    RoomGlobals.loveDic.Add(id, h.li);
                }
            }
        }
        //如果有人则隐藏添加图标
        if (point1.transform.childCount>0)
        {
            RoomGlobals.point1Btn.SetActive(false);
        }
        if (point2.transform.childCount > 0)
        {
            RoomGlobals.point2Btn.SetActive(false);
        }
        MusicManager.PlayBgm("room");
    }

    void OnpointBtn(string btnName)
    {
        MusicManager.PlaySe("click");
        GameObject point = GameObject.Find(btnName.Replace("Btn", ""));
        if (point.transform.childCount == 0)//Add Chracter(Select UI->Instantiate)
        {
            if (!selectCharacter.activeInHierarchy)
            {
                selectCharacter.SetActive(true);
                //Create Characters
                Transform grid = selectCharacter.transform.Find("grid");
                for (int i = 0; i < Globals.heroes.Count; i++)
                {
                    if (!RoomFuncs.FindModel(Globals.heroes[i].id))
                    {
                        GameObject character_roomSelect = GameObject.Instantiate(
                       (GameObject)GameFuncs.GetResource("Prefabs/character_roomSelect"), grid);
                        string modelPath = "Prefabs/" + Globals.heroes[i].id;
                        string imagePath = "Character/Portrait/" + Globals.heroes[i].id;
                        Image image = character_roomSelect.transform.Find("Image").GetComponent<Image>();
                        image.sprite = Resources.Load<Sprite>(imagePath);

                        Button character_roomSelectBtn = character_roomSelect.GetComponent<Button>();
                        character_roomSelectBtn.onClick.AddListener(() =>
                        { Oncharacter_roomSelectBtnClick(point.transform, modelPath, btnName); });
                    }
                }
            }
            else
            {
                Transform grid = selectCharacter.transform.Find("grid");
                foreach (Transform child in grid)
                {
                    Destroy(child.gameObject);
                }
                selectCharacter.SetActive(false);
            }
        }
    }
    void Oncharacter_roomSelectBtnClick(Transform point, string modelPath, string btnName)
    {
        MusicManager.PlaySe("click");
        RoomFuncs.CreateCharacter(point, modelPath);
        RoomGlobals.currentId = modelPath.Replace("Prefabs/","");
        if (!RoomGlobals.roomInfos.ContainsKey(point.gameObject.name))
        {
            RoomGlobals.roomInfos.Add(point.gameObject.name, modelPath);
        }
        if (!RoomGlobals.loveDic.ContainsKey(RoomGlobals.currentId))
        {
            Hero h = GameFuncs.GetHero(RoomGlobals.currentId);
            RoomGlobals.loveDic.Add(RoomGlobals.currentId,h.li);
        }
        GameObject.Find(btnName).SetActive(false);
    }

}
