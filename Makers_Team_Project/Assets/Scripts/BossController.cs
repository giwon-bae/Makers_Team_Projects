using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator bossAnimator;

    public float speed = 0.1f;
    public int HP;

    [SerializeField] Transform BossPos;
    [SerializeField] GameObject[] SnowBalls;
    [SerializeField] GameObject[] Enemies;
    [SerializeField] GameObject Laser;
    [SerializeField] Transform SnowBallTransform;
    [SerializeField] Transform[] EnemyTransform;
    [SerializeField] Transform LaserTransform;
    Rigidbody2D rigid;

    private float attackDelay = 1f;
    private float curCool = 0f;
    private bool isAttackReady = false;

    private float LaserDelay = 15f;
    private float curLaserCool = 1f;
    private float shootDelay = 9f;
    private float curShootCool = 1f;
    private float fireDelay = 5f;
    private float curFireCool = 1f;
    private bool isFireReady = true;
    private bool isLaserReady = true;
    private bool isShootReady = true;
    private int randomIdx;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        HP = 100;
    }

    void Update()
    {
        Move();
        SelectAttack();

        if (isLaserReady)
        {
            Destroy(GameObject.Find("Tmp_subEnermy(Clone)"));
            Destroy(GameObject.Find("Tmp_BossBullet(Clone)"));
        }

        if (HP <= 0)
        {
            //Game Clear
            Debug.Log("Game Clear!");
            bossAnimator.SetBool("IsDead", true);
            //Use Coroutine - TODO
            //Time.timeScale = 0;
            //Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (transform.position == BossPos.position)
        {
            bossAnimator.SetBool("IsBossPos", true);
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, BossPos.position, Time.deltaTime * 2f);
    }

    private void SelectAttack()
    {
        if (transform.position != BossPos.position)
        {
            return;
        }

        curCool += Time.deltaTime;
        isAttackReady = curCool > attackDelay;

        if (!isAttackReady)
        {
            return;
        }

        bossAnimator.SetTrigger("DoAttack");
        randomIdx = Random.Range(0, 3);
        switch (randomIdx)
        {
            case 0:
                shootEnemy();
                //change attackDelay - TODO
                break;
            case 1:
                //shootLaser();
                Attack();
                //change attackDelay - TODO
                break;
            case 2:
                Attack();
                //change attackDelay - TODO
                break;
        }

        curCool = 0f;
    }

    private void Attack()
    {
        //curFireCool += Time.deltaTime;
        //isFireReady = curFireCool > fireDelay;

        //if (isLaserReady)
        //{
        //    curFireCool = 0f;
        //}

        //if (isFireReady)
        //{
        //    Instantiate(Bullet, BulletTransform.position, BulletTransform.rotation);
        //    curFireCool = 0f;
        //}

        int numberOfSnowBall = Random.Range(0, 3);

        for(int i=0; i<=numberOfSnowBall; i++)
        {
            int typeOfSnowBall = Random.Range(0, SnowBalls.Length);
            Vector2 PosOfSnowBall = new Vector2(SnowBallTransform.position.x, SnowBallTransform.position.y - Random.Range(-1f, 1f));
            Instantiate(SnowBalls[typeOfSnowBall], PosOfSnowBall, SnowBallTransform.rotation);
        }
    }

    private void shootEnemy()
    {
        //curShootCool += Time.deltaTime;
        //isShootReady = curShootCool > shootDelay;


        //if (isLaserReady)
        //{
        //    curShootCool = 0f;
        //}

        //if (isShootReady)
        //{
        //    //Instantiate(Enermy, EnermyTransform.position, EnermyTransform.rotation);
        //    curShootCool = 0f;
        //}

        int typeOfEnemy = Random.Range(0, Enemies.Length);
        Instantiate(Enemies[typeOfEnemy], EnemyTransform[typeOfEnemy].position, EnemyTransform[typeOfEnemy].rotation);
    }

    private void shootLaser()
    {
        curLaserCool += Time.deltaTime;
        isLaserReady = curLaserCool > LaserDelay;
        if (isLaserReady)
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
        if (collision.gameObject.tag == "PlayerBullet")
        {
            HP -= 1;
        }
    }
}
