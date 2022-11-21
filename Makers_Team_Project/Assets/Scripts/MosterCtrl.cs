using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosterCtrl : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rigid;

    //int HP = 100;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        rigid.velocity = new Vector2(-1, rigid.velocity.y); //단순 왼쪽방향 이동  
        transform.Translate(new Vector2(-0.03f, 0));
         
        //if (HP <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController.isInvincibility)
            {
                return;
            }

            playerController.CheckHit();
            Destroy(gameObject);
        }
    }
}
