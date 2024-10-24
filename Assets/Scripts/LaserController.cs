using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

//子弹控制
public class LaserController : MonoBehaviour
{
    GameObject ufo;
    private Transform ufoTransform;

    Vector2 forceDirection = new(0,1);

    UFO ufoScript;
    float timer;

    GameObject eventSystem;
    GameController gameController;

    Canvas canvas;
    UIController uiController;


    // Start is called before the first frame update
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

        //forceDirection运动方向
        if (this.gameObject.name == "-Laser")
        {
            forceDirection = ufoTransform.position - transform.position;
        }
        else
        {
            forceDirection = ufoScript.fireDirection; 
        }
        forceDirection.Normalize();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 4f)
        {
            Destroy(this.gameObject);
        }
        
        transform.Translate(Global.LaserSpeed * Time.deltaTime * forceDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.name == "-Laser")
        {
            if (collision.CompareTag("Player") && collision.isTrigger)
            {
                Destroy(this.gameObject);
            }

            if (collision.CompareTag("Player") && !collision.isTrigger)
            {

                uiController.SetBlood(1);
                Destroy(this.gameObject);
            }
        }
        if (this.gameObject.name == "+Laser")
        {
            if (collision.CompareTag("Enemy"))
            {
                gameController.PlayAudio();
                collision.gameObject.transform.parent = null;
                //回收
                gameController.objectPool.ReturnObject(collision.gameObject);
                Global.TheSpacePirateNumber -= 1;
                Destroy(this.gameObject);
            }
        }
    }
}
