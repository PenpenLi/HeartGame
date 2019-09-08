using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pack : MonoBehaviour {
    GameObject items;
    GameObject itemPre;
    GameObject panel;
    List<GameObject> itemList = new List<GameObject>();
    List<GameObject> infos=new List<GameObject>();
	// Use this for initialization
	void Start () {
        itemPre = (GameObject)GameFuncs.GetResource("Prefabs/item");
        items = GameObject.Find("Items");
        panel = GameObject.Find("Panel");
        //Create Items
        for (int i = 0; i < Globals.items.Count; i++)
        {
            int j = i / 6;
            Item it = Globals.items[i];
            string iname = it.iname;
            string imagePath = "Icons/"+it.imagePath;
            int count = it.count;
            string info = it.info;
            GameObject item = GameObject.Instantiate(itemPre);
            item.transform.SetParent(items.transform);
            itemList.Add(item);
            //values
            item.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
            item.transform.Find("Text").GetComponent<Text>().text=count.ToString();
            //Set info
            GameObject infoObj = item.transform.Find("info").gameObject;
            infoObj.name = i.ToString();
            infoObj.transform.SetParent(panel.transform);
            infoObj.GetComponent<RectTransform>().position = new Vector2(
                item.GetComponent<RectTransform>().position.x+245.4f+135*i,
                item.GetComponent<RectTransform>().position.y+524-135*j);
            infos.Add(infoObj);
            //Bind Btn
            item.GetComponent<Button>().onClick.AddListener(()=> { OnitemClick(item,
                "<color=yellow>" + iname+ "</color>\n" + info,infoObj); });
        }
	}
    private void OnDestroy()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i]);
        }
        for (int i = 0; i < infos.Count; i++)
        {
            Destroy(infos[i]);
        }
    }
    void OnitemClick(GameObject item,string infoStr,GameObject info)
    {
        if (info.activeInHierarchy==false)
        {
            info.SetActive(true);
            info.transform.Find("Text").GetComponent<Text>().text = infoStr;
            for (int i = 0; i < infos.Count; i++)
            {
                if (info.name!=i.ToString())
                {
                    infos[i].SetActive(false);
                }
            }
        }
        else
        {
            info.SetActive(false);
        }
    }
    
}
