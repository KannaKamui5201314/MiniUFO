/*
 * ���ߣ��ǿ����ӵĵ���
 * ��飺�Զ�׷�����ڵ�һλ�ĵ��ˣ���ɱ�����׷����һ������
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Lissile : MonoBehaviour
{
    readonly float speed = 13f;//����׷���ٶ�

    GameObject eventSystem;
    GameController gameController;

    GameObject theSpacePirates;//���˵ĸ���������

    void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();

        theSpacePirates = GameObject.FindGameObjectWithTag("TheSpacePirates");
    }

    void Update()
    {
        if (theSpacePirates.transform.childCount > 0)
        {
            //׷����һ�����ˣ���ɱ����Ȼ׷�����ڵ�һλ�ĵ���
            Vector2 direction = theSpacePirates.transform.GetChild(0).position - transform.position;
            transform.Translate(speed * Time.deltaTime * direction.normalized);
        }
        //ȫ����ɱ���Ի�
        if (theSpacePirates.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    //���е���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameController.PlayAudio();
            collision.gameObject.transform.parent = null;
            //����
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }
    }
}
