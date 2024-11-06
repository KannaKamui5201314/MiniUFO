/*
 * �ڶ�����
 * ��һ����Χ������ڶ����ģ�����
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleCenter : MonoBehaviour
{
    GameObject eventSystem;
    GameController gameController;

    Canvas canvas;
    UIController uiController;
    
    void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();

        canvas = FindObjectOfType<Canvas>();
        uiController = canvas.GetComponent<UIController>();
    }

    //������ڶ����ĵ����壬ֻҪ����ͻᱻ����
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            uiController.SetBlood(5);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.parent = null;
            //����
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }

        if (collision.gameObject.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
        }

        //�������ˢ���ںڶ��������򣬻�����ˢ�´˵��ߵ�λ��
        if (collision.gameObject.CompareTag("prop"))
        {
            collision.transform.position = new Vector3(
            Random.Range(-500f, 500f), // x ������ -10 �� 10 ֮�����
            Random.Range(-500f, 500f), // y ������ -10 �� 10 ֮�����
            0f
            );
        }
    }
}
