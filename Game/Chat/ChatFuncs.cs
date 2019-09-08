using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatFuncs : MonoBehaviour {
    public static ChatFuncs Instance;
    private void Awake()
    {
        Instance = this;
    }
    public static void LoadDialogs(string path)
    {
        Globals.dialogs = new List<Dialog>();
        Globals.dialogs.Clear();
        string context = GameFuncs.GetResource("Dialogs/"+path).ToString();
        string[] dialogs = context.Split('#');
        for (int i = 0; i < dialogs.Length; i++)
        {
            string d = dialogs[i];
            Dialog dialog = new Dialog(d);
            Globals.dialogs.Add(dialog);
        }
    }
    public void ShowDialog(int i)
    {
        for (int j = 0; j < Globals.dialogs[i].context.Length; j++)
        {
            if (Globals.dialogs[i].context.Contains("nickname"))
            {
                Globals.dialogs[i].context = Globals.dialogs[i].context.Replace("nickname",Globals.player.nickname);
            }
        }
        string name = Globals.dialogs[i].dname;
        string image = Globals.dialogs[i].image;
        Sprite s = Resources.Load<Sprite>("Character/Image/" + image);
        string context = Globals.dialogs[i].context;
        string bg = Globals.dialogs[i].bg;
        UIManager.ChangeText("name", name);
        UIManager.ChangeImage("character", "Character/Image/" + image);
        UIManager.ChangeImage("BG","Background/"+bg);
        StartCoroutine(DelayShowContext(context));
    }
    IEnumerator DelayShowContext(string context)
    {
        for (int i = 0; i < context.Length; i++)
        {
            Text con = UIManager.GetText("context");
            con.text += context[i];
            yield return new WaitForSeconds(0.03f);
        }
    }
}
