/*
 * 主相机控制器
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Camera MainCamera;
    private Transform ufoTransform; // ufo

    Transform SpriteMask;
    float cun;//当前屏幕对角线的长度
    void Start()
    {
        MainCamera = GetComponent<Camera>();
        ufoTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SetSize();
        SpriteMask = this.transform.Find("SpriteMask");
        cun = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height)/100f;
        //Debug.Log(SpriteMask.localScale);
        //动态修改圆形遮罩的直径为cun + 1f
        SpriteMask.localScale = new(cun + 1f, cun + 1f, 1f);
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

    //相机跟随 UFO移动
    void Move()
    {
        transform.position = new Vector3(ufoTransform.position.x, ufoTransform.position.y, -10f);
    }
}
