using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 13f;
    public float fireDelay = 3f;
    public float kickDelay = 3f;
    public float curKickCool = 3f;
    public float invincibilityDuration = 1f;
    public bool isGigantic = false;
    public bool isInvincibility = false;
    public int maxJumpCount = 2;
    public int req_exp = 10;
    public int cur_exp = 0;

    public GameObject shield;
    public Animator animator;

    private float curFireCool = 1f;
    private bool isJump = false;
    private bool isDead = false;
    private bool isFireReady = true;
    private bool isKickReady = true;
    private int jumpCount = 0;
    private int hp = 3;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject meleeArea;
    private Rigidbody2D playerRigidbody;
    private CapsuleCollider2D playerCollider;
    private SpriteRenderer playerSprite;
    private Transform petTransform;
    private Vector2 ColliderOffset;
    private Vector2 ColliderSize;
    private Vector2 slideColliderOffset;
    private Vector2 slideColliderSize;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        petTransform = this.gameObject.transform.GetChild(0);

        ColliderOffset = playerCollider.offset;
        ColliderSize = playerCollider.size;
        slideColliderOffset = new Vector2(playerCollider.offset.x, playerCollider.offset.y * 10f);
        slideColliderSize = new Vector2(playerCollider.size.x, playerCollider.size.y / 2f);
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        Move();
        Timer();
        RangedAttack();

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Level Up");
            LevelUp();
        }
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(0) && jumpCount < maxJumpCount)
        {
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("DoJump");
        }
        else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.8f;
        }

        if (Input.GetMouseButtonDown(1)&&!isJump&&!isGigantic)
        {
            playerCollider.offset = slideColliderOffset;
            playerCollider.size = slideColliderSize;
            petTransform.position = new Vector2(petTransform.position.x, petTransform.position.y - 1f);
            animator.SetBool("IsSlide", true);
        }
        if (Input.GetMouseButtonUp(1) && !isGigantic)
        {
            playerCollider.offset = ColliderOffset;
            playerCollider.size = ColliderSize;
            petTransform.position = new Vector2(petTransform.position.x, petTransform.position.y + 1f);
            animator.SetBool("IsSlide", false);
        }

        if (Input.GetKey(KeyCode.K))
        {
            MeleeAttack();
        }
    }

    public void Jump()
    {
        if (jumpCount < maxJumpCount)
        {
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("DoJump");
        }
        else if (playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.8f;
        }
    }

    public void SlideDown()
    {
        if (isJump || isGigantic)
        {
            return;
        }
        playerCollider.offset = slideColliderOffset;
        playerCollider.size = slideColliderSize;
        petTransform.position = new Vector2(petTransform.position.x, petTransform.position.y - 1f);
        animator.SetBool("IsSlide", true);
    }

    public void SlideUp()
    {
        if (isJump || isGigantic)
        {
            return;
        }
        playerCollider.offset = ColliderOffset;
        playerCollider.size = ColliderSize;
        petTransform.position = new Vector2(petTransform.position.x, petTransform.position.y + 1f);
        animator.SetBool("IsSlide", false);
    }

    public void LevelUp()
    {
        if (cur_exp >= req_exp)
        {
            cur_exp = cur_exp - req_exp;
            req_exp += 10;
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

    public void CheckHit()
    {
        if(!isGigantic)
        {
            if (shield.activeSelf == true)
            {
                shield.SetActive(false);
            }
            else
            {
                hp -= 1;
                animator.SetTrigger("Hitted");
                HpController();
                //hitted motion
            }
            StopCoroutine(Invincibility());
            StartCoroutine(Invincibility());
        }
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

    public void GiganticAbility()
    {
        StopCoroutine(Gigantic());
        StartCoroutine(Gigantic());
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
        meleeArea.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        meleeArea.SetActive(false);
    }

    IEnumerator Invincibility()
    {
        Debug.Log("Start Coroutine");
        isInvincibility = true;

        for(int i=0; i<invincibilityDuration/1; i++)
        {
            playerSprite.color = new Color(255, 255, 255, 0.4f);
            yield return new WaitForSeconds(0.5f);
            playerSprite.color = new Color(255, 255, 255, 0.7f);
            yield return new WaitForSeconds(0.5f);
        }

        isInvincibility = false;
        playerSprite.color = new Color(255, 255, 255, 1);
        Debug.Log("Stop Coroutine");
    }

    IEnumerator Gigantic()
    {
        isGigantic = true;
        gameObject.transform.position = new Vector2(-5f, -1.1f);
        gameObject.transform.localScale = new Vector2(1f, 1f);
        yield return new WaitForSeconds(0.3f);

        gameObject.transform.position = new Vector2(-3.5f, 0.3f);
        gameObject.transform.localScale = new Vector2(1.8f, 1.8f);
        yield return new WaitForSeconds(5f);

        gameObject.transform.localScale = new Vector2(1f, 1f);
        gameObject.transform.position = new Vector2(-5f, -1.1f);
        yield return new WaitForSeconds(0.3f);

        gameObject.transform.localScale = new Vector2(0.5f, 0.5f);
        gameObject.transform.position = new Vector2(-6.5f, -2.2f);
        isGigantic = false;
        yield return Invincibility();
    }
}
