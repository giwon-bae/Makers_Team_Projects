using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 13f;
    public int maxJumpCount = 2;

    private float fireDelay = 3f;
    private float curFireCool = 1f;
    private int jumpCount = 0;
    private bool isGround = false;
    private bool isDead = false;
    private bool isFireReady = true;

    [SerializeField]GameObject bulletPrefab;
    private Rigidbody2D playerRigidbody;
    private CapsuleCollider2D playerCollider;
    private Transform petTransform;
    private Vector2 ColliderOffset;
    private Vector2 ColliderSize;
    private Vector2 slideColliderOffset;
    private Vector2 slideColliderSize;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        petTransform = this.gameObject.transform.GetChild(0);

        ColliderOffset = playerCollider.offset;
        ColliderSize = playerCollider.size;
        slideColliderOffset = new Vector2(playerCollider.offset.x, playerCollider.offset.y * 5f);
        slideColliderSize = new Vector2(playerCollider.size.x, playerCollider.size.y / 2f);
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        Move();
        Attack();
    }

    private void Move()
    {
        //Move에 있던 점프랑 슬라이드를 함수로 뺌
        Jump();
        Slide();


        
    }

    public void Jump()
    {
        if (Input.GetKeyDown("z") && jumpCount < maxJumpCount)
        {
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        else if (Input.GetKeyUp("z") && playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.8f;
        }
    }

    public void Slide()
    {
        if (Input.GetKeyDown("x"))
        {
            playerCollider.offset = slideColliderOffset;
            playerCollider.size = slideColliderSize;
        }
        else if (Input.GetKeyUp("x"))
        {
            playerCollider.offset = ColliderOffset;
            playerCollider.size = ColliderSize;
        }
    }

    public void Attack()  //kick
    {
        curFireCool += Time.deltaTime;
        isFireReady = curFireCool > fireDelay;

        if (isFireReady)
        {
            Instantiate(bulletPrefab, petTransform.position, petTransform.rotation);
            curFireCool = 0f;
        }
    }

    private void Die()
    {
        playerRigidbody.velocity = Vector2.zero;
        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f){
            isGround = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGround = false;
    }
}
