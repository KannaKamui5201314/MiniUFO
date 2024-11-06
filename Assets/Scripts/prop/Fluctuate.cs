/*
 * ���ߣ����沨�������漶���ѣ�
 * ��飺���򳤶�Ϊ1�����򳤶�Ϊ1000�������꣨-500��0�����Һ�ɨ������Ϸ����
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluctuate : MonoBehaviour
{
    private readonly float speed = 5f;//�����ƶ��ٶ�

    GameObject eventSystem;
    GameController gameController;

    Canvas canvas;
    UIController uiController;

    //PropController propController;
    
    void Start()
    {
        //propController = GetComponentInParent<Transform>().GetComponentInParent<PropController>();

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();

        canvas = FindObjectOfType<Canvas>();
        uiController = canvas.GetComponent<UIController>();
    }

    void Update()
    {
        //������ͼ��Χʱ�Ի�
        if (transform.position.x>500f || !Global.gameStart)
        {
            Destroy(this.gameObject);
        }
        if (Global.gameStart)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.right);//�����ƶ�
        }
        
    }

    //���������е�����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger)
        {
            uiController.SetBlood(3);//��Ҵ�������ʱ��3��Ѫ
        }

        //���˴�������ʱֱ�ӱ�����
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameController.PlayAudio();
            collision.gameObject.transform.parent = null;
            //����
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }

        if (collision.gameObject.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
        }
    }
}
