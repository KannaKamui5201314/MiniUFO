/*
 * 黑洞道具
 * 主要在黑洞范围施加朝向黑洞中心的力
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private Rigidbody2D rb;
    private readonly float strength = 200f;

    //黑洞设置为触发器
    private void OnTriggerStay2D(Collider2D collision)
    {
        rb = collision.GetComponent<Rigidbody2D>();
        if (rb!=null)
        {
            Vector2 forceDirection = transform.position - collision.transform.position;
            // rb.AddForce加力
            rb.AddForce(forceDirection.normalized * strength);
        }
    }
}
