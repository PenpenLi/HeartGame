using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnSmallChatWindowClick : MonoBehaviour, IPointerClickHandler
{
    int i = 0;
    public void OnPointerClick(PointerEventData eventData)
    {
        i++;
        if (i < Globals.dialogs.Count)
        {
            ShowSmallDialog(i);
        }
        else
        {
            i = 0;
            gameObject.SetActive(false);
        }
    }
    public static void ShowSmallDialog(int i)
    {
        string image = Globals.dialogs[i].image;
        string context = Globals.dialogs[i].context;
        UIManager.ChangeImage("character", "Character/Image/" + image);
        UIManager.ChangeText("context", context);
    }
}
