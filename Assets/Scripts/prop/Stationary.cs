/*
 * 道具：静止
 * 简介：拾取时，使玩家UFO 1秒内无法移动
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stationary : MonoBehaviour
{
    GameObject _gameObject;

    bool isStationary;

    float timer;//静止时间定时器

    PropController propController;

    private void OnEnable()
    {
        isStationary = false;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    void Start()
    {
        propController = GetComponentInParent<Transform>().GetComponentInParent<PropController>();
    }

    void Update()
    {
        propController.SetRenderer(this.gameObject.GetComponent<SpriteRenderer>(), isStationary);

        //一秒内，UFO无法移动
        if (isStationary && Global.gameStart)
        {
            timer += Time.deltaTime;
            if (timer < 1f)
            {
                if (_gameObject.CompareTag("Player"))
                {
                    //静止
                    Global.UFOSpeed = 0f;
                }
            }
            else
            {
                //UFO恢复静止前的状态
                timer = 0f;
                isStationary = false;
                Global.UFOSpeed = 4.6f;
                propController.StationaryObjectPool.ReturnObject(this.gameObject);
            }

        }
    }

    //拾取道具
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isStationary = true;
            _gameObject = collision.gameObject;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
