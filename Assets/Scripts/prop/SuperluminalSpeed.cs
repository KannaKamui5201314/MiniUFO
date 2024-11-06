/*
 * ���ߣ�����
 * ��飺ʹUFO�ٶȼӴ�
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperluminalSpeed : MonoBehaviour
{
    GameObject _gameObject;

    bool isSuperluminalSpeed;//�Ƿ��Ǽ���״̬

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
            //UFO����3�룬�ٶ�Ϊ13
            if (timer < 3f)
            {
                if (_gameObject.CompareTag("Player"))
                {
                    if (Global.UFOSpeed < 13f)
                    {
                        //����
                        Global.UFOSpeed = 13f;
                    }
                }
            }
            else
            {
                timer = 0f;
                isSuperluminalSpeed = false;
                //�����������ټ�δ��״̬��
                propController.SuperluminalSpeedObjectPool.ReturnObject(this.gameObject);
                
            }
            
        }
    }

    //ʰȡ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isSuperluminalSpeed = true;
            _gameObject = collision.gameObject;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;//���߱�ʰȡʱ�������������ײ��ʧЧ�������ظ�����ʰȡЧ��
        }
    }
}
