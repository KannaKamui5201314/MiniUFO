/*
 * ���߹�����
 * ��飺�������е���
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    GameObject SuperluminalSpeed;//���ٵ���
    GameObject SuperluminalSpeedes;//���ٵ��߸���������
    public ObjectPool SuperluminalSpeedObjectPool;//���ٵ��߶����

    GameObject Stationary;//��ֹ����
    GameObject Stationaries;
    public ObjectPool StationaryObjectPool;

    GameObject Invincible;//�޵е���
    GameObject Invincibles;
    public ObjectPool InvincibleObjectPool;

    GameObject Fluctuate;//���沨��
    float timer;//���沨������ˢ�µĶ�ʱ��

    GameObject Guard;//�ǿ�����
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

    //ʵ������ͨ����
    void SetActived(string name, GameObject Prefabs, ObjectPool objectPool)
    {
        if (objectPool != null)
        {
            if (objectPool.GetCount() > 0)
            {
                //�������ơ������塢ˢ��λ��
                GameObject newObject = objectPool.GetObject();
                newObject.name = name;
                newObject.transform.SetParent(Prefabs.transform);
                newObject.transform.position = new Vector3(
                Random.Range(-500f, 500f), // x ������ -500f �� 500f ֮�����
                Random.Range(-500f, 500f), // y ������ -500f �� 500f ֮�����
                0f
                );
            }
        }
        else
        {
            Debug.Log(objectPool+ "=null");
        }
        
    }

    //ʵ�������漶���Ѳ���
    void GetFluctuate()
    {
        if (Global.gameStart)
        {
            timer += Time.deltaTime;
            //60��ˢ��һ�����沨��
            if (timer > 60f)
            {
                timer = 0f;
                GameObject obj = GameObject.Instantiate(Fluctuate);
                obj.transform.SetParent(transform);
                obj.transform.position = new(-506f, 0f);//ˢ�µ�λ��
            }
        }
        if (!Global.gameStart)
        {
            //��Ϸ��������ʼ��ˢ�¶�ʱ��
            timer = 0f;
        }
    }

    //���õ����Ƿ���Ⱦ
    //   isTouch:�Ƿ�Ӵ�
    public void SetRenderer(Renderer obj, bool isTouch)
    {
        float distance = (ufoTransform.position - obj.transform.position).magnitude;
        //����ⲻ��Ⱦ,������Ⱦ
        if (distance > Screen.width / 100f / 2f + 1f)
        {
            obj.enabled = false;
        }
        else
        {
            //ʰȡ����ʱ����Ⱦ����
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
