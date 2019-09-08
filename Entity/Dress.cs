using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Dress{
    public string did;
    public string dname;
    public string imagePath;
    public string modelPath;
    public string belong;
    public Dress(string did,string dname,string imagePath,string modelPath,string belong)
    {
        this.did = did;
        this.dname = dname;
        this.imagePath = imagePath;
        this.modelPath = modelPath;
        this.belong = belong;
    }
}
