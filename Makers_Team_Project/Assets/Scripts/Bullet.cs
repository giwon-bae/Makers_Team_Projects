using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;

    public float speed = 3f;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        //Destroy
        if (gameObject.name == "MonsterCtrl")
        {
            Destroy(gameObject);
        }

    }
}
