using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    GameObject SuperluminalSpeed;
    GameObject SuperluminalSpeedes;
    public ObjectPool SuperluminalSpeedObjectPool;

    GameObject Stationary;
    GameObject Stationaries;
    public ObjectPool StationaryObjectPool;

    GameObject Invincible;
    GameObject Invincibles;
    public ObjectPool InvincibleObjectPool;

    GameObject Fluctuate;
    float timer;

    GameObject Guard;
    GameObject Guards;
    public ObjectPool GuardObjectPool;

    Transform ufoTransform;

    private void Awake()
    {
        ufoTransform = GameObject.FindGameObjectWithTag("Player").transform;

        SuperluminalSpeed = Resources.Load<GameObject>("Prefabs/prop/SuperluminalSpeed");
        SuperluminalSpeedes = GameObject.FindGameObjectWithTag("SuperluminalSpeedes");
        SuperluminalSpeedObjectPool = new ObjectPool(SuperluminalSpeed, Global.InitialCount);

        Stationary = Resources.Load<GameObject>("Prefabs/prop/Stationary");
        Stationaries = GameObject.FindGameObjectWithTag("Stationaries");
        StationaryObjectPool = new ObjectPool(Stationary, Global.InitialCount);

        Invincible = Resources.Load<GameObject>("Prefabs/prop/Invincible");
        Invincibles = GameObject.FindGameObjectWithTag("Invincibles");
        InvincibleObjectPool = new ObjectPool(Invincible, Global.InitialCount);

        Fluctuate = Resources.Load<GameObject>("Prefabs/prop/Fluctuate");

        Guard = Resources.Load<GameObject>("Prefabs/prop/Guard");
        Guards = GameObject.FindGameObjectWithTag("Guards");
        GuardObjectPool = new ObjectPool(Guard, Global.InitialCount/2);

    }

    void Update()
    {
        SetActived("SuperluminalSpeed", SuperluminalSpeedes, SuperluminalSpeedObjectPool);
        SetActived("Stationary", Stationaries, StationaryObjectPool);
        SetActived("Invincible", Invincibles, InvincibleObjectPool);

        GetFluctuate();

        SetActived("Guard", Guards, GuardObjectPool);
    }

    //实例化普通道具
    void SetActived(string name, GameObject Prefabs, ObjectPool objectPool)
    {
        if (objectPool != null)
        {
            if (objectPool.GetCount() > 0)
            {
                GameObject newObject = objectPool.GetObject();
                newObject.name = name;
                newObject.transform.SetParent(Prefabs.transform);
                newObject.transform.position = new Vector3(
                Random.Range(-500f, 500f), // x 坐标在 -500f 到 500f 之间随机
                Random.Range(-500f, 500f), // y 坐标在 -500f 到 500f 之间随机
                0f
                );
            }
        }
        else
        {
            Debug.Log(objectPool+ "=null");
        }
        
    }

    //实例化宇宙级灾难波动
    void GetFluctuate()
    {
        if (Global.gameStart)
        {
            timer += Time.deltaTime;
            if (timer > 60f)
            {
                timer = 0f;
                GameObject obj = GameObject.Instantiate(Fluctuate);
                obj.transform.SetParent(transform);
                obj.transform.position = new(-506f, 0f);
            }
        }
        if (!Global.gameStart)
        {
            timer = 0f;
        }
    }

    //   isTouch:是否触摸
    public void SetRenderer(Renderer obj, bool isTouch)
    {
        float distance = (ufoTransform.position - obj.transform.position).magnitude;
        if (distance > Screen.width / 100f / 2f + 1f)
        {
            obj.enabled = false;
        }
        else
        {
            if (isTouch)
            {
                obj.enabled = false;
            }
            else
            {
                obj.enabled = true;
            }
        }
    }
}
