using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 13f;
    public float fireDelay = 3f;
    public float kickDelay = 3f;
    public int maxJumpCount = 2;
    public int hp = 3;
    public int cur_exp = 0;

    private float curFireCool = 1f;
    private float curKickCool = 0f;
    private bool isJump = false;
    private bool isDead = false;
    private bool isFireReady = true;
    private bool isKickReady = true;
    private int jumpCount = 0;
    private int req_exp = 10;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameManager gameManager;
    [SerializeField] BoxCollider2D meleeArea;
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
        Timer();
        RangedAttack();

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Level Up");
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
        if (isJump)
        {
            return;
        }
        playerCollider.offset = slideColliderOffset;
        playerCollider.size = slideColliderSize;
        petTransform.position = new Vector2(petTransform.position.x, petTransform.position.y - 1f);
    }

    public void SlideUp()
    {
        if (isJump)
        {
            return;
        }
        playerCollider.offset = ColliderOffset;
        playerCollider.size = ColliderSize;
        petTransform.position = new Vector2(petTransform.position.x, petTransform.position.y + 1f);
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

    public void MeleeAttack()
    {
        if (isKickReady)
        {
            StopCoroutine("Kick");
            StartCoroutine("Kick");
            curKickCool = 0f;
        }
    }

    private void RangedAttack()
    {
        if (isFireReady)
        {
            Instantiate(bulletPrefab, petTransform.position, petTransform.rotation);
            curFireCool = 0f;
        }
    }

    private void Timer()
    {
        curFireCool += Time.deltaTime;
        isFireReady = curFireCool > fireDelay;
        curKickCool += Time.deltaTime;
        isKickReady = curKickCool > kickDelay;
    }

    public void HpController()
    {
        gameManager.UpdateHpIcon(hp);
        if(hp > 0)
        {
            return;
        }
        Debug.Log("Game Over");
        gameManager.GameOver();
        playerRigidbody.velocity = Vector2.zero;
        isDead = true;
        //start die animation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Dead" && !isDead)
        {
            hp = 0;
            HpController();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f){
            isJump = false;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isJump = true;
    }

    IEnumerator Kick()
    {
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.3f);

        meleeArea.enabled = false;
    }
}
