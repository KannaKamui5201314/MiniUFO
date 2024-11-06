/*
 * UI控制器
 * 进入游戏时会显示游戏开始字幕按钮，点击后即可开始游戏
 */
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
    private Button gameStartButton;//游戏开始字幕按钮
    private RectTransform gameStartButtonRectTransform;//游戏开始字幕按钮Transform
    private TextMeshProUGUI Times;//游戏挑战次数

    private readonly float scaleAmplitude = 0.1f;//游戏开始字幕缩放振幅
    private float scaleFrequency = 1f;//游戏开始字幕缩放频率
    Vector3 gameStartButtonOriginalScale;//游戏开始字幕初始缩放

    private Button Ad;//点击广告按钮，跳转到TTSDK内的广告模块界面

    private GameObject ProgressBar;//已弃用
    RectTransform ProgressBarRectTransform;//已弃用
    Vector2 ProgressBarOriginalSize;//已弃用

    TextMeshProUGUI Level;//左上角显示的关数

    public GameObject enemy;//敌人Image UI
    public TextMeshProUGUI sum;//当前敌人数量

    Button Fire;//开火攻击按钮

    public static bool  isFire = true;//是否开火状态，已弃用

    GameObject MoveRoulette;//已弃用

    public Button speedUp;//加速键

    public bool isPressed;//加速键是否按下

    public Joystick moveJoystick;//虚拟摇杆，控制玩家UFO移动，位于画布左下
    public Joystick fireJoystick;//虚拟摇杆，控制玩家UFO上的炮台攻击方向，位于画布右下

    Canvas canvas;//画布
    //血量UI，blood1-blood5，blood为父物体
    GameObject blood;
    GameObject blood1;
    GameObject blood2;
    GameObject blood3;
    GameObject blood4;
    GameObject blood5;

    GameObject eventSystem;
    GameController gameController;

    private Button SideBar;//进入抖音侧边栏按钮

    private Button Rank;//进入抖音排行榜按钮

    void Start()
    {

        //TT.GetAppLifeCycle().OnShow获取进入小游戏是通过什么方式
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

    //进入抖音小游戏回调
    private void OnAppShow(Dictionary<string, object> param)
    {
        object locationValue;
        if (param.TryGetValue("location", out locationValue))
        {
            string location = locationValue as string;
            //sidebar_card侧边栏进入
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

    //UFO加速按钮，持续增加移动速度，放开加速按钮后会自动减速
    void SpeedUp()
    {
        if (isPressed)
        {
            //加速
            if (Global.UFOSpeed < Global.UFOMaxSpeed)
            {
                Global.UFOSpeed += Time.deltaTime;
            }
        }
        else
        {
            //减速
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

    //已弃用
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
        //游戏未开始时，不显示其他UI，只显示游戏开始按钮
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
        //游戏开始时，显示UI
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

    //游戏开始按钮的点击事件
    void GameStartButtonOnClick()
    {
        if (Global.Times >= 1)
        {
            //游戏开始按钮消失
            gameStartButton.gameObject.SetActive(false);
            Global.gameStart = true;
            Debug.Log("游戏开始！");
        }
    }

    //广告按钮的点击事件，完成看广告任务后获取5次挑战机会
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

    //游戏开始UI的动画，循环变大变小，类似正弦函数
    void GameStartButtonAnimation()
    {
        float time = Time.time;
        float sineValue = scaleAmplitude * Mathf.Sin(scaleFrequency * time);
        Vector3 newScale = new(gameStartButtonOriginalScale.x + sineValue, gameStartButtonOriginalScale.y + sineValue, gameStartButtonOriginalScale.z);
        gameStartButtonRectTransform.localScale = newScale;
    }

    //blood：需要减少的血量，最大血量为5，扣完后游戏结束
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

    //抖音侧边栏按钮点击事件
    private void SideBarOnClick()
    {
        //显示是否进入抖音侧边栏对话框
        SideBar.gameObject.transform.Find("Dialog_SideBar").gameObject.SetActive(true);
    }
    //抖音排行榜按钮点击事件
    private void RankOnClick()
    {
        SetImRank(Global.level);
        GetImRank();
    }

    //从抖音获取排行榜信息
    public void GetImRank()
    {
        var paramJson = new JsonData
        {
            ["rankType"] = "all",//排名类型，可选值：day、week、month、all
            ["dataType"] = 0,//排名数据类型，可选值： 0  或  1。0 表示返回数据被解析为 number。1 表示返回数据被解析为 string
            ["relationType"] = "all",//排行榜类型
            ["suffix"] = "关",//排名数据后的字段，比如“关”、“分”
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
    //将游戏内的相关排行榜数据发送给抖音排行榜
    //level为最高关数
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
