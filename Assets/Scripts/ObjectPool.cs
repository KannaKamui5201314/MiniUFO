/*
 * 对象池类
 * 有利于生成对象，释放对象，回收对象
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;//预制体
    private List<GameObject> pool;//对象列表

    //构造函数，创建对象池
    //prefab 预制体
    //initialCount 实例化对象的数量
    public ObjectPool(GameObject prefab, int initialCount)
    {
        this.prefab = prefab;
        pool = new List<GameObject>();
        for (int i = 0; i < initialCount; i++)
        {
            //实例化对象
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);//对象失活
            pool.Add(obj);
        }
    }

    //释放对象
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            obj.SetActive(true);//启用对象
            return obj;
        }
        else
        {
            //当对象池没有对象时，实例化一个对象
            return GameObject.Instantiate(prefab);
        }
    }

    //回收对象
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Add(obj);
    }

    //获取对象池的对象数量
    public int GetCount()
    {
        return pool.Count;
    }

}
