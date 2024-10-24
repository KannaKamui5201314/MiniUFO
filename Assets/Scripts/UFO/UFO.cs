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
    public Vector2 direction = new(1f, 0f);
    public Vector2 fireDirection = new(0f, 1f);

    public Joystick moveJoystick;
    public Joystick fireJoystick;

    private float moveSpeed = 0f;

    GameObject laser;
    GameObject Lasers;
    Transform cannon;
    public Transform GunMuzzle;

    private float fireFrequencyTimer;

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

    void Move()
    {
        moveSpeed = Global.UFOSpeed;

        if (moveJoystick.Horizontal != 0 && moveJoystick.Vertical != 0)
        {
            direction = new(moveJoystick.Horizontal, moveJoystick.Vertical);
        }

        if (direction != Vector2.zero)
        {
            transform.up = direction;
        }
        ufoRigidbody2D.velocity = (moveSpeed * direction.normalized);
    }

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
            GameObject newLaser = Instantiate(laser);
            newLaser.name = "+Laser";
            newLaser.transform.SetParent(Lasers.transform, false);
            newLaser.transform.position = GunMuzzle.position;
        }
    }


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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // UFO与星盗船发生碰撞，
            gameController.PlayAudio();
            collision.gameObject.transform.parent = null;
            //回收
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }
    }
}
