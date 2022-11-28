using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{
    public GameObject effectPrefab;

    private PlayerController playerCtrl;
    private Vector2 offSet = new Vector2(0.2f, 0f);

    private void Awake()
    {
        playerCtrl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            playerCtrl.cur_exp += 5;
            playerCtrl.LevelUp();
            collision.gameObject.SetActive(false);
            Instantiate(effectPrefab, collision.transform.position, collision.transform.rotation);
        }
    }
}
