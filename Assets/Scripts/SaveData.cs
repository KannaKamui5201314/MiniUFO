/*
 * TTSDK�Ļ�����������SaveData
 * �����ݲ��־ã�δʹ�ã�
 */
using System;
using TTSDK;
using UnityEngine;

[Serializable]
public class SaveData
{
    public bool IsClicked_Sidebar;//�Ƿ����˶����������ť
    public int Times;//��ս����
    
    //name ���ݵ�����
    //TT.LoadSaving�ӻ����������
    //TT.Save�������ݵ�����
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
