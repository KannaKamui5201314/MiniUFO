/*
 * 敌人控制器
 * 主要是控制移动
 */
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TheSpacePirateController : MonoBehaviour
{

    readonly float safeDistance = Screen.width/100f * 0.5f;//屏幕宽度的一半，横屏1920分辨率下大约是10单位

    private Transform ufo;//玩家

    private float limitTime = 2f;//敌人每2秒改变移动方向
    private float flyTimer;//敌人移动时间定时器
    private float fireFrequencyTimer;//敌人开火定时器

    Vector3 newEnemyPositon;

    Vector2 forceDirection;//目标方向

    GameObject laser;//子弹
    GameObject Lasers;//子弹父物体

    private Rigidbody2D rd;//敌人刚体组件

    //引用
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

    //敌人移动控制
    void Move()
    {
        flyTimer += Time.deltaTime;
        fireFrequencyTimer += Time.deltaTime;
        //Debug.Log("safeDistance=" + safeDistance);
        //敌人和玩家UFO的距离小于屏幕宽，会智能化移动
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

            //2秒更换一次移动方向
            if (flyTimer > limitTime)
            {
                flyTimer = 0;

                //线程执行高强度计算
                Task.Run(() =>
                {
                    // 在这里执行耗时的计算任务
                    //随机玩家UFO的位置，智能化敌人移动路径
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
                //敌人移动
                rd.velocity = (Global.MaxSpeed * forceDirection.normalized);
            }
        }
        else
        {
            //开始时冲向玩家UFO
            forceDirection = GetEnemyDirection().normalized;
            rd.velocity = (3f *Global.MaxSpeed * forceDirection.normalized);
            //transform.Translate(Global.MaxSpeed * Time.deltaTime * forceDirection);
        }
        
    }

    //玩家坐标
    Vector3 GetEnemyPositon()
    {
        return ufo.position;
    }

    //玩家目标距离
    float GetEnemyDistance()
    {
        return (GetEnemyPositon() - transform.position).magnitude;
    }

    //玩家的方向
    Vector3 GetEnemyDirection()
    {
        Vector3 enemyDirection = GetEnemyPositon() - transform.position;
        return enemyDirection;
    }
}
