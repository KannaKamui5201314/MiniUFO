/*
 * 道具：无敌
 * 简介：设置UFO为触发器状态，使敌人和宇宙波动检测不到UFO，
 *              无敌状态UFO碰撞敌人会消灭敌人，也可以安全穿过宇宙波动
 */
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    GameObject _gameObject;

    bool isInvincible;

    float timer;

    PropController propController;

    GameObject UFO;
    Transform UFOTransform;
    Transform guard;//UFO周边的护卫舰

    private float speedguard = 120f; // 护卫舰角速度增量
    private float angleguard = 0f; //护卫舰角速度

    private void OnEnable()
    {
        isInvincible = false;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }
   
    void Start()
    {

        propController = GetComponentInParent<Transform>().GetComponentInParent<PropController>();
        UFO = GameObject.FindGameObjectWithTag("Player");
        UFOTransform = UFO.transform;
        guard = UFOTransform.Find("guard");
    }

    void Update()
    {
        propController.SetRenderer(this.gameObject.GetComponent<SpriteRenderer>(), isInvincible);
        // 以固定速度增加角度
        angleguard += speedguard;

        // 计算圆形轨迹上的位置
        float xguard = Mathf.Sin(angleguard);
        float yguard = Mathf.Cos(angleguard);

        if (isInvincible && Global.gameStart)
        {
            timer += Time.deltaTime;
            if (timer < 5f)
            {
                if (_gameObject.CompareTag("Player"))
                {
                    //ufo无敌状态
                    UFO.GetComponent<CircleCollider2D>().isTrigger = true;
                    Global.UFOSpeed = 8f;
                    //护卫舰围绕UFO旋转移动，形成类似圆形护盾保护中心UFO
                    guard.up = new(xguard, yguard);

                }
            }
            else
            {
                timer = 0f;
                isInvincible = false;

                UFO.GetComponent<CircleCollider2D>().isTrigger = false;
                guard.up = new(0, 0);
                propController.InvincibleObjectPool.ReturnObject(this.gameObject);

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //拾取道具
        if (collision.CompareTag("Player"))
        {
            isInvincible = true;
            _gameObject = collision.gameObject;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
