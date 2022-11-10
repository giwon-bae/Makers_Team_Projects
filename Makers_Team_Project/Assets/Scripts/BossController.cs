using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    Rigidbody2D rigid;
    public float speed = 0.1f;
    private float fireDelay = 5f;
    private float curFireCool = 1f;
    private bool isFireReady = true;
    private Transform BulletTransform;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        BulletTransform = this.gameObject.transform.GetChild(0);
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
            Instantiate(Bullet, BulletTransform.position, BulletTransform.rotation);
            curFireCool = 0f;
        }
    }


}
