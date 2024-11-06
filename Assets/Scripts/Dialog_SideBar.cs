/*
 * 功能：抖音侧边栏
 * 内容：对话框有一个取消按钮和一个确认按钮，取消就退出对话框，确认就打开抖音侧边栏。
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TTSDK;
using TTSDK.UNBridgeLib.LitJson;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_SideBar : MonoBehaviour
{
    Button Left;//取消按钮
    Button Right;//确认按钮

    GameObject eventSystem;
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        gameController = eventSystem.GetComponent<GameController>();

        Left = transform.Find("Left").GetComponent<Button>();
        Right = transform.Find("Right").GetComponent<Button>();
        Left.onClick.AddListener(LeftOnClick);
        Right.onClick.AddListener(RightOnClick);
    }

    //退出对话框
    private void LeftOnClick()
    {
        this.gameObject.SetActive(false);
    }

    //打开抖音侧边栏
    private void RightOnClick()
    {
        this.gameObject.SetActive(false);
        Global.Times = 5;
        TT.PlayerPrefs.SetString("Times", Global.Times.ToString());
        TT.PlayerPrefs.SetString("IsClicked_Sidebar", "1");
        
        TT.CheckScene(TTSideBar.SceneEnum.SideBar, CheckSceneSuccess, CheckSceneComplete, CheckSceneError);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TT.CheckScene回调
    private void CheckSceneSuccess(bool obj)
    {
        if (obj)
        {
            JsonData data = new JsonData();
            data["scene"] = "sidebar";

            //打开抖音侧边栏
            TT.NavigateToScene(data, () =>
            {
                Debug.Log("navigate to scene success");
            }, () =>
            {
                Debug.Log("navigate to scene complete");
            }, (errCode, errMsg) =>
            {
                Debug.Log($"navigate to scene error, errCode:{errCode}, errMsg:{errMsg}");
            });
        }
    }

    private void CheckSceneComplete()
    {

    }

    private void CheckSceneError(int arg1, string arg2)
    {

    }
}
