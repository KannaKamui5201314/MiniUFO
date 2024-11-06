/*
 * ���UFO������
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
    public Vector2 direction = new(1f, 0f);//�ƶ�����
    public Vector2 fireDirection = new(0f, 1f);//�ӵ����䷽��

    public Joystick moveJoystick;//�ƶ�ҡ��
    public Joystick fireJoystick;//������ҡ��

    private float moveSpeed = 0f;//�ƶ��ٶ�

    GameObject laser;//�ӵ�
    GameObject Lasers;//�ӵ�������
    Transform cannon;//��̨
    public Transform GunMuzzle;//�ڿ�

    private float fireFrequencyTimer;//�����ӵ�Ƶ�ʶ�ʱ��

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

    //��ɫ�ƶ�����ҡ�˿���
    void Move()
    {
        moveSpeed = Global.UFOSpeed;

        if (moveJoystick.Horizontal != 0 && moveJoystick.Vertical != 0)
        {
            direction = new(moveJoystick.Horizontal, moveJoystick.Vertical);
        }

        if (direction != Vector2.zero)
        {
            transform.up = direction;//UFO���Ϸ�ʼ�ո���ҡ�˷���
        }
        ufoRigidbody2D.velocity = (moveSpeed * direction.normalized);
    }

    //��������ҡ�˿���
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

        //�����ӵ�
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
            GameObject newLaser = Instantiate(laser);//ʵ�����ӵ�
            newLaser.name = "+Laser";
            newLaser.transform.SetParent(Lasers.transform, false);
            newLaser.transform.position = GunMuzzle.position;
        }
    }

    //UFO��ײ������Ӧ�¼�
    //collisionΪ����UFO��ײ�����ײ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // �����ǽ������ײ��ֹͣ�ƶ�
            moveSpeed = 0f;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // UFO���ǵ���������ײ��
            uiController.SetBlood(5);
        }
    }

    //UFO�����������Ӧ�¼�
    //UFO�޵�״̬����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // UFO����˷�����ײ
            gameController.PlayAudio();
            collision.gameObject.transform.parent = null;
            //���յ���
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }
    }
}
