/*
 * 道具：星空卫队的导弹
 * 简介：自动追击排在第一位的敌人，击杀后继续追击下一个敌人
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Lissile : MonoBehaviour
{
    readonly float speed = 13f;//导弹追击速度

    GameObject eventSystem;
    GameController gameController;

    GameObject theSpacePirates;//敌人的父物体容器

    void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();

        theSpacePirates = GameObject.FindGameObjectWithTag("TheSpacePirates");
    }

    void Update()
    {
        if (theSpacePirates.transform.childCount > 0)
        {
            //追击第一个敌人，击杀后依然追击排在第一位的敌人
            Vector2 direction = theSpacePirates.transform.GetChild(0).position - transform.position;
            transform.Translate(speed * Time.deltaTime * direction.normalized);
        }
        //全部击杀后，自毁
        if (theSpacePirates.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    //击中敌人
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameController.PlayAudio();
            collision.gameObject.transform.parent = null;
            //回收
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }
    }
}
