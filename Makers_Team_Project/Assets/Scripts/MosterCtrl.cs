using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosterCtrl : MonoBehaviour
{
    Rigidbody2D rigid;
    public float speed = 10f;

    int HP = 100;

    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(-1, rigid.velocity.y); //단순 왼쪽방향 이동  
        transform.Translate(new Vector2(-0.03f, 0));
         
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tmp_bullet(Clone)")
        {
            HP -= 20;
        }
    }
}
