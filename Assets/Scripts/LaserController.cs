/*
 * �ӵ�������
 */
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    GameObject ufo;
    private Transform ufoTransform;

    Vector2 forceDirection = new(0,1);//�ӵ����з���

    UFO ufoScript;
    float timer;//�ӵ��Իٶ�ʱ��

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
            //���˷�����ӵ��ķ���
            forceDirection = ufoTransform.position - transform.position;
        }
        else
        {
            //UFO������ӵ��ķ���
            forceDirection = ufoScript.fireDirection; 
        }
        forceDirection.Normalize();//����������һ��
    }

    void Update()
    {
        //�ӵ�����4�����Ի�
        timer += Time.deltaTime;
        if (timer > 4f)
        {
            Destroy(this.gameObject);
        }
        //�ӵ��ƶ�
        transform.Translate(Global.LaserSpeed * Time.deltaTime * forceDirection);
    }

    //�ӵ����������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���˵��ӵ�
        if (this.gameObject.name == "-Laser")
        {
            //���ӵ��������UFO�޵�״̬����ƣ�UFO������״̬��Ϊ�޵У�ʱ���ӵ�������������
            if (collision.CompareTag("Player") && collision.isTrigger)
            {
                Destroy(this.gameObject);
            }
            //���ӵ��������UFOʱ���ӵ��Ի٣���ҵ�һ��Ѫ
            if (collision.CompareTag("Player") && !collision.isTrigger)
            {

                uiController.SetBlood(1);
                Destroy(this.gameObject);
            }
        }
        //��ҵ��ӵ�
        if (this.gameObject.name == "+Laser")
        {
            //���е���
            if (collision.CompareTag("Enemy"))
            {
                gameController.PlayAudio();
                collision.gameObject.transform.parent = null;

                //ReturnObject����ػ��յ��ˣ��Ա㸴��
                gameController.objectPool.ReturnObject(collision.gameObject);
                Global.TheSpacePirateNumber -= 1;
                Destroy(this.gameObject);
            }
        }
    }
}
