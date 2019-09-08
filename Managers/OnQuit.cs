using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnQuit : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        //update db
        //update player
        if (Globals.player!=null)
        {
            DBFuncs.UpdatePlayer();
            JsonFuncs.StoreHeroes();
            JsonFuncs.StoreDresses();
            JsonFuncs.StoreItems();
            JsonFuncs.StoreRoom();
        }
    }
}
