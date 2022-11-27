using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosterCtrl : MonoBehaviour
{
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
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
            gameObject.SetActive(false);
            //StopCoroutine("Die");
            //StartCoroutine("Die");
        }
    }

    //IEnumerator Die()
    //{
    //    Debug.Log("Disabled");
    //    gameObject.SetActive(false);
    //    yield return new WaitForSeconds(3f);
    //    gameObject.SetActive(true);
    //    Debug.Log("Activate");
    //}
}
