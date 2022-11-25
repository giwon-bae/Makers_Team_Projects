using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //public float speed = 3f;


    //void FixedUpdate()
    //{
    //    transform.Translate(Vector3.left * speed * Time.deltaTime);
    //}

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
        }
    }
}
