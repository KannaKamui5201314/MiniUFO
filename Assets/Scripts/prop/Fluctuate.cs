/*
 * 道具：宇宙波动（宇宙级灾难）
 * 简介：横向长度为1，竖向长度为1000，从坐标（-500，0）向右横扫整个游戏场景
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluctuate : MonoBehaviour
{
    private readonly float speed = 5f;//道具移动速度

    GameObject eventSystem;
    GameController gameController;

    Canvas canvas;
    UIController uiController;

    //PropController propController;
    
    void Start()
    {
        //propController = GetComponentInParent<Transform>().GetComponentInParent<PropController>();

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();

        canvas = FindObjectOfType<Canvas>();
        uiController = canvas.GetComponent<UIController>();
    }

    void Update()
    {
        //超出地图范围时自毁
        if (transform.position.x>500f || !Global.gameStart)
        {
            Destroy(this.gameObject);
        }
        if (Global.gameStart)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.right);//道具移动
        }
        
    }

    //检测道具命中的物体
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger)
        {
            uiController.SetBlood(3);//玩家触碰道具时掉3滴血
        }

        //敌人触碰道具时直接被毁灭
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameController.PlayAudio();
            collision.gameObject.transform.parent = null;
            //回收
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }

        if (collision.gameObject.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
        }
    }
}
