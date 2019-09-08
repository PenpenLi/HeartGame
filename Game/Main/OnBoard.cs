using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OnBoard : MonoBehaviour,IBeginDragHandler,IEndDragHandler{
    float posX;//board偏移量
    float width;//board宽度
    float height;//board高度
    Transform boardBtns;
    ScrollRect sr;
    Transform boardToggles;
    int num;//the count of boards
    Toggle[] toggles = new Toggle[4];
    string[] boardTexts;
    Sprite[] boardImages = new Sprite[4];
    int lastIndex;//上次索引(toggle)
    int currIndex;//现在索引
    float lastX;//上次sr位置
    float timer;
    bool canSlide = true;
    //timer to auto slide
    // Use this for initialization
    void Start () {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Main":
                num = 4;
                boardImages = Resources.LoadAll<Sprite>("Background");
                boardTexts = new string[]{ "Battle", "Heroes", "Room", "Summon" };
                break;
            case "Summon":
                num = 3;
                boardImages = Resources.LoadAll<Sprite>("Summon");
                boardTexts = new string[num];
                break;
            default:
                break;
        }
        boardBtns = transform.Find("boardBtns");
        sr = GetComponent<ScrollRect>();
        boardToggles = transform.Find("boardToggles");
        width = boardBtns.GetComponent<RectTransform>().sizeDelta.x;
        height= boardBtns.GetComponent<RectTransform>().sizeDelta.y;
        posX = width / 2;
        boardBtns.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX*(num-1),0);
        boardBtns.GetComponent<RectTransform>().sizeDelta = new Vector2(width*num, boardBtns.GetComponent<RectTransform>().rect.height);
        //instantiate btns and toggles
        for (int i = 0; i < num; i++)
        {
            //btns
            GameObject boardBtn = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/boardBtn"));
            boardBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            boardBtn.transform.SetParent(boardBtns);
            boardBtn.GetComponent<Image>().sprite = boardImages[i];
            boardBtn.GetComponentInChildren<Text>().text = boardTexts[i];
            boardBtn.GetComponent<Button>().onClick.AddListener(()=> { OnboardBtnClick(boardBtn.GetComponentInChildren<Text>().text); });
            //toggles
            GameObject boardToggle = Instantiate((GameObject)GameFuncs.GetResource("Prefabs/boardToggle"));
            boardToggle.transform.SetParent(boardToggles);
            Toggle bt = boardToggle.GetComponent<Toggle>();
            bt.group = boardToggles.GetComponent<ToggleGroup>();
            if (i == 0) bt.isOn = true;
            bt.onValueChanged.AddListener((bool isOn)=> { OnboardToggleClick(isOn,bt); });
            toggles[i] = bt;
        }
    }
	
	// 定时滚动
	void Update () {
        timer += Time.deltaTime;
        if (timer>=6 && canSlide)//6s滑动一次
        {
            canSlide = false;
            if (currIndex < num-1) currIndex++;
            else currIndex = 0;
            StopAllCoroutines();
            StartCoroutine(MoveTo(currIndex * ((float)1 / (num-1))));
        }
	}
    void OnboardBtnClick(string boardStr)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Main":
                GameFuncs.GoToSceneAsync(boardStr);
                break;
            case "Summon":
                break;
            default:
                break;
        } 
    }
    void OnboardToggleClick(bool isOn,Toggle t)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn) { currIndex = i; break; }
        }
        StopAllCoroutines();
        StartCoroutine(MoveTo(currIndex * ((float)1 / (num - 1))));
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastX = sr.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)//toggle change ison
    {
        if (Mathf.Abs(lastX- sr.horizontalNormalizedPosition)>=0.01f)
        {
            if (lastX < sr.horizontalNormalizedPosition)//向左划
            {
                if (currIndex<num-1)
                {
                    currIndex++;
                    StopAllCoroutines();
                    StartCoroutine(MoveTo(currIndex * ((float)1 / (num - 1))));
                }
            }
            else if (lastX > sr.horizontalNormalizedPosition)//向右划
            {
                if (currIndex >0)
                {
                    currIndex--;
                    StopAllCoroutines();
                    StartCoroutine(MoveTo(currIndex * ((float)1 / (num - 1))));
                }
            }
        }
    }
    IEnumerator MoveTo(float targetX)
    {
        while (true)
        {
            if (Mathf.Abs(sr.horizontalNormalizedPosition - targetX) >= 0.01f)
            {
                sr.horizontalNormalizedPosition = Mathf.Lerp(
                sr.horizontalNormalizedPosition, targetX,
                Time.deltaTime * (Mathf.Abs(currIndex - lastIndex)*10));
            }
            else
            {
                sr.horizontalNormalizedPosition = targetX;
                lastIndex = currIndex;
                toggles[currIndex].isOn = true;
                timer = 0;
                canSlide = true;
                break;
            }
            yield return null;
        }
    }
}
