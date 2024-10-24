using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TheSpacePirateController : MonoBehaviour
{

    readonly float safeDistance = Screen.width/100f * 0.5f;//1920分辨率下大约是10

    private Transform ufo;

    private float limitTime;
    private float flyTimer;
    private float fireFrequencyTimer;

    Vector3 newEnemyPositon;

    Vector2 forceDirection;

    GameObject laser;
    GameObject Lasers;

    private Rigidbody2D rd;

    void Start()
    {
        laser = Resources.Load<GameObject>("Prefabs/Laser");
        
        limitTime = 2f;//15/6=2.5s
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

    void Move()
    {
        flyTimer += Time.deltaTime;
        fireFrequencyTimer += Time.deltaTime;
        //Debug.Log("safeDistance=" + safeDistance);
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
            //1.5秒发射一次子弹
            if (fireFrequencyTimer > 1.5f)
            {
                fireFrequencyTimer = 0;
                GameObject newLaser = Instantiate(laser);
                newLaser.name = "-Laser";
                newLaser.transform.SetParent(Lasers.transform, false);
                newLaser.transform.position = transform.position;
            }

            //2.5秒更换一次移动方向
            if (flyTimer > limitTime)
            {
                flyTimer = 0;

                //线程执行高强度计算
                Task.Run(() =>
                {
                    // 在这里执行耗时的计算任务
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
                rd.velocity = (Global.MaxSpeed * forceDirection.normalized);
            }
        }
        else
        {
            //冲向UFO
            forceDirection = GetEnemyDirection().normalized;
            rd.velocity = (3f *Global.MaxSpeed * forceDirection.normalized);
            //transform.Translate(Global.MaxSpeed * Time.deltaTime * forceDirection);
        }
        
    }

    //敌人坐标
    Vector3 GetEnemyPositon()
    {
        return ufo.position;
    }

    //敌人距离
    float GetEnemyDistance()
    {
        return (GetEnemyPositon() - transform.position).magnitude;
    }

    //敌人方向
    Vector3 GetEnemyDirection()
    {
        Vector3 enemyDirection = GetEnemyPositon() - transform.position;
        return enemyDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
