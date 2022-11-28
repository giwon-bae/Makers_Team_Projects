using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;

    public GameObject effectPrefab;

    PlayerController playerCtrl;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            playerCtrl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerCtrl.cur_exp += 5;
            playerCtrl.LevelUp();
            collision.gameObject.SetActive(false);
            Destroy(gameObject);
            Instantiate(effectPrefab, collision.transform.position, collision.transform.rotation);
        }
        else if(collision.tag == "Boss")
        {
            Destroy(gameObject);
            //Boss Hp --;
            Instantiate(effectPrefab, collision.transform.position, collision.transform.rotation);
        }
        
    }
}
