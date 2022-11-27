using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController.isInvincibility)
            {
                return;
            }
            if (playerController.isGigantic)
            {
                //StopCoroutine("Destroy");
                //StartCoroutine("Destroy");
                gameObject.SetActive(false);
            }

            playerController.CheckHit();
        }
    }

    //IEnumerator Destroy()
    //{
    //    gameObject.SetActive(false);
    //    yield return new WaitForSeconds(3f);
    //    gameObject.SetActive(true);
    //}
}
