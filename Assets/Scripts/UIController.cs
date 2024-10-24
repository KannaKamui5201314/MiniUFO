using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TTSDK;
using TTSDK.UNBridgeLib.LitJson;
using UnityEngine;
using UnityEngine.UI;
using static TTSDK.TTAppLifeCycle;

public class UIController : MonoBehaviour
{
    private Button gameStartButton;
    private RectTransform gameStartButtonRectTransform;
    private TextMeshProUGUI Times;

    private readonly float scaleAmplitude = 0.1f;
    private float scaleFrequency = 1f;
    Vector3 gameStartButtonOriginalScale;

    private Button Ad;

    private GameObject ProgressBar;
    RectTransform ProgressBarRectTransform;
    Vector2 ProgressBarOriginalSize;

    TextMeshProUGUI Level;

    public GameObject enemy;
    public TextMeshProUGUI sum;

    Button Fire;

    public static bool  isFire = true;

    GameObject MoveRoulette;

    public Button speedUp;

    public bool isPressed;

    public Joystick moveJoystick;
    public Joystick fireJoystick;

    Canvas canvas;
    GameObject blood;
    GameObject blood1;
    GameObject blood2;
    GameObject blood3;
    GameObject blood4;
    GameObject blood5;

    GameObject eventSystem;
    GameController gameController;

    private Button SideBar;

    private Button Rank;

    void Start()
    {
        

        TT.GetAppLifeCycle().OnShow += OnAppShow;
        gameStartButton = transform.Find("GameStartButton").gameObject.GetComponent<Button>();
        gameStartButtonRectTransform = gameStartButton.GetComponent<RectTransform>();
        Times = gameStartButtonRectTransform.Find("Times").gameObject.GetComponent<TextMeshProUGUI>();

        Ad = transform.Find("Ad").gameObject.GetComponent<Button>();
        Ad.onClick.AddListener(AdOnClick);
        Ad.gameObject.SetActive(false);

        Level = transform.Find("Level").gameObject.GetComponent<TextMeshProUGUI>();

        ProgressBar = transform.Find("ProgressBar").gameObject;
        ProgressBarRectTransform = ProgressBar.GetComponent<RectTransform>();

        Fire = transform.Find("Fire").gameObject.GetComponent<Button>();

        MoveRoulette = transform.Find("MoveRoulette").gameObject;

        gameStartButtonOriginalScale = gameStartButtonRectTransform.localScale;
        ProgressBarOriginalSize = ProgressBarRectTransform.sizeDelta;
        //Debug.Log("ProgressBarOriginalScale=" + ProgressBarOriginalScale);
        gameStartButton.onClick.AddListener(GameStartButtonOnClick);
        if (Fire.gameObject.activeInHierarchy ==true)
        {
            Fire.onClick.AddListener(FireOnClick);
        }

        canvas = FindObjectOfType<Canvas>();
        blood = canvas.transform.Find("blood").gameObject;
        blood5 = blood.transform.Find("5").gameObject;
        blood4 = blood5.transform.Find("4").gameObject;
        blood3 = blood4.transform.Find("3").gameObject;
        blood2 = blood3.transform.Find("2").gameObject;
        blood1 = blood2.transform.Find("1").gameObject;

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();

        SideBar = transform.Find("SideBar").gameObject.GetComponent<Button>();
        SideBar.onClick.AddListener(SideBarOnClick);

        

        Rank = transform.Find("Rank").gameObject.GetComponent<Button>();
        Rank.onClick.AddListener(RankOnClick);
        Rank.gameObject.SetActive(false);
    }

    private void OnAppShow(Dictionary<string, object> param)
    {
        object locationValue;
        if (param.TryGetValue("location", out locationValue))
        {
            string location = locationValue as string;
            if (location == "sidebar_card")
            {
                
                Console.WriteLine("Found location: sidebar_card");
            }
            else
            {
                Console.WriteLine("Location not found or not sidebar_card");
            }
        }
        
    }

    //加速按钮
    void SpeedUp()
    {
        if (isPressed)
        {
            if (Global.UFOSpeed < Global.UFOMaxSpeed)
            {
                Global.UFOSpeed += Time.deltaTime;
            }
        }
        else
        {
            if (Global.UFOSpeed > 4.6f)
            {
                Global.UFOSpeed -= Time.deltaTime;
            }
        }
    }

    public void OnSpeedUpButtonDown()
    {
        // 按钮按下时的处理逻辑
        //Debug.Log("Button is pressed.");
        isPressed = true;
        isFire = false;
    }

    public void OnSpeedUpButtonUp()
    {
        // 按钮抬起时的处理逻辑
        //Debug.Log("Button is released.");
        isPressed = false;
        isFire = true;
    }

    public bool IsFire()
    {
        return isFire;
    }
    private void LateUpdate()
    {
        
    }

    void Update()
    {
        //Debug.Log("Global.UFOSpeed=" + Global.UFOSpeed);
        if (!Global.gameStart)
        {
            isPressed = false;
            isFire = true;
            //Level.gameObject.SetActive(false);
            enemy.SetActive(false);
            sum.gameObject.SetActive(false);
            ProgressBar.SetActive(false);
            gameStartButton.gameObject.SetActive(true);
            Times.text = Global.Times.ToString();
            Fire.gameObject.SetActive(false);
            MoveRoulette.SetActive(false);
            speedUp.gameObject.SetActive(false);
            moveJoystick.transform.position= Vector2.zero;
            moveJoystick.gameObject.SetActive(false);
            fireJoystick.gameObject.SetActive(false);
            blood.SetActive(false);
            if (Global.Times == 0)
            {
                Ad.gameObject.SetActive(true);
                //Debug.Log("onshow=" + TT.GetAppLifeCycle().OnShow);
            }
            Debug.Log("IsClicked_Sidebar=" + TT.PlayerPrefs.GetString("IsClicked_Sidebar"));
            if (TT.PlayerPrefs.GetString("IsClicked_Sidebar") =="" && Global.Times == 0)
            {
                SideBar.gameObject.SetActive(true);
            }
            else
            {
                SideBar.gameObject.SetActive(false);
            }

            Rank.gameObject.SetActive(true);
            
            
        }
        GameStartButtonAnimation();
        if (Global.gameStart)
        {
            Level.gameObject.SetActive(true);
            enemy.SetActive(true);
            sum.gameObject.SetActive(true);
            ProgressBar.SetActive(true);
            Level.text = Global.level.ToString();
            Fire.gameObject.SetActive(false);
            MoveRoulette.SetActive(false);
            speedUp.gameObject.SetActive(true);
            moveJoystick.gameObject.SetActive(true);
            fireJoystick.gameObject.SetActive(true);
            blood.SetActive(true);
            //Debug.Log("TheSpacePirateNumber ="+ Global.TheSpacePirateNumber);
            sum.text = "X" + Global.TheSpacePirateNumber;
            SideBar.gameObject.SetActive(false);
            Rank.gameObject.SetActive(false);
            Ad.gameObject.SetActive(false);

        }
        SpeedUp();


    }

    //void SetProgressBar()
    //{
       
    //    if (Global.level>0)
    //    {
    //        float newXScale = Global.TheSpacePirateNumber / 3f / Global.level;

    //        Vector2 newScale = new(ProgressBarOriginalSize.x * newXScale, ProgressBarOriginalSize.y); ;
            
            
    //        ProgressBarRectTransform.sizeDelta = newScale;
    //    }
    //}

    void GameStartButtonOnClick()
    {
        if (Global.Times >= 1)
        {
            gameStartButton.gameObject.SetActive(false);
            Global.gameStart = true;
            Debug.Log("游戏开始！");
        }
    }

    void AdOnClick()
    {
        Global.Times = 5;
        TT.PlayerPrefs.SetString("Times", Global.Times.ToString());
        Ad.gameObject.SetActive(false);
    }

    void FireOnClick()
    {
        isFire = true;
    }

    void GameStartButtonAnimation()
    {
        float time = Time.time;
        float sineValue = scaleAmplitude * Mathf.Sin(scaleFrequency * time);
        Vector3 newScale = new(gameStartButtonOriginalScale.x + sineValue, gameStartButtonOriginalScale.y + sineValue, gameStartButtonOriginalScale.z);
        gameStartButtonRectTransform.localScale = newScale;
    }

    //blood：需要减少的血量
    public void SetBlood(int blood)
    {
        gameController.PlayAudio();
        
        Global.AffectedTimes += blood;
        
        switch (Global.AffectedTimes)
        {
            case 1:
                blood1.SetActive(false);
                break;
            case 2:
                blood2.SetActive(false);
                break;
            case 3:
                blood3.SetActive(false);
                break;
            case 4:
                blood4.SetActive(false);
                break;
            default:
                Global.AffectedTimes = 0;
                SetImRank(Global.level);
                Global.gameStart = false;
                Global.UFOSpeed = 4.6f;
                Global.Times -= 1;
                TT.PlayerPrefs.SetString("Times", Global.Times.ToString());
                blood1.SetActive(true);
                blood2.SetActive(true);
                blood3.SetActive(true);
                blood4.SetActive(true);
                blood5.SetActive(true);
                break;
        }
    }

    private void SideBarOnClick()
    {
        SideBar.gameObject.transform.Find("Dialog_SideBar").gameObject.SetActive(true);
    }

    private void RankOnClick()
    {
        SetImRank(Global.level);
        GetImRank();
    }

    public void GetImRank()
    {
        var paramJson = new JsonData
        {
            ["rankType"] = "all",
            ["dataType"] = 0,
            ["relationType"] = "all",
            ["suffix"] = "关",
        };
        Debug.Log($"GetImRankList param:{paramJson.ToJson()}");
        TT.GetImRankList(paramJson, (b, s) =>
        {
            if (b)
            {
                Debug.Log("GetImRankList");
            }
            else
            {
                Debug.Log("GetImRankList 2");
            }
        });
    }
    public void SetImRank(int level)
    {
        var paramJson = new JsonData
        {
            ["dataType"] = 0,
            ["value"] = level - 1,
            ["priority"] = level-1,
        };
        Debug.Log($"SetImRankData param:{paramJson.ToJson()}");
        TT.SetImRankData(paramJson, (b, s) =>
        {
            if (b)
            {
                Debug.Log("SetImRankData success ");
            }
            else
            {
                Debug.Log("SetImRankData fail ");
            }
        });
    }
}
