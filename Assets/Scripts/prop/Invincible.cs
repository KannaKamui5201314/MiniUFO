/*
 * ���ߣ��޵�
 * ��飺����UFOΪ������״̬��ʹ���˺����沨����ⲻ��UFO��
 *              �޵�״̬UFO��ײ���˻�������ˣ�Ҳ���԰�ȫ�������沨��
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
    Transform guard;//UFO�ܱߵĻ�����

    private float speedguard = 120f; // ���������ٶ�����
    private float angleguard = 0f; //���������ٶ�

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
        // �Թ̶��ٶ����ӽǶ�
        angleguard += speedguard;

        // ����Բ�ι켣�ϵ�λ��
        float xguard = Mathf.Sin(angleguard);
        float yguard = Mathf.Cos(angleguard);

        if (isInvincible && Global.gameStart)
        {
            timer += Time.deltaTime;
            if (timer < 5f)
            {
                if (_gameObject.CompareTag("Player"))
                {
                    //ufo�޵�״̬
                    UFO.GetComponent<CircleCollider2D>().isTrigger = true;
                    Global.UFOSpeed = 8f;
                    //������Χ��UFO��ת�ƶ����γ�����Բ�λ��ܱ�������UFO
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
        //ʰȡ����
        if (collision.CompareTag("Player"))
        {
            isInvincible = true;
            _gameObject = collision.gameObject;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
