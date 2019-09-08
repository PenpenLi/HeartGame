using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item : MonoBehaviour {
    public string itemId;
    public string iname;
    public string imagePath;
    public int count;
    public string info;
    public int itag;//普通物品还是礼物(0/1)
    public int value;//效果
    public Item(string itemId, string iname, string imagePath, int count,string info,
        int itag,int value)
    {
        this.itemId = itemId;
        this.iname = iname;
        this.imagePath = imagePath;
        this.count = count;
        this.info = info;
        this.itag = itag;
        this.value = value;
    }
}
