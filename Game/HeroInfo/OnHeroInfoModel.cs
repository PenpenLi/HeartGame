using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHeroInfoModel : MonoBehaviour
{
    Animator anim;
    float timer = 3;
    bool canTouch = true;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        if (canTouch)
        {
            anim.SetTrigger("pose");
            MusicManager.PlayVoice(HeroGlobals.currentid + "_click");
            canTouch =false;
            StartCoroutine(Wait(GameFuncs.GetAnim(anim, "pose").length));
        }
    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        canTouch = true;
    }
}
