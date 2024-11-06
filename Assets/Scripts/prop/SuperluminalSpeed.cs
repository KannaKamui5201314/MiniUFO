/*
 * 道具：加速
 * 简介：使UFO速度加大
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperluminalSpeed : MonoBehaviour
{
    GameObject _gameObject;

    bool isSuperluminalSpeed;//是否是加速状态

    float timer;

    PropController propController;

    private void OnEnable()
    {
        isSuperluminalSpeed = false;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }
    void Start()
    {
        propController = GetComponentInParent<Transform>().GetComponentInParent<PropController>();
    }

    void Update()
    {
        propController.SetRenderer(this.gameObject.GetComponent<SpriteRenderer>(), isSuperluminalSpeed);

        if (isSuperluminalSpeed && Global.gameStart)
        {
            timer += Time.deltaTime;
            //UFO加速3秒，速度为13
            if (timer < 3f)
            {
                if (_gameObject.CompareTag("Player"))
                {
                    if (Global.UFOSpeed < 13f)
                    {
                        //加速
                        Global.UFOSpeed = 13f;
                    }
                }
            }
            else
            {
                timer = 0f;
                isSuperluminalSpeed = false;
                //减速依靠加速键未按状态。
                propController.SuperluminalSpeedObjectPool.ReturnObject(this.gameObject);
                
            }
            
        }
    }

    //拾取道具
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isSuperluminalSpeed = true;
            _gameObject = collision.gameObject;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;//道具被拾取时，道具自身的碰撞器失效，避免重复触发拾取效果
        }
    }
}
