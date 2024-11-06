/*
 * �������
 * ���������ɶ����ͷŶ��󣬻��ն���
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;//Ԥ����
    private List<GameObject> pool;//�����б�

    //���캯�������������
    //prefab Ԥ����
    //initialCount ʵ�������������
    public ObjectPool(GameObject prefab, int initialCount)
    {
        this.prefab = prefab;
        pool = new List<GameObject>();
        for (int i = 0; i < initialCount; i++)
        {
            //ʵ��������
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);//����ʧ��
            pool.Add(obj);
        }
    }

    //�ͷŶ���
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            obj.SetActive(true);//���ö���
            return obj;
        }
        else
        {
            //�������û�ж���ʱ��ʵ����һ������
            return GameObject.Instantiate(prefab);
        }
    }

    //���ն���
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Add(obj);
    }

    //��ȡ����صĶ�������
    public int GetCount()
    {
        return pool.Count;
    }

}
