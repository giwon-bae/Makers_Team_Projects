using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 13f;
    public int maxJumpCount = 2;
    public int health = 3;
    public int cur_exp = 0;

    private float fireDelay = 3f;
    private float curFireCool = 1f;
    private bool isGround = false;
    private bool isDead = false;
    private bool isFireReady = true;
    private int jumpCount = 0;
    private int req_exp = 10;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameManager gameManager;
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
        //Move();
        Attack();

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Hello");
            LevelUp();
        }
    }

    //private void Move()
    //{
    //    if(Input.GetMouseButtonDown(0) && jumpCount < maxJumpCount)
    //    {
    //        jumpCount++;
    //        playerRigidbody.velocity = Vector2.zero;
    //        playerRigidbody.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
    //    }
    //    else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
    //    {
    //        playerRigidbody.velocity = playerRigidbody.velocity * 0.8f;
    //    }

    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        playerCollider.offset = slideColliderOffset;
    //        playerCollider.size = slideColliderSize;
    //    }
    //    else if (Input.GetMouseButtonUp(1))
    //    {
    //        playerCollider.offset = ColliderOffset;
    //        playerCollider.size = ColliderSize;
    //    }
    //}

    public void Jump()
    {
        if (jumpCount < maxJumpCount)
        {
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        else if (playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.8f;
        }
    }

    public void SlideDown()
    {
        playerCollider.offset = slideColliderOffset;
        playerCollider.size = slideColliderSize;
    }

    public void SlideUp()
    {
        playerCollider.offset = ColliderOffset;
        playerCollider.size = ColliderSize;
    }

    public void LevelUp()
    {
        if (cur_exp >= req_exp)
        {
            cur_exp = cur_exp - req_exp;
            Time.timeScale = 0;
            gameManager.AbilityEnforce();
        }
    }

    private void Attack()
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
