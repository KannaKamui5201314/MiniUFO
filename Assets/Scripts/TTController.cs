/*
 * TTSDK控制器
 * TTSDK是抖音平台推出的集广告、登录、侧边栏、排行榜等功能为一体的程序包
 * 由于未上线，目前只能测试登录功能，排行榜广告侧边栏功能斗无法测试。
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TTSDK;
using UnityEngine;


public class TTController : MonoBehaviour
{
    
    private void Awake()
    {
        //使用TTSDK时必须先初始化TTSDK
        TT.InitSDK(OnTTContainerInit);
    }

    //TTSDK初始化回调
    private void OnTTContainerInit(int code, ContainerEnv env)
    {
        //code为0代表初始化成功
        if (code == 0)
        {
            //检测实名完成
            //TT.SetRealNameAuthenticationCallback(onRealNameAuthenticationSuccess);
            //检测是否需要重新登录
            // TT.CheckSession检测登录信息是否失效
            //TT.CheckScene检查是否支持抖音侧边栏
            TT.CheckSession(OnCheckSessionSuccess, OnCheckSessionFailed);
            TT.CheckScene(TTSideBar.SceneEnum.SideBar, CheckSceneSuccess, CheckSceneComplete, CheckSceneError);
        }
    }

    private void CheckSceneSuccess(bool obj)
    {
        
    }

    private void CheckSceneComplete()
    {
        
    }

    private void CheckSceneError(int arg1, string arg2)
    {
        
    }

    private void OnCheckSessionSuccess()
    {
        //获取玩家信息
        TT.GetUserInfo(false, true, OnGetUserInfoSuccess, OnGetUserInfoFailed);
    }
    private void OnCheckSessionFailed(string errMsg)
    {
        //玩家登录
        TT.Login(OnLoginSuccess, OnLoginFailed);
    }

    //登录成功回调
    private void OnLoginSuccess(string code, string anonymousCode, bool islogin)
    {
        if (islogin)
        {
            TT.GetUserInfo(false, true, OnGetUserInfoSuccess, OnGetUserInfoFailed);
        }
    }

    private void OnLoginFailed(string errMsg)
    {
        TT.Login(OnLoginSuccess, OnLoginFailed);
    }



    private void OnGetUserInfoSuccess(ref TTUserInfo scUserInfo)
    {
        Debug.Log("scUserInfo=" + scUserInfo);
    }

    private void OnGetUserInfoFailed(string errMsg)
    {
        
    }

    private void OnAuthenticateRealNameSuccess(string errMsg)
    {
        
    }
    private void OnAuthenticateRealNameFailed(string errMsg)
    {
        //TT.AuthenticateRealName(OnAuthenticateRealNameSuccess, OnAuthenticateRealNameFailed);
    }

    private void OnRealNameAuthenticationSuccess()
    {
       //实名认证完成后的操作
    }


    //上面是初始化，登录，实名认证


}
