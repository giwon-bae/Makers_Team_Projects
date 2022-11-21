using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject Enemy1;
    [SerializeField] GameObject Enemy2;
    [SerializeField] GameObject Laser;
    Rigidbody2D rigid;
    public int HP = 100;
    public float speed = 0.1f;
    private float LaserDelay = 15f;
    private float curLaserCool = 1f;
    private bool isLaserReady = true;
    private float shootDelay = 9f;
    private float curShootCool = 1f;
    private bool isShootReady = true;
    private float fireDelay = 5f;
    private float curFireCool = 1f;
    private bool isFireReady = true;
    private Transform BulletTransform;
    private Transform EnermyTransform;
    private Transform LaserTransform;
    private int random;
    private int random_subEnemy;
    

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        BulletTransform = this.gameObject.transform.GetChild(0);
        EnermyTransform = this.gameObject.transform.GetChild(1);
        LaserTransform = this.gameObject.transform.GetChild(2);
        random = Random.Range(0, 3);
       

    }

    void Update()
    {
        if (random == 0)
            shootLaser();

        if (random == 1)
            Attack();

        if (random == 2)
            shootEnemy();

        if (isLaserReady)
        {
            Destroy(GameObject.Find("Tmp_subEnermy(Clone)"));
            Destroy(GameObject.Find("Tmp_BossBullet(Clone)"));
        }

        if (HP <= 0)
            Destroy(this.gameObject);

        

    }
    private void Attack()
    {
        curFireCool += Time.deltaTime;
        isFireReady = curFireCool > fireDelay;


        if(isLaserReady)
        {
            curFireCool = 0f;
        }

        if (isFireReady)
        {
            Instantiate(Bullet, BulletTransform.position, BulletTransform.rotation);
            curFireCool = 0f;
        }

    }

    private void shootEnemy()
    {
        curShootCool += Time.deltaTime;
        isShootReady = curShootCool > shootDelay;
        random_subEnemy = Random.Range(0, 2);

        if (isLaserReady)
        {
            curShootCool = 0f;
        }

        if (isShootReady)
        {
            if(random_subEnemy == 0)
            {
                Instantiate(Enemy1, EnermyTransform.position, EnermyTransform.rotation);
                curShootCool = 0f;
            }
            else
            {
                Instantiate(Enemy2, EnermyTransform.position, EnermyTransform.rotation);
                curShootCool = 0f;
            }

        }

    }

    private void shootLaser()
    {
        curLaserCool += Time.deltaTime;
        isLaserReady = curLaserCool > LaserDelay;
        if(isLaserReady)
        {
            Destroy(GameObject.Find("Tmp_subEnermy(Clone)"));

            Instantiate(Laser, LaserTransform.position, LaserTransform.rotation);

            Destroy(GameObject.Find("Tmp_subEnermy(Clone)"));
 
            isShootReady = false;
            isFireReady = false;

            Destroy(GameObject.Find("Tmp_subEnermy(Clone)"));
            Destroy(GameObject.Find("Tmp_Laser(Clone)"), 3f);

            curLaserCool = 0f;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "p_Bullet")
        {
            HP -= 10;
        }
    }
}