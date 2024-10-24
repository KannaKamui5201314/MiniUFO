using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private Rigidbody2D rb;
    private readonly float strength = 200f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        rb = collision.GetComponent<Rigidbody2D>();
        if (rb!=null)
        {
            Vector2 forceDirection = transform.position - collision.transform.position;
            //ºÚ¶´ÖÐÐÄÎü¸½
            rb.AddForce(forceDirection.normalized * strength);
        }
    }
}
