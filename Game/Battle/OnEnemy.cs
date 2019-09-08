using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class OnEnemy : MonoBehaviour {
    Transform canvas;
    GameObject arrow;
    private void Start()
    {
        canvas = transform.Find("Canvas");
        arrow = canvas.Find("arrow").gameObject;
    }
    private void Update()
    {
        if (BattleGlobals.isSelectEnemy)
        {
            if (Input.GetMouseButtonDown(1))
            {
                BattleGlobals.isSelectEnemy = false;
                BattleCamera.SetIsStop();
                //active hero ui
                BattleUI.heroPanel.SetActive(true);
            }
        }
    }
    private void OnMouseEnter()
    {
        if (BattleGlobals.isSelectEnemy)
        {
            if (!arrow.activeInHierarchy)
            {
                arrow.SetActive(true);
            }
        }
    }
    private void OnMouseExit()
    {
        if (BattleGlobals.isSelectEnemy)
        {
            if (arrow.activeInHierarchy)
            {
                arrow.SetActive(false);
            }
        }
    }
    private void OnMouseDown()
    {
        if (BattleGlobals.isSelectEnemy)
        {
            BattleGlobals.otherObjs.Clear();
            BattleGlobals.otherObjs.Add(gameObject);
            BattleGlobals.isSelectEnemy = false;
            if (arrow.activeInHierarchy)
            {
                arrow.SetActive(false);
            }
            if (BattleGlobals.isMagic)
            {
                //magic
                BattleGlobals.currentObj.GetComponent<Hero>().Magic(BattleGlobals.currentSid,GetComponent<Enemy>());
                BattleGlobals.isMagic = false;
            }
        }
    }
}
