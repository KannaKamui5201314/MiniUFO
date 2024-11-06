/*
 * 道具管理器
 * 简介：生成所有道具
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    GameObject SuperluminalSpeed;//加速道具
    GameObject SuperluminalSpeedes;//加速道具父物体容器
    public ObjectPool SuperluminalSpeedObjectPool;//加速道具对象池

    GameObject Stationary;//静止道具
    GameObject Stationaries;
    public ObjectPool StationaryObjectPool;

    GameObject Invincible;//无敌道具
    GameObject Invincibles;
    public ObjectPool InvincibleObjectPool;

    GameObject Fluctuate;//宇宙波动
    float timer;//宇宙波动道具刷新的定时器

    GameObject Guard;//星空卫队
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
                //设置名称、父物体、刷新位置
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
            //60秒刷新一个宇宙波动
            if (timer > 60f)
            {
                timer = 0f;
                GameObject obj = GameObject.Instantiate(Fluctuate);
                obj.transform.SetParent(transform);
                obj.transform.position = new(-506f, 0f);//刷新的位置
            }
        }
        if (!Global.gameStart)
        {
            //游戏结束，初始化刷新定时器
            timer = 0f;
        }
    }

    //设置道具是否渲染
    //   isTouch:是否接触
    public void SetRenderer(Renderer obj, bool isTouch)
    {
        float distance = (ufoTransform.position - obj.transform.position).magnitude;
        //相机外不渲染,否则渲染
        if (distance > Screen.width / 100f / 2f + 1f)
        {
            obj.enabled = false;
        }
        else
        {
            //拾取道具时不渲染道具
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
