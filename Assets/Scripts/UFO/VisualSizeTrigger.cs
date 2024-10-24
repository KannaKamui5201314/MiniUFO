using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisualSizeTrigger : MonoBehaviour
{
    private BoxCollider2D[] visualSize;
    // Start is called before the first frame update
    void Start()
    {
        SetVisualSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //可视屏幕大小
    void SetVisualSize()
    {
        visualSize = GetComponentsInChildren<BoxCollider2D>();
        visualSize[0].size = new Vector2(Screen.width / 2f / 100f - 0.2f, Screen.height / 2f / 100f - 0.2f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Laser"))
        //{
        //    Destroy(collision.gameObject);
        //}
    }
}
