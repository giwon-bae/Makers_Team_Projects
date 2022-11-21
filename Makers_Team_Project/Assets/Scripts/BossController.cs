using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject Enermy;
    [SerializeField] GameObject Laser;
    Rigidbody2D rigid;
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

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        BulletTransform = this.gameObject.transform.GetChild(0);
        EnermyTransform = this.gameObject.transform.GetChild(1);
        LaserTransform = this.gameObject.transform.GetChild(2);

    }

    void Update()
    {
        shootLaser();
        Attack();
        shootEnermy();

        if(isLaserReady)
        {
            Destroy(GameObject.Find("Tmp_subEnermy(Clone)"));
            Destroy(GameObject.Find("Tmp_BossBullet(Clone)"));
        }
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

    private void shootEnermy()
    {
        curShootCool += Time.deltaTime;
        isShootReady = curShootCool > shootDelay;


        if(isLaserReady)
        {
            curShootCool = 0f;
        }

        if (isShootReady)
        {
            Instantiate(Enermy, EnermyTransform.position, EnermyTransform.rotation);
            curShootCool = 0f;
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
    
}
