using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFuncs : MonoBehaviour
{
    /// <summary>
    /// 直接加载指定场景
    /// </summary>
    /// <param name="name"></param>
    public static void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    /// <summary>
    /// 异步加载场景（Loading)
    /// </summary>
    /// <param name="name"></param>
    public static void GoToSceneAsync(string name)
    {
        Globals.nextScene = name;
        SceneManager.LoadScene("Loading");
    }
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Object GetResource(string path)
    {
        Object resource = Resources.Load(path);
        return resource;
    }
    /// <summary>
    /// 查找隐藏物体
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject FindHiden(string parent)
    {
        GameObject parentObj = GameObject.Find(parent);
        Regex r = new Regex("Parent");
        GameObject childObj = parentObj.transform.Find(r.Replace(parent,"")).gameObject;
        return childObj;
    }
    /// <summary>
    /// 递归查找子孙物体
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static GameObject FindChild(Transform parent,string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name == childName)
            {
                return child.gameObject;
            }
            else
            {
                FindChild(child,childName);
            }
        }
        return null;
    }
    /// <summary>
    /// 获得英雄id
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static string GetId(Transform parent)
    {
        return parent.GetChild(0).gameObject.name.Substring(0, 3);
    }
    public static Transform GetParent(string id,Transform points)
    {
        foreach (Transform point in points)
        {
            foreach (Transform child in point)
            {
                if (child.gameObject.name.Contains(id))
                {
                    return point;
                }
            }
        }
        return null;
    } 
    public static void CreateMsg(string s)
    {
        GameObject msg = Instantiate((GameObject)GetResource("Prefabs/msg"),
            GameObject.Find("Canvas").transform);
        msg.transform.Find("Text").GetComponent<Text>().text = s ;
        Destroy(msg,2f);
    }
    public static IEnumerator WaitTime(int time)
    {
        yield return new WaitForSeconds(time);
    }
    public static Entity GetEntity(string id)
    {
        foreach (Entity e in Globals.heroList)
        {
            if (e.id == id)
            {
                return e;
            }
        }
        foreach (Entity e in Globals.enemyList)
        {
            if (e.id == id)
            {
                return e;
            }
        }
        return null;
    }
        public static Hero GetHero(string id)
    {
        foreach (Hero e in Globals.heroes)
        {
            if (e.id == id)
            {
                return e;
            }
        }
        return null;
    }
    public static Skill GetSkill(string sid)
    {
        foreach (Skill s in Globals.skillList)
        {
            if (s.sid==sid)
            {
                return s;
            }
        }
        return null;
    }
    public static void CopyEntity(Entity e,string id)
    {
        Entity etemp = GetEntity(id);
        e.id = etemp.id;
        e.ename = etemp.ename;
        e.imagePath = etemp.imagePath;
        e.ele = etemp.ele;
        e.infos = etemp.infos;
        e.currentHp = etemp.infos[0];
        e.skills = etemp.skills;
    }
    public static void CopyHero(Hero h, string id)
    {
        CopyEntity(h,id);
        Hero htemp =GetHero(id);
        h.superSkill = htemp.superSkill;
        h.skills = htemp.skills;
    }
    public static AnimationClip GetAnim(Animator animator, string clip)
    {
        if (null == animator || string.IsNullOrEmpty(clip) || null == animator.runtimeAnimatorController)
            return null;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        AnimationClip[] tAnimationClips = ac.animationClips;
        if (null == tAnimationClips || tAnimationClips.Length <= 0) return null;
        AnimationClip tAnimationClip;
        for (int tCounter = 0, tLen = tAnimationClips.Length; tCounter < tLen; tCounter++)
        {
            tAnimationClip = ac.animationClips[tCounter];
            if (null != tAnimationClip && tAnimationClip.name == clip)
                return tAnimationClip;
        }
        return null;
    }
}
