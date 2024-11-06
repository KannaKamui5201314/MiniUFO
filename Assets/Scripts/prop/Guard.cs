/*
 * ���ߣ��ǿ�����
 * ��飺���ʰȡ���ߺ󣬵��߻ᷢ��һ�������������������ˣ�ֱ����ʼ��һ��
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    PropController propController;

    GameObject Lissile;

    private void OnEnable()
    {
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;//����UFOʱ�������
    }

    private void Awake()
    {
        Lissile = Resources.Load<GameObject>("Prefabs/prop/body/Lissile");//���ص���
        //Debug.Log(Lissile);
    }
    
    void Start()
    {
        propController = GetComponentInParent<Transform>().GetComponentInParent<PropController>();
    }

    void Update()
    {
        propController.SetRenderer(this.gameObject.GetComponent<SpriteRenderer>(), false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ʰȡ���ߣ�ʰȡ��ˢ��λ�ã����µ�λ������
        if (collision.CompareTag("Player"))
        {
            GameObject obj = GameObject.Instantiate(Lissile);
            obj.transform.position = transform.position;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            propController.GuardObjectPool.ReturnObject(this.gameObject);
        }
    }
}
