/*
 * 全局静态变量，方便脚本之间使用的变量
 */
using UnityEngine;

public class Global
{
    
    public static bool gameStart = false;//游戏是否开始

    public static int level = 0;//第几关

    public static float MaxSpeed = 5f;//敌人移动速度

    public static float UFOSpeed = 4.6f;//UFO移动速度
    public static float UFOMaxSpeed = 9f;//UFO加速时最大速度

    public static float LaserSpeed = 12f;//子弹速度

    public static int TheSpacePirateNumber = 0;//敌人数量

    public static float TheFireFrequency = 0.4f;//发射子弹的频率

    public static int AffectedTimes = 0;//UFO受击次数

    public static int InitialCount = 300;//场景中每样道具的数量

    public static int Times;//游戏剩余挑战次数
    //public enum UFO_State
    //{
    //    Idel,
    //    Manual
    //}
    //public static UFO_State ufo_State;
}
