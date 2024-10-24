using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Camera MainCamera;
    private Transform ufoTransform; // ufo

    Transform SpriteMask;
    float cun;
    void Start()
    {
        MainCamera = GetComponent<Camera>();
        ufoTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SetSize();
        SpriteMask = GetComponentInChildren<Transform>();
        cun = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height)/100f;
        //Debug.Log(SpriteMask.localScale);
        //动态修改遮罩大小
        SpriteMask.localScale = new(cun + 1f, cun + 1f, 1f);
        //SpriteMask.localScale=new(SpriteMask.localScale.x *(cun + 1f), SpriteMask.localScale.y * (cun + 1f), SpriteMask.localScale.z);
    }

    void Update()
    {
        if (Global.gameStart)
        {
            Move();
        }
    }

    //设置相机大小和屏幕大小一致  set  Camera & Screen Size same
    void SetSize()
    {
        if (MainCamera.orthographic)
        {
            
            MainCamera.orthographicSize = Screen.height / 2 / 100f;
        }
    }

    //follow UFO
    void Move()
    {
        transform.position = new Vector3(ufoTransform.position.x, ufoTransform.position.y, -10f);
    }
}
