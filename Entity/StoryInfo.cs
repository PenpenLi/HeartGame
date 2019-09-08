using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryInfo : MonoBehaviour {
    public string storyPath;
    public string storyName;
    public bool isLocked=true;
    public StoryInfo(string storyPath,string storyName,bool isLocked)
    {
        this.storyPath = storyPath;
        this.storyName = storyName;
        this.isLocked = isLocked;
    }
}
