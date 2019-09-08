using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnModel : MonoBehaviour
{
    Transform parent;
    Animation anim;
    GameObject acts;
    GameObject[] actObjs = new GameObject[2];
    GameObject backBtn;
    GameObject point1Btn;
    GameObject point2Btn;
    GameObject selectCharacter;
    Button changeBtn;
    Transform grid;
    private void Start()
    {
        backBtn = RoomGlobals.backBtn;
        point1Btn = RoomGlobals.point1Btn;
        point2Btn = RoomGlobals.point2Btn;
        selectCharacter = GameFuncs.FindHiden("selectCharacterParent");
        anim = Camera.main.GetComponent<Animation>();
        parent = gameObject.transform.parent;
        if (GameObject.Find("Acts"))
        {
            acts = GameObject.Find("Acts");
            if (acts.activeInHierarchy)
            {
                BindActsBtns();
                GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

    private void OnMouseDown()
    {
        RoomGlobals.isDestroy = true;
        GetComponent<CapsuleCollider>().enabled = false;
        //Hide Other AddBtns
        if (point1Btn.activeInHierarchy)
        {
            point1Btn.SetActive(false);
        }
        if (point2Btn.activeInHierarchy)
        {
            point2Btn.SetActive(false);
        }
        if (backBtn.activeInHierarchy)
        {
            backBtn.GetComponent<OnbackBtn>().enabled = false;
            backBtn.SetActive(false);
        }
        RoomGlobals.currentId = gameObject.name.Substring(0, 3);
        //Camera Rotate
        anim.Play("camera_roomTo_" + parent.gameObject.name);
        StartCoroutine(IShowActs());
    }
    IEnumerator IShowActs()
    {
        yield return new WaitForSeconds(1f);
        //Acts
        acts = GameFuncs.FindHiden("ActsParent");
        acts.SetActive(true);
        GameObject menu = acts.transform.Find("menu").gameObject;
        RoomGlobals.menu = menu;
        RoomGlobals.menu.SetActive(true);
        BindActsBtns();
        RoomFuncs.ChangeLove();
    }
    void BindActsBtns()
    {
        //Bind Btns
        Button closeBtn = UIManager.GetButton("closeBtn");
        changeBtn = UIManager.GetButton("changeBtn");
        Button chatBtn = UIManager.GetButton("chatBtn");
        Button giftBtn = UIManager.GetButton("giftBtn");
        Button dateBtn = UIManager.GetButton("dateBtn");
        Button storyBtn = UIManager.GetButton("storyBtn");
        closeBtn.onClick.AddListener(OncloseBtnClick);
        changeBtn.onClick.AddListener(OnchangeBtnClick);
        chatBtn.onClick.AddListener(OnchatBtnClick);
        giftBtn.onClick.AddListener(OngiftBtnClick);
        dateBtn.onClick.AddListener(OndateBtnClick);
        storyBtn.onClick.AddListener(OnstoryBtnClick);
    }
    void OnchatBtnClick()
    {
        MusicManager.PlaySe("click");
        parent = transform.parent;
        string id = GameFuncs.GetId(parent.transform);
        int indexs = 0;
        foreach (var dialog in Resources.LoadAll("Dialogs"))
        {
            if (dialog.name.Contains(id) && dialog.name.Contains("chat"))
            {
                indexs++;
            }
        }
        Globals.currentDialog = id + "_chat" + Random.Range(1, indexs + 1);
        GameFuncs.GoToScene("Chat");
    }
    void OngiftBtnClick()
    {
        MusicManager.PlaySe("click");
        RoomGlobals.menu.SetActive(false);
        GameObject gifts = GameFuncs.FindHiden("giftsParent");
        gifts.SetActive(true);
        gifts.AddComponent<Gift>();
    }
    void OndateBtnClick()
    {
        MusicManager.PlaySe("click");
        GameFuncs.GoToScene("SelectDateMap");
    }
    void OnstoryBtnClick()
    {
        MusicManager.PlaySe("click");
        RoomGlobals.menu.SetActive(false);
        GameObject story = GameFuncs.FindHiden("storyParent");
        story.SetActive(true);
        story.AddComponent<Story>();
    }
    void OncloseBtnClick()
    {
        if (GameObject.Find("story") != null)
        {
            GameObject.Find("story").gameObject.SetActive(false);
        }
        if (GameObject.Find("gifts") != null)
        {
            GameObject.Find("gifts").gameObject.SetActive(false);
        }
        if (selectCharacter.activeInHierarchy)
        {
            selectCharacter.SetActive(false);
        }
        MusicManager.PlaySe("click");
        acts.SetActive(false);
        anim.Play("camera_roomBack_" + parent.gameObject.name);
        if (!backBtn.activeInHierarchy)
        {
            backBtn.GetComponent<OnbackBtn>().enabled = true;
            backBtn.SetActive(true);
        }
        if (!point1Btn.activeInHierarchy && RoomGlobals.point1.transform.childCount == 0)
        {
            point1Btn.SetActive(true);
        }
        if (!point2Btn.activeInHierarchy && RoomGlobals.point2.transform.childCount == 0)
        {
            point2Btn.SetActive(true);
        }
        GetComponent<CapsuleCollider>().enabled = true;
    }
    void OnchangeBtnClick()
    {
        MusicManager.PlaySe("click");
        if (selectCharacter.activeInHierarchy == false)
        {
            selectCharacter.SetActive(true);
            grid = selectCharacter.transform.Find("grid");
            UIManager.GetButton("closeChangeBtn").onClick.AddListener(()=>
            {
                MusicManager.PlaySe("click");
                foreach (Transform t in grid)
                {
                    Destroy(t.gameObject);
                }
                selectCharacter.SetActive(false);
            });
            //Create Characters
            foreach (Transform child in grid)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < Globals.heroes.Count; i++)
            {
                if (!RoomFuncs.FindModel(Globals.heroes[i].id)
                    && (RoomGlobals.point1.transform.childCount == 0
                    || RoomGlobals.point2.transform.childCount == 0))//替换没有在房间内的
                {
                    bool canCreate = true;
                    foreach (Transform child in grid)
                    {
                        if (child.Find("Image").GetComponent<Image>().sprite.name.Contains(Globals.heroes[i].id))
                        {
                            canCreate = false;
                            break;
                        }
                    }
                    if (canCreate)
                    {
                        GameObject character_roomSelect = GameObject.Instantiate(
                            (GameObject)GameFuncs.GetResource("Prefabs/character_roomSelect"), grid);
                        string modelPath = "Prefabs/" + Globals.heroes[i].id;
                        string imagePath = "Character/Portrait/" + Globals.heroes[i].id;
                        Image image = character_roomSelect.transform.Find("Image").GetComponent<Image>();
                        image.sprite = Resources.Load<Sprite>(imagePath);

                        Button character_roomSelectBtn = character_roomSelect.GetComponent<Button>();
                        character_roomSelectBtn.onClick.AddListener(() =>
                        { Oncharacter_roomSelectBtnClick(parent, modelPath); });
                        if (RoomGlobals.roomInfos.ContainsKey(parent.gameObject.name))
                        {
                            RoomGlobals.roomInfos[parent.gameObject.name] = modelPath;
                        }
                        if (!RoomGlobals.loveDic.ContainsKey(Globals.heroes[i].id))
                        {
                            RoomGlobals.loveDic.Add(Globals.heroes[i].id, new LoveInfo(1, 0));
                        }
                    }
                }
                else if (RoomFuncs.FindModel(Globals.heroes[i].id)
                    && (RoomGlobals.point1.transform.childCount > 0
                    && RoomGlobals.point2.transform.childCount > 0))//交换在房间内的
                {
                    parent = GameFuncs.GetParent(RoomGlobals.currentId, GameObject.Find("Points").transform);
                    string id = "";//对方的id
                    Transform p = null;//对方的point
                    string modelPath1 = "Prefabs/" + GameFuncs.GetId(parent.transform);
                    if (parent.name == "point1")
                    {
                        id = GameFuncs.GetId(RoomGlobals.point2.transform);
                        p = RoomGlobals.point2.transform;
                    }
                    else if (parent.name == "point2")
                    {
                        id = GameFuncs.GetId(RoomGlobals.point1.transform);
                        p = RoomGlobals.point1.transform;
                    }
                    GameObject character_roomSelect = GameObject.Instantiate(
    (GameObject)GameFuncs.GetResource("Prefabs/character_roomSelect"), grid);
                    string modelPath2 = "Prefabs/" + id;
                    string imagePath = "Character/Portrait/" + id;
                    Image image = character_roomSelect.transform.Find("Image").GetComponent<Image>();
                    image.sprite = Resources.Load<Sprite>(imagePath);

                    Button character_roomSelectBtn = character_roomSelect.GetComponent<Button>();
                    character_roomSelectBtn.onClick.AddListener(() =>
                    { OnExchangeCharacter(parent, modelPath1, p, modelPath2); });
                    if (RoomGlobals.roomInfos.ContainsKey(p.gameObject.name))
                    {
                        RoomGlobals.roomInfos[p.gameObject.name] = modelPath1;
                    }
                    if (RoomGlobals.roomInfos.ContainsKey(parent.gameObject.name))
                    {
                        RoomGlobals.roomInfos[parent.gameObject.name] = modelPath2;
                    }
                }
                
            }
            //如果是自身或重复就销毁
            if (RoomGlobals.isDestroy)
            {
                for (int i = 0; i < grid.childCount; i++)
                {
                    if (i >= Globals.heroes.Count - 1)
                    {
                        Destroy(grid.GetChild(i).gameObject);
                    }
                }
                RoomGlobals.isDestroy = false;
            }
        }
    }
    void Oncharacter_roomSelectBtnClick(Transform point, string modelPath)
    {
        MusicManager.PlaySe("click");
        RoomFuncs.CreateCharacter(point, modelPath);
        Destroy(gameObject);
        RoomGlobals.currentId = modelPath.Replace("Prefabs/", "");
        RoomFuncs.ChangeLove();
    }
    void OnExchangeCharacter(Transform point1, string modelPath1,
        Transform point2, string modelPath2)
    {
        MusicManager.PlaySe("click");
        RoomFuncs.DestroyAll();
        RoomFuncs.CreateCharacter(point1, modelPath2);
        RoomFuncs.CreateCharacter(point2, modelPath1);
        RoomGlobals.currentId = modelPath2.Replace("Prefabs/", "");
        RoomFuncs.ChangeLove();
    }
}
