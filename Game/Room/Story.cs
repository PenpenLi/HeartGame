using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour {
    List<GameObject> storys = new List<GameObject>();
    Transform storyGrid;
    int storyCount;
    // Use this for initialization
    void Start () {
        GameObject storyBtnPre = (GameObject)GameFuncs.GetResource("Prefabs/storyBtn");
        Button closeStoryBtn = UIManager.GetButton("closeStoryBtn");
        Transform storyList = transform.Find("storyList");
        storyGrid = storyList.Find("storyGrid");
        closeStoryBtn.onClick.AddListener(OncloseStoryBtnClick);
        //Load StoryBtns
        for (int i = 0; i < RoomGlobals.storyInfos.Count; i++)
        {
            StoryInfo si = RoomGlobals.storyInfos[i];
            string path = si.storyPath;
            string id = path.Substring(0,3);
            if (id==RoomGlobals.currentId)
            {
                storyCount++;
                GameObject story = GameObject.Instantiate(storyBtnPre);
                story.transform.SetParent(storyGrid);
                storys.Add(story);
                story.transform.Find("Text").GetComponent<Text>().text =si.storyName;
                story.GetComponent<Button>().onClick.AddListener(() => { OnstoryBtnClick(path); });
                //for i<lv,if(si.storyName[last]<=lv) interactable=true;
                story.GetComponent<Button>().interactable = !si.isLocked;
                for (int j = 0; j < RoomGlobals.loveDic[RoomGlobals.currentId].lv; j++)
                {
                    if (int.Parse(si.storyName[si.storyName.Length - 1].ToString())
                        <= RoomGlobals.loveDic[RoomGlobals.currentId].lv)
                    {
                        story.GetComponent<Button>().interactable = true;
                    }
                }
            }
            
        }
        for (int i = 0; i < storyGrid.childCount; i++)
        {
            if (i >= storyCount)
            {
                Destroy(storyGrid.GetChild(i).gameObject);
            }
        }
    }

    void OncloseStoryBtnClick()
    {
        MusicManager.PlaySe("click");
        gameObject.SetActive(false);
        RoomGlobals.menu.SetActive(true);
        Destroy(this);
    }
    void OnstoryBtnClick(string dialogPath)
    {
        MusicManager.PlaySe("click");
        Globals.currentDialog = dialogPath;
        Globals.nextScene = "Room";
        GameFuncs.GoToScene("Chat");
    }
}
