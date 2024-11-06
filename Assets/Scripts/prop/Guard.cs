/*
 * 道具：星空卫队
 * 简介：玩家拾取道具后，道具会发出一个导弹，会逐个消灭敌人，直到开始下一关
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
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;//靠近UFO时激活道具
    }

    private void Awake()
    {
        Lissile = Resources.Load<GameObject>("Prefabs/prop/body/Lissile");//加载道具
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
        //拾取道具，拾取后，刷新位置，在新的位置重生
        if (collision.CompareTag("Player"))
        {
            GameObject obj = GameObject.Instantiate(Lissile);
            obj.transform.position = transform.position;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            propController.GuardObjectPool.ReturnObject(this.gameObject);
        }
    }
}
