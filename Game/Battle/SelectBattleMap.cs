using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBattleMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //ui
        UIManager.ChangeText("energyText", Globals.player.energy.ToString() + "/" + Globals.player.maxEnergy);
        UIManager.ChangeSlider("energySlider",(float)Globals.player.energy /Globals.player.maxEnergy);
        //get place
        Transform grid = GameObject.Find("grid").transform;
        foreach (Transform place in grid)
        {
            place.GetComponent<Button>().onClick.AddListener(()=>
            {
                BattleGlobals.placeName = place.gameObject.name[place.gameObject.name.Length - 1].ToString();
                GameFuncs.GoToScene("SelectHero");
            });
        }
    }
	
}
