/*
 * 黑洞中心
 * 有一个范围，进入黑洞中心，必死
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

    //检测进入黑洞中心的物体，只要进入就会被消灭
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            uiController.SetBlood(5);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.parent = null;
            //回收
            gameController.objectPool.ReturnObject(collision.gameObject);
            Global.TheSpacePirateNumber -= 1;
        }

        if (collision.gameObject.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
        }

        //如果道具刷新在黑洞中心区域，会重新刷新此道具的位置
        if (collision.gameObject.CompareTag("prop"))
        {
            collision.transform.position = new Vector3(
            Random.Range(-500f, 500f), // x 坐标在 -10 到 10 之间随机
            Random.Range(-500f, 500f), // y 坐标在 -10 到 10 之间随机
            0f
            );
        }
    }
}
