/*
 * TTSDK的缓存数据类型SaveData
 * （数据不持久，未使用）
 */
using System;
using TTSDK;
using UnityEngine;

[Serializable]
public class SaveData
{
    public bool IsClicked_Sidebar;//是否点击了抖音侧边栏按钮
    public int Times;//挑战次数
    
    //name 数据的名称
    //TT.LoadSaving从缓存加载数据
    //TT.Save保存数据到缓存
    public SaveData Load(string name)
    {
        SaveData loaded = TT.LoadSaving<SaveData>(name);
        if (loaded == null)
        {
            loaded = new SaveData();
            IsClicked_Sidebar = false;
            Times = 5;
            TT.Save<SaveData>(loaded, name);
        }
        return loaded;
    }
}
