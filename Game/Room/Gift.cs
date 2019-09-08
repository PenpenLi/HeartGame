using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    List<GameObject> giftsList = new List<GameObject>();
    Transform giftGrid;
    int giftCount;
    // Use this for initialization
    void Start()
    {
        Button closeGiftBtn = UIManager.GetButton("closeGiftBtn");
        Transform giftList = transform.Find("giftList");
        giftGrid = giftList.Find("giftGrid");
        Button giftBtn = UIManager.GetButton("giftBtn");
        closeGiftBtn.onClick.AddListener(OncloseGiftBtnClick);
        giftBtn.onClick.AddListener(OngiftBtn);
        //Load Gifts
        GameObject giftPre = (GameObject)GameFuncs.GetResource("Prefabs/gift");
        for (int i = 0; i < Globals.items.Count; i++)
        {
            int j = i / 3;
            Item it = Globals.items[i];
            if (it.itag == 1)//is gift
            {
                giftCount++;
                string imagePath = "Icons/" + it.imagePath;
                int count = it.count;
                GameObject item = GameObject.Instantiate(giftPre);
                item.transform.SetParent(giftGrid.transform);
                giftsList.Add(item);
                //values
                item.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
                item.transform.Find("Text").GetComponent<Text>().text = count.ToString();
                item.GetComponent<Toggle>().group = giftGrid.GetComponent<ToggleGroup>();
            }
        }        
        for (int i = 0; i < giftGrid.childCount; i++)
        {
            if (i >= giftCount)
            {
                Destroy(giftGrid.GetChild(i).gameObject);
            }
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < giftsList.Count; i++)
        {
            Destroy(giftsList[i]);
        }
    }
    void OncloseGiftBtnClick()
    {
        MusicManager.PlaySe("click");
        gameObject.SetActive(false);
        RoomGlobals.menu.SetActive(true);
        Destroy(this);
    }
    void OngiftBtn()
    {
        MusicManager.PlaySe("click");
        IEnumerable<Toggle> gifts = giftGrid.GetComponent<ToggleGroup>().ActiveToggles();
        foreach (var gift in gifts)
        {
            if (gift.isOn)
            {
                string giftId = gift.transform.Find("Image").GetComponent<Image>().sprite.name;
                int count = int.Parse(gift.transform.Find("Text").GetComponent<Text>().text);
                if (count >= 1)
                {
                    count--;
                    gift.transform.Find("Text").GetComponent<Text>().text = count.ToString();
                    for (int i = 0; i < Globals.items.Count; i++)
                    {
                        if (Globals.items[i].itemId == giftId)
                        {
                            Globals.items[i].count = count;
                            RoomGlobals.loveDic[RoomGlobals.currentId].love += Globals.items[i].value;
                            if (RoomGlobals.loveDic[RoomGlobals.currentId].love>=RoomGlobals.loveDic[RoomGlobals.currentId].maxLove)
                            {
                                RoomGlobals.loveDic[RoomGlobals.currentId].lv++;
                                RoomGlobals.loveDic[RoomGlobals.currentId].love -=RoomGlobals.loveDic[RoomGlobals.currentId].maxLove;
                            }
                            break;
                        }
                    }
                    RoomFuncs.ChangeLove();
                }

            }
        }
    }
}
