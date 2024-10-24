using System.Collections;
using System.Collections.Generic;
using TTSDK;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private Button _gameStartButton;

    GameObject theSpacePirates;
    GameObject theSpacePirate;

    GameObject Lasers;

    GameObject UFO;

    Transform UFOTransform;

    //���˶����
    public ObjectPool objectPool;

    Canvas canvas;
    GameObject blood;
    GameObject blood1;
    GameObject blood2;
    GameObject blood3;
    GameObject blood4;
    GameObject blood5;

    private AudioSource audioSource;

    private void Awake()
    {
        theSpacePirate = Resources.Load<GameObject>("Prefabs/TheSpacePirate");
        
        if (TT.PlayerPrefs.GetString("Times") == "")
        {
            TT.PlayerPrefs.SetString("Times", "5");
            //Debug.Log(TT.PlayerPrefs.GetString("IsClicked_Sidebar"));
        }
        int.TryParse(TT.PlayerPrefs.GetString("Times"), out Global.Times);

    }


    void Start()
    {
        
        theSpacePirates = GameObject.Find("TheSpacePirates");
        
        _gameStartButton = GameObject.FindGameObjectWithTag("GameStartButton").GetComponent<Button>();
        UFO = GameObject.FindGameObjectWithTag("Player");
        UFOTransform = UFO.GetComponent<Transform>();
        objectPool = new ObjectPool(theSpacePirate, 3);

        canvas = FindObjectOfType<Canvas>();
        blood = canvas.transform.Find("blood").gameObject;
        blood5 = blood.transform.Find("5").gameObject;
        blood4 = blood5.transform.Find("4").gameObject;
        blood3 = blood4.transform.Find("3").gameObject;
        blood2 = blood3.transform.Find("2").gameObject;
        blood1 = blood2.transform.Find("1").gameObject;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Global.TheSpacePirateNumber = theSpacePirates.transform.childCount;
        
        if (Global.gameStart)
        {
            if (Global.TheSpacePirateNumber ==0)
            {
                Global.level += 1;
                UFO.SetActive(true);
                UFO.GetComponent<SpriteRenderer>().enabled = true;

                blood1.SetActive(true);
                blood2.SetActive(true);
                blood3.SetActive(true);
                blood4.SetActive(true);
                blood5.SetActive(true);
            }
        }
        if (!Global.gameStart)
        {
            //��ʼ��
            //Destroy(theSpacePirates);
            Lasers = GameObject.Find("Lasers");
            Destroy(Lasers);
            while (theSpacePirates.transform.childCount > 0)
            {
                objectPool.ReturnObject(theSpacePirates.transform.GetChild(0).gameObject);
                theSpacePirates.transform.GetChild(0).parent = null;
            }

            //Global.gameStart = false;
            //_gameStartButton.gameObject.SetActive(true);
            Global.level = 0;
            Global.UFOSpeed = 4.6f;
            Global.TheSpacePirateNumber = 0;
            Global.AffectedTimes = 0;
            UFO.GetComponent<SpriteRenderer>().enabled = false;
            UFO.GetComponent<Transform>().position = new(0, 0, 0);
            
            //UFO.SetActive(false);
            
        }
    }
    private void LateUpdate()
    {
        if (Global.gameStart)
        {
            if (Global.TheSpacePirateNumber == 0)
            {
                InstantiateTheSpacePirates();
            }
        }
    }

    //ȡ������
    void InstantiateTheSpacePirates()
    {
        if (theSpacePirates ==null)
        {
            theSpacePirates = new GameObject
            {
                name = "TheSpacePirates"
            };
        }
        //ÿ�غϼ�3�����˵�����
        for (int i = 0; i< 3* Global.level;i++)
        {
            
            GameObject newTheSpacePirate = objectPool.GetObject();
            Global.TheSpacePirateNumber += 1;

            newTheSpacePirate.name = "TheSpacePirate";
            newTheSpacePirate.transform.SetParent(theSpacePirates.transform, false);

            //�˴��޸�Ԥ�������ݣ���Ҫ��λ��
            newTheSpacePirate.transform.position = new Vector3(
                Random.Range(-40f + UFOTransform.position.x, 40f + UFOTransform.position.y), // x ������ -10 �� 10 ֮�����
                Random.Range(-40f + UFOTransform.position.x, 40f + UFOTransform.position.y), // y ������ -10 �� 10 ֮�����
                0f
                );
        }
    }

    //�����е���Ƶ
    public void PlayAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
