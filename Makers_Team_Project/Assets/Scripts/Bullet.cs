using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 2;

    public GameObject effectPrefab;

    PlayerController playerCtrl;
    BossController bossCtrl;
    SpriteRenderer rend;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 3.5f);
        switch (damage)
        {
            case 3:
                rend.flipX = false;
                animator.SetBool("damage3", true);
                break;
            case 4:
                rend.flipX = false;
                animator.SetBool("damage4", true);
                break;
            case 5:
                rend.flipX = false;
                animator.SetBool("damage5", true);
                break;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerCtrl.cur_exp += collision.GetComponent<MonsterCtrl>().dropExp;
            playerCtrl.LevelUp();
            collision.gameObject.SetActive(false);
            Destroy(gameObject);
            Instantiate(effectPrefab, collision.transform.position, collision.transform.rotation);
        }
        else if(collision.tag == "Boss")
        {
            bossCtrl = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
            bossCtrl.HP -= damage;
            Instantiate(effectPrefab, collision.transform.position, collision.transform.rotation);
            Destroy(gameObject);
        }
        
    }
}
