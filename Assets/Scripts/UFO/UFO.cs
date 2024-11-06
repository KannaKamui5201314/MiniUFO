/*
 * 玩家UFO控制器
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UFO : MonoBehaviour
{
    public Vector2 direction = new(1f, 0f);//移动方向
    public Vector2 fireDirection = new(0f, 1f);//子弹发射方向

    public Joystick moveJoystick;//移动摇杆
    public Joystick fireJoystick;//开火方向摇杆

    private float moveSpeed = 0f;//移动速度

    GameObject laser;//子弹
    GameObject Lasers;//子弹父物体
    Transform cannon;//炮台
    public Transform GunMuzzle;//炮口

    private float fireFrequencyTimer;//发射子弹频率定时器

    Canvas canvas;
    UIController uiController;

    private Rigidbody2D ufoRigidbody2D;

    GameObject eventSystem;
    GameController gameController;



    void Start()
    {
        ufoRigidbody2D = GetComponent<Rigidbody2D>();

        laser = Resources.Load<GameObject>("Prefabs/Laser_1");

        canvas = FindObjectOfType<Canvas>();
        uiController = canvas.GetComponent<UIController>();

        cannon = transform.Find("cannon");
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();
    }

    void Update()
    {
        if (Global.gameStart)
        {
            Move();
            Fire();
            if (Global.TheSpacePirateNumber == 0)
            {
                Global.AffectedTimes = 0;
            }
        }
    }

    //角色移动，左摇杆控制
    void Move()
    {
        moveSpeed = Global.UFOSpeed;

        if (moveJoystick.Horizontal != 0 && moveJoystick.Vertical != 0)
        {
            direction = new(moveJoystick.Horizontal, moveJoystick.Vertical);
        }

        if (direction != Vector2.zero)
        {
            transform.up = direction;//UFO的上方始终跟随摇杆方向
        }
        ufoRigidbody2D.velocity = (moveSpeed * direction.normalized);
    }

    //开火方向，右摇杆控制
    void Fire()
    {
        if (fireJoystick.Horizontal != 0 && fireJoystick.Vertical != 0)
        {
            fireDirection = new(fireJoystick.Horizontal, fireJoystick.Vertical);
        }
        if (fireDirection != Vector2.zero)
        {
            cannon.up = fireDirection;
        }

        fireFrequencyTimer += Time.deltaTime;

        //发射子弹
        if (fireFrequencyTimer > Global.TheFireFrequency && uiController.IsFire())
        {
            fireFrequencyTimer = 0;
            Lasers = GameObject.Find("Lasers");
            if (Lasers == null)
            {
                Lasers = new GameObject
                {
                    name = "Lasers"
                };
            }
            GameObject newLaser = Instantiate(laser);//实例化子弹
            newLaser.name = "+Laser";
            newLaser.transform.SetParent(Lasers.transform, false);
            newLaser.transform.position = GunMuzzle.position;
        }
    }

    //UFO碰撞体检测响应事件
    //collision为进入UFO碰撞体的碰撞体
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 玩家与墙发生碰撞，停止移动
            moveSpeed = 0f;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // UFO与星盗船发生碰撞，
            uiController.SetBlood(5);
        }
    }

    //UFO触发器检测响应事件
    //UFO无敌状态触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // UFO与敌人发生碰撞
            gameController.PlayAudio();
            collision.gameObject.transform.parent = null;
            //回收敌人
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }
    }
}
