using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomGlobals : MonoBehaviour {
    public static GameObject point1Btn;
    public static GameObject point2Btn;
    public static GameObject backBtn;
    public static GameObject point1;
    public static GameObject point2;
    public static Dictionary<string, string> roomInfos = new Dictionary<string, string>();
    public static GameObject menu;
    public static string currentId;
    public static Dictionary<string, LoveInfo> loveDic = new Dictionary<string, LoveInfo>();
    public static int maxLove=100;
    public static List<StoryInfo> storyInfos = new List<StoryInfo>();
    public static bool isDestroy = true;
}
