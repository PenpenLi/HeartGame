using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    string nameStr;
    string pwdStr;
    // Use this for initialization
    private void Awake()
    {
        GameObject bgm = GameObject.Find("bgm");
        GameObject se = GameObject.Find("se");
        GameObject voice = GameObject.Find("voice");
        GameObject quit = GameObject.Find("quit");
        DontDestroyOnLoad(bgm);
        DontDestroyOnLoad(se);
        DontDestroyOnLoad(voice);
        DontDestroyOnLoad(quit);
    }
    void Start()
    {
        DBFuncs.InitAllLists();
        Globals.lastScene = "Login";
        Button loginBtn = UIManager.GetButton("loginBtn");
        loginBtn.onClick.AddListener(OnloginBtnClick);
        Button regBtn = UIManager.GetButton("regBtn");
        regBtn.onClick.AddListener(OnregBtnClick);

        //PlayBGM
        MusicManager.PlayBgm("normal");
    }
    void OnloginBtnClick()
    {
        nameStr = UIManager.GetInputStr("nameInput");
        pwdStr = UIManager.GetInputStr("pwdInput");
        //PlaySE
        MusicManager.PlaySe("click");;
        //Formal
        //Globals.isFirst = DBFuncs.FindSingle<int>("Players", "isFirst");
        if (nameStr == "" || pwdStr == "")
        {
            GameFuncs.CreateMsg("用户名或密码不能为空！");
            return;
        }
        else
        {
            if (DBFuncs.FindPlayer(nameStr, pwdStr))
            {
                //Enter Loading->Chat/Main
                if (Globals.player.isFirst)
                {
                    Globals.player.isFirst = false;
                    //Test
                    Globals.currentDialog = "test";

                    Globals.nextScene = "Main";
                    GameFuncs.GoToSceneAsync("Chat");
                }
                else
                {
                    //Load Globals.items&Globals.heroes
                    GameFuncs.GoToSceneAsync("Main");
                }
            }
            else
            {
                //弹出对话框"用户名或密码错误！"
                GameFuncs.CreateMsg("用户名或密码错误！");
            }
        }
    }
    void OnregBtnClick()
    {
        nameStr = UIManager.GetInputStr("nameInput");
        pwdStr = UIManager.GetInputStr("pwdInput");
        //PlaySE
        MusicManager.PlaySe("click");
        if (nameStr == "" || pwdStr == "")
        {
            GameFuncs.CreateMsg("用户名或密码不能为空！");
            return;
        }
        else
        {
            if (DBFuncs.FindPlayer(nameStr, pwdStr))
            {
                //弹出对话框"用户名已存在！"
                GameFuncs.CreateMsg("用户名已存在！");
            }
            else
            {
                //Register
                DBFuncs.AddPlayer(nameStr, pwdStr);
                GameFuncs.CreateMsg("注册成功！请点击登录进入游戏");
            }
        }
    }
}
