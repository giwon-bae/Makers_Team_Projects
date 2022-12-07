using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator bossAnimator;

    public float speed = 0.1f;
    public bool isDead = false;
    public int HP;

    [SerializeField] GameManager gameManager;
    [SerializeField] Transform BossPos;
    [SerializeField] GameObject[] SnowBalls;
    [SerializeField] GameObject[] Enemies;
    [SerializeField] GameObject Laser;
    [SerializeField] Transform[] SnowBallTransform;
    [SerializeField] Transform[] EnemyTransform;
    [SerializeField] Transform LaserTransform;
    Rigidbody2D rigid;

    private float attackDelay = 1f;
    private float curCool = 0f;
    private bool isAttackReady = false;

    private float LaserDelay = 15f;
    private float curLaserCool = 1f;
    //private float shootDelay = 9f;
    //private float curShootCool = 1f;
    //private float fireDelay = 5f;
    //private float curFireCool = 1f;
    //private bool isFireReady = true;
    private bool isLaserReady = true;
    //private bool isShootReady = true;
    private int randomIdx;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        HP = 100;
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

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
            isDead = true;
            bossAnimator.SetBool("IsDead", true);
            Invoke("GameClear", 1f);
        }
    }

    private void GameClear()
    {
        Debug.Log("GameClear!");
        gameManager.GameClear();
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
        randomIdx = Random.Range(0, 4);
        switch (randomIdx)
        {
            case 0:
                shootEnemy();
                attackDelay = 1.5f;
                break;
            case 1:
                StartCoroutine("Attack1");
                attackDelay = 3f;
                break;
            case 2:
                StartCoroutine("Attack2");
                attackDelay = 6f;
                break;
            case 3:
                StartCoroutine("Attack3");
                attackDelay = 4f;
                break;
        }

        if(HP <= 50)
        {
            attackDelay *= 0.75f;
        }

        curCool = 0f;
    }

    private void shootEnemy()
    {

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

            //isShootReady = false;
            //isFireReady = false;
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

    IEnumerator Attack1()
    {
        for(int i=0; i<6; i++)
        {
            if (i == 3)
            {
                bossAnimator.SetTrigger("DoAttack");
                yield return new WaitForSeconds(0.1f);
            }
            Instantiate(SnowBalls[0], SnowBallTransform[i%3].position, SnowBallTransform[i%3].rotation);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Attack2()
    {
        Instantiate(SnowBalls[0], SnowBallTransform[0].position, SnowBallTransform[0].rotation);
        Instantiate(SnowBalls[0], SnowBallTransform[2].position, SnowBallTransform[2].rotation);
        yield return new WaitForSeconds(0.4f);
        bossAnimator.SetTrigger("DoAttack");
        Instantiate(SnowBalls[0], SnowBallTransform[0].position, SnowBallTransform[0].rotation);
        Instantiate(SnowBalls[0], SnowBallTransform[1].position, SnowBallTransform[1].rotation);
        yield return new WaitForSeconds(0.6f);
        bossAnimator.SetTrigger("DoAttack");
        Instantiate(SnowBalls[0], SnowBallTransform[0].position, SnowBallTransform[0].rotation);
        Instantiate(SnowBalls[0], SnowBallTransform[2].position, SnowBallTransform[2].rotation);
        yield return new WaitForSeconds(0.5f);
        bossAnimator.SetTrigger("DoAttack");
        Instantiate(SnowBalls[0], SnowBallTransform[1].position, SnowBallTransform[1].rotation);
        Instantiate(SnowBalls[0], SnowBallTransform[2].position, SnowBallTransform[2].rotation);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Attack3()
    {
        int numberOfSnowBall = Random.Range(1, 5);
        for (int i=1; i<= numberOfSnowBall; i++)
        {
            if (i % 3 == 0)
            {
                bossAnimator.SetTrigger("DoAttack");
            }
            Instantiate(SnowBalls[1], SnowBallTransform[1].position, SnowBallTransform[1].rotation);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
