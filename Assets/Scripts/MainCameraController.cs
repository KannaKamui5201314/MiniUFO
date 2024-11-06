/*
 * �����������
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Camera MainCamera;
    private Transform ufoTransform; // ufo

    Transform SpriteMask;
    float cun;//��ǰ��Ļ�Խ��ߵĳ���
    void Start()
    {
        MainCamera = GetComponent<Camera>();
        ufoTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SetSize();
        SpriteMask = this.transform.Find("SpriteMask");
        cun = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height)/100f;
        //Debug.Log(SpriteMask.localScale);
        //��̬�޸�Բ�����ֵ�ֱ��Ϊcun + 1f
        SpriteMask.localScale = new(cun + 1f, cun + 1f, 1f);
    }

    void Update()
    {
        if (Global.gameStart)
        {
            Move();
        }
    }

    //���������С����Ļ��Сһ��  set  Camera & Screen Size same
    void SetSize()
    {
        if (MainCamera.orthographic)
        {
            
            MainCamera.orthographicSize = Screen.height / 2 / 100f;
        }
    }

    //������� UFO�ƶ�
    void Move()
    {
        transform.position = new Vector3(ufoTransform.position.x, ufoTransform.position.y, -10f);
    }
}
