/*
 * ���˿�����
 * ��Ҫ�ǿ����ƶ�
 */
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TheSpacePirateController : MonoBehaviour
{

    readonly float safeDistance = Screen.width/100f * 0.5f;//��Ļ��ȵ�һ�룬����1920�ֱ����´�Լ��10��λ

    private Transform ufo;//���

    private float limitTime = 2f;//����ÿ2��ı��ƶ�����
    private float flyTimer;//�����ƶ�ʱ�䶨ʱ��
    private float fireFrequencyTimer;//���˿���ʱ��

    Vector3 newEnemyPositon;

    Vector2 forceDirection;//Ŀ�귽��

    GameObject laser;//�ӵ�
    GameObject Lasers;//�ӵ�������

    private Rigidbody2D rd;//���˸������

    //����
    void Start()
    {
        laser = Resources.Load<GameObject>("Prefabs/Laser");
        
        ufo = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        rd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Global.gameStart)
        {
            Move();
        }
    }

    //�����ƶ�����
    void Move()
    {
        flyTimer += Time.deltaTime;
        fireFrequencyTimer += Time.deltaTime;
        //Debug.Log("safeDistance=" + safeDistance);
        //���˺����UFO�ľ���С����Ļ�������ܻ��ƶ�
        if (GetEnemyDistance() < 2*safeDistance)
        {
            Lasers = GameObject.Find("Lasers");
            if (Lasers == null)
            {
                Lasers = new GameObject
                {
                    name = "Lasers"
                };
            }
            //1.5�뷢��һ���ӵ�
            if (fireFrequencyTimer > 1.5f)
            {
                fireFrequencyTimer = 0;
                GameObject newLaser = Instantiate(laser);
                newLaser.name = "-Laser";
                newLaser.transform.SetParent(Lasers.transform, false);
                newLaser.transform.position = transform.position;
            }

            //2�����һ���ƶ�����
            if (flyTimer > limitTime)
            {
                flyTimer = 0;

                //�߳�ִ�и�ǿ�ȼ���
                Task.Run(() =>
                {
                    // ������ִ�к�ʱ�ļ�������
                    //������UFO��λ�ã����ܻ������ƶ�·��
                    float xx_UFO = GetEnemyPositon().x + UnityEngine.Random.Range(-2f * safeDistance, 2f * safeDistance);
                    float yy_UFO = GetEnemyPositon().y - GetEnemyDirection().y / Mathf.Abs(GetEnemyDirection().y) *
                                                                            Mathf.Sqrt(2f * safeDistance * 2f * safeDistance - xx_UFO * xx_UFO);
                    newEnemyPositon = new(xx_UFO, yy_UFO, 0);
                    forceDirection = newEnemyPositon - transform.position;
                    //Debug.Log("============" + forceDirection);
                });
                
            }
            else
            {
                if (forceDirection == Vector2.zero)
                {
                    forceDirection = new(-GetEnemyDirection().x, -GetEnemyDirection().y);
                }
                //Debug.Log("===" + forceDirection);
                //�����ƶ�
                rd.velocity = (Global.MaxSpeed * forceDirection.normalized);
            }
        }
        else
        {
            //��ʼʱ�������UFO
            forceDirection = GetEnemyDirection().normalized;
            rd.velocity = (3f *Global.MaxSpeed * forceDirection.normalized);
            //transform.Translate(Global.MaxSpeed * Time.deltaTime * forceDirection);
        }
        
    }

    //�������
    Vector3 GetEnemyPositon()
    {
        return ufo.position;
    }

    //���Ŀ�����
    float GetEnemyDistance()
    {
        return (GetEnemyPositon() - transform.position).magnitude;
    }

    //��ҵķ���
    Vector3 GetEnemyDirection()
    {
        Vector3 enemyDirection = GetEnemyPositon() - transform.position;
        return enemyDirection;
    }
}
