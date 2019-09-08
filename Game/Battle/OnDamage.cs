using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDamage : MonoBehaviour
{
    Text text;
    Color color;
    RectTransform rt;
    float mina = 0;
    float maxa = 1;
    float mins = 10;
    float maxs = 15;
    float miny = 165;
    float maxy = 365;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        rt = transform.GetComponent<RectTransform>();
        color = text.color;
        text.color = new Color(color.r, color.g, color.b, 0);
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (mina < 1)
        {
            mina += Time.deltaTime * 2;
            text.color = new Color(color.r, color.g, color.b, mina);
        }
        else
        {
            maxa -= Time.deltaTime;
            text.color = new Color(color.r, color.g, color.b, maxa);
        }
        if (mins < 15)
        {
            mins += Time.deltaTime * 2;
            transform.localScale = new Vector2(mins, mins);
        }
        else
        {
            maxs -= Time.deltaTime;
            transform.localScale = new Vector2(maxs, maxs);
        }
        if (miny < 365)
        {
            miny += 10;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, miny);
        }
        else
        {
            maxy -= 10;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, maxy);
        }
    }
}
