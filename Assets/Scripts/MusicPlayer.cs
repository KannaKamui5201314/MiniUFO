/*
 * 背景音乐播放控制器
 * （切换音乐时，存在卡顿现象，已弃用）
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    //public AudioSource musicSource1;
    //public AudioSource musicSource2;
    //private bool isPlayingMusic1 = true;

    void Start()
    {
       // musicSource1.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!musicSource1.isPlaying && isPlayingMusic1)
        //{
        //    musicSource1.Stop();
        //    musicSource2.Play();
        //    isPlayingMusic1 = false;
        //}
        //else if (!musicSource2.isPlaying && !isPlayingMusic1)
        //{
        //    musicSource2.Stop();
        //    musicSource1.Play();
        //    isPlayingMusic1 = true;
        //}
    }
}
