using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    public Transform bullet_pos;
    Rigidbody2D rigid;
    public int HP = 1000;
    public float speed = 0.1f;
    private float fireDelay = 5f;
    private float curFireCool = 1f;
    private bool isFireReady = true;
    

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Attack();
    }
    private void Attack()
    {
        curFireCool += Time.deltaTime;
        isFireReady = curFireCool > fireDelay;

        if (isFireReady)
        {
            Instantiate(Bullet, bullet_pos.position, bullet_pos.rotation);
            curFireCool = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tmp_bullet(Clone)")
        {
            HP -= 100;
        }
    }
}
