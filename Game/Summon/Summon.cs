using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summon : MonoBehaviour {
    GameObject summonGetPre;
    GameObject gets;
    GameObject summonGets;
    Button oneBtn;
    Button tenBtn;
    // Use this for initialization
    void Start () {
        summonGetPre = (GameObject)GameFuncs.GetResource("Prefabs/summonGet");
        gets = GameFuncs.FindHiden("GetsParent");

        oneBtn = UIManager.GetButton("oneBtn");
        tenBtn = UIManager.GetButton("tenBtn");
        oneBtn.onClick.AddListener(OnoneBtnClick);
        tenBtn.onClick.AddListener(OntenBtnClick);

        UIManager.ChangeText("goldText",Globals.player.gold.ToString());
        UIManager.ChangeText("diaText", Globals.player.dia.ToString());
    }
    void OneRandom()
    {
        Globals.player.dia -= 100;
        int ran1 = Random.Range(0,10);
        if (ran1 <= 3)
        {
            bool isGetHero=false;
            //character
            Hero hero = Globals.heroList[Random.Range(0,Globals.heroList.Count)];
            GameObject summonGet=GameObject.Instantiate(summonGetPre,summonGets.transform);
            string imagePath;
            for (int i = 0; i < Globals.heroes.Count; i++)
            {
                if (Globals.heroes[i].id==hero.id)
                {
                    isGetHero = true;
                    break;
                }
            }
            if (!isGetHero)
            {
                summonGet.transform.Find("Text").gameObject.SetActive(true);
                Globals.heroes.Add(hero);
                imagePath = "Character/Portrait/" + hero.imagePath;
            }
            else
            {
                bool isGetItem = false;
                imagePath = "Icons/003";
                for (int i = 0; i < Globals.items.Count; i++)
                {
                    if (Globals.items[i].itemId == "003")
                    {
                        isGetItem = true;
                        Globals.items[i].count++;
                        break;
                    }
                }
                if (!isGetItem)
                {
                    Globals.items.Add(Globals.itemList[2]);
                }
            }
            summonGet.transform.Find("Image").GetComponent<Image>().sprite=Resources.Load<Sprite>(imagePath);
        }
        else
        {
            //item
            Item item = Globals.itemList[Random.Range(0,Globals.itemList.Count)];
            bool isGetItem = false;
            for (int i = 0; i < Globals.items.Count; i++)
            {
                if (Globals.items[i].itemId == item.itemId)
                {
                    isGetItem = true;
                    Globals.items[i].count++;
                    break;
                }
            }
            if (!isGetItem)
            {
                Globals.items.Add(item);
            }
            GameObject summonGet = GameObject.Instantiate(summonGetPre,summonGets.transform);
            string imagePath = "Icons/"+item.imagePath;
            summonGet.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
        }
    }
    void OnoneBtnClick()
    {
        if (Globals.player.dia <100)
        {
            //弹出对话框“钻石不足！”
            return;
        }
        MusicManager.PlaySe("click");
        gets.SetActive(true);
        summonGets = GameObject.Find("summonGets");
        Button closeGetBtn = UIManager.GetButton("closeGetBtn");
        closeGetBtn.onClick.AddListener(OncloseGetBtn);
        oneBtn.gameObject.SetActive(false);
        tenBtn.gameObject.SetActive(false);
        OneRandom();
        UIManager.ChangeText("goldText", Globals.player.gold.ToString());
        UIManager.ChangeText("diaText", Globals.player.dia.ToString());
    }
    void OntenBtnClick()
    {
        if (Globals.player.dia < 1000)
        {
            //弹出对话框“钻石不足！”
            return;
        }
        MusicManager.PlaySe("click");
        gets.SetActive(true);
        summonGets = GameObject.Find("summonGets");
        Button closeGetBtn = UIManager.GetButton("closeGetBtn");
        closeGetBtn.onClick.AddListener(OncloseGetBtn);
        oneBtn.gameObject.SetActive(false);
        tenBtn.gameObject.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            OneRandom();
        }
        UIManager.ChangeText("goldText", Globals.player.gold.ToString());
        UIManager.ChangeText("diaText", Globals.player.dia.ToString());
    }
    void OncloseGetBtn()
    {
        MusicManager.PlaySe("click");
        foreach (Transform child in summonGets.transform)
        {
            Destroy(child.gameObject);
        }
        gets.SetActive(false);
        oneBtn.gameObject.SetActive(true);
        tenBtn.gameObject.SetActive(true);
    }
}
