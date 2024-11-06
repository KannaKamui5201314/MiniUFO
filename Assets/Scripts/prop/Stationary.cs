/*
 * ���ߣ���ֹ
 * ��飺ʰȡʱ��ʹ���UFO 1�����޷��ƶ�
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stationary : MonoBehaviour
{
    GameObject _gameObject;

    bool isStationary;

    float timer;//��ֹʱ�䶨ʱ��

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

        //һ���ڣ�UFO�޷��ƶ�
        if (isStationary && Global.gameStart)
        {
            timer += Time.deltaTime;
            if (timer < 1f)
            {
                if (_gameObject.CompareTag("Player"))
                {
                    //��ֹ
                    Global.UFOSpeed = 0f;
                }
            }
            else
            {
                //UFO�ָ���ֹǰ��״̬
                timer = 0f;
                isStationary = false;
                Global.UFOSpeed = 4.6f;
                propController.StationaryObjectPool.ReturnObject(this.gameObject);
            }

        }
    }

    //ʰȡ����
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
