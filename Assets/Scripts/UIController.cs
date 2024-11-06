/*
 * UI������
 * ������Ϸʱ����ʾ��Ϸ��ʼ��Ļ��ť������󼴿ɿ�ʼ��Ϸ
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
    private Button gameStartButton;//��Ϸ��ʼ��Ļ��ť
    private RectTransform gameStartButtonRectTransform;//��Ϸ��ʼ��Ļ��ťTransform
    private TextMeshProUGUI Times;//��Ϸ��ս����

    private readonly float scaleAmplitude = 0.1f;//��Ϸ��ʼ��Ļ�������
    private float scaleFrequency = 1f;//��Ϸ��ʼ��Ļ����Ƶ��
    Vector3 gameStartButtonOriginalScale;//��Ϸ��ʼ��Ļ��ʼ����

    private Button Ad;//�����水ť����ת��TTSDK�ڵĹ��ģ�����

    private GameObject ProgressBar;//������
    RectTransform ProgressBarRectTransform;//������
    Vector2 ProgressBarOriginalSize;//������

    TextMeshProUGUI Level;//���Ͻ���ʾ�Ĺ���

    public GameObject enemy;//����Image UI
    public TextMeshProUGUI sum;//��ǰ��������

    Button Fire;//���𹥻���ť

    public static bool  isFire = true;//�Ƿ񿪻�״̬��������

    GameObject MoveRoulette;//������

    public Button speedUp;//���ټ�

    public bool isPressed;//���ټ��Ƿ���

    public Joystick moveJoystick;//����ҡ�ˣ��������UFO�ƶ���λ�ڻ�������
    public Joystick fireJoystick;//����ҡ�ˣ��������UFO�ϵ���̨��������λ�ڻ�������

    Canvas canvas;//����
    //Ѫ��UI��blood1-blood5��bloodΪ������
    GameObject blood;
    GameObject blood1;
    GameObject blood2;
    GameObject blood3;
    GameObject blood4;
    GameObject blood5;

    GameObject eventSystem;
    GameController gameController;

    private Button SideBar;//���붶���������ť

    private Button Rank;//���붶�����а�ť

    void Start()
    {

        //TT.GetAppLifeCycle().OnShow��ȡ����С��Ϸ��ͨ��ʲô��ʽ
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

    //���붶��С��Ϸ�ص�
    private void OnAppShow(Dictionary<string, object> param)
    {
        object locationValue;
        if (param.TryGetValue("location", out locationValue))
        {
            string location = locationValue as string;
            //sidebar_card���������
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

    //UFO���ٰ�ť�����������ƶ��ٶȣ��ſ����ٰ�ť����Զ�����
    void SpeedUp()
    {
        if (isPressed)
        {
            //����
            if (Global.UFOSpeed < Global.UFOMaxSpeed)
            {
                Global.UFOSpeed += Time.deltaTime;
            }
        }
        else
        {
            //����
            if (Global.UFOSpeed > 4.6f)
            {
                Global.UFOSpeed -= Time.deltaTime;
            }
        }
    }

    public void OnSpeedUpButtonDown()
    {
        // ��ť����ʱ�Ĵ����߼�
        //Debug.Log("Button is pressed.");
        isPressed = true;
        isFire = false;
    }

    public void OnSpeedUpButtonUp()
    {
        // ��ţ̌��ʱ�Ĵ����߼�
        //Debug.Log("Button is released.");
        isPressed = false;
        isFire = true;
    }

    //������
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
        //��Ϸδ��ʼʱ������ʾ����UI��ֻ��ʾ��Ϸ��ʼ��ť
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
        //��Ϸ��ʼʱ����ʾUI
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

    //��Ϸ��ʼ��ť�ĵ���¼�
    void GameStartButtonOnClick()
    {
        if (Global.Times >= 1)
        {
            //��Ϸ��ʼ��ť��ʧ
            gameStartButton.gameObject.SetActive(false);
            Global.gameStart = true;
            Debug.Log("��Ϸ��ʼ��");
        }
    }

    //��水ť�ĵ���¼�����ɿ����������ȡ5����ս����
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

    //��Ϸ��ʼUI�Ķ�����ѭ������С���������Һ���
    void GameStartButtonAnimation()
    {
        float time = Time.time;
        float sineValue = scaleAmplitude * Mathf.Sin(scaleFrequency * time);
        Vector3 newScale = new(gameStartButtonOriginalScale.x + sineValue, gameStartButtonOriginalScale.y + sineValue, gameStartButtonOriginalScale.z);
        gameStartButtonRectTransform.localScale = newScale;
    }

    //blood����Ҫ���ٵ�Ѫ�������Ѫ��Ϊ5���������Ϸ����
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

    //�����������ť����¼�
    private void SideBarOnClick()
    {
        //��ʾ�Ƿ���붶��������Ի���
        SideBar.gameObject.transform.Find("Dialog_SideBar").gameObject.SetActive(true);
    }
    //�������а�ť����¼�
    private void RankOnClick()
    {
        SetImRank(Global.level);
        GetImRank();
    }

    //�Ӷ�����ȡ���а���Ϣ
    public void GetImRank()
    {
        var paramJson = new JsonData
        {
            ["rankType"] = "all",//�������ͣ���ѡֵ��day��week��month��all
            ["dataType"] = 0,//�����������ͣ���ѡֵ�� 0  ��  1��0 ��ʾ�������ݱ�����Ϊ number��1 ��ʾ�������ݱ�����Ϊ string
            ["relationType"] = "all",//���а�����
            ["suffix"] = "��",//�������ݺ���ֶΣ����硰�ء������֡�
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
    //����Ϸ�ڵ�������а����ݷ��͸��������а�
    //levelΪ��߹���
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
