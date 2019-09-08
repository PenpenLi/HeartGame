using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnChatWindowClick : MonoBehaviour, IPointerClickHandler
{
    int i = 1;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (i < Globals.dialogs.Count-1)
        {
            UIManager.ChangeText("context","");
            Globals.logIndex = i;
            ChatFuncs.Instance.ShowDialog(i);
            i++;
        }
        else
        {
            if (Globals.lastScene == "Login") Globals.nextScene = "Main";
            if (Globals.lastScene == "Room") GameFuncs.GoToScene("Room");
            else GameFuncs.GoToSceneAsync(Globals.nextScene);
        }
    }
}
