/*
 * 子弹控制器
 */
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    GameObject ufo;
    private Transform ufoTransform;

    Vector2 forceDirection = new(0,1);//子弹飞行方向

    UFO ufoScript;
    float timer;//子弹自毁定时器

    GameObject eventSystem;
    GameController gameController;

    Canvas canvas;
    UIController uiController;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        uiController = canvas.GetComponent<UIController>();

        ufo = GameObject.FindGameObjectWithTag("Player");
        //moveJoystick = GameObject.FindGameObjectWithTag("JoyStick1");

        ufoTransform = ufo.GetComponent<Transform>();
        ufoScript = ufo.GetComponent<UFO>();

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();

        if (this.gameObject.name == "-Laser")
        {
            //敌人发射的子弹的方向
            forceDirection = ufoTransform.position - transform.position;
        }
        else
        {
            //UFO发射的子弹的方向
            forceDirection = ufoScript.fireDirection; 
        }
        forceDirection.Normalize();//方向向量归一化
    }

    void Update()
    {
        //子弹存在4秒后会自毁
        timer += Time.deltaTime;
        if (timer > 4f)
        {
            Destroy(this.gameObject);
        }
        //子弹移动
        transform.Translate(Global.LaserSpeed * Time.deltaTime * forceDirection);
    }

    //子弹触发器组件
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敌人的子弹
        if (this.gameObject.name == "-Laser")
        {
            //当子弹碰到玩家UFO无敌状态（设计：UFO触发器状态视为无敌）时，子弹会立即被消灭
            if (collision.CompareTag("Player") && collision.isTrigger)
            {
                Destroy(this.gameObject);
            }
            //当子弹击中玩家UFO时，子弹自毁，玩家掉一滴血
            if (collision.CompareTag("Player") && !collision.isTrigger)
            {

                uiController.SetBlood(1);
                Destroy(this.gameObject);
            }
        }
        //玩家的子弹
        if (this.gameObject.name == "+Laser")
        {
            //击中敌人
            if (collision.CompareTag("Enemy"))
            {
                gameController.PlayAudio();
                collision.gameObject.transform.parent = null;

                //ReturnObject对象池回收敌人，以便复用
                gameController.objectPool.ReturnObject(collision.gameObject);
                Global.TheSpacePirateNumber -= 1;
                Destroy(this.gameObject);
            }
        }
    }
}
