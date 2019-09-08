using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDateMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Transform placeBtns = GameObject.Find("placeBtns").transform;
        foreach (Transform placeBtn in placeBtns)
        {
            placeBtn.GetComponent<Button>().onClick.AddListener(
                ()=> { OnplaceBtnClick(placeBtn.gameObject.name); });
        }
        Button backBtn = UIManager.GetButton("backBtn");
        backBtn.onClick.AddListener(OnbackBtnClick);
	}

    void OnplaceBtnClick(string index)
    {
        MusicManager.PlaySe("click");
        //Change Love
        RoomGlobals.loveDic[RoomGlobals.currentId].love += int.Parse(index)*10;
        if (RoomGlobals.loveDic[RoomGlobals.currentId].love >= RoomGlobals.loveDic[RoomGlobals.currentId].maxLove)
        {
            RoomGlobals.loveDic[RoomGlobals.currentId].lv++;
            RoomGlobals.loveDic[RoomGlobals.currentId].love -= RoomGlobals.loveDic[RoomGlobals.currentId].maxLove;
        }
        //Load Dialogs
        Globals.currentDialog = RoomGlobals.currentId + "_date" + index;
        //Globals.lastScene = "Room";
        GameFuncs.GoToScene("Chat");
    }
    void OnbackBtnClick()
    {
        GameFuncs.GoToScene("Room");
    }
}
