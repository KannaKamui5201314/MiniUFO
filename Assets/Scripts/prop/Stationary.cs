using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stationary : MonoBehaviour
{
    GameObject _gameObject;

    bool isStationary;

    float timer;

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

        if (isStationary && Global.gameStart)
        {
            timer += Time.deltaTime;
            if (timer < 1f)
            {
                if (_gameObject.CompareTag("Player"))
                {
                    //¾²Ö¹
                    Global.UFOSpeed = 0f;
                }
            }
            else
            {
                timer = 0f;
                isStationary = false;
                Global.UFOSpeed = 4.6f;
                propController.StationaryObjectPool.ReturnObject(this.gameObject);
            }

        }
    }
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
