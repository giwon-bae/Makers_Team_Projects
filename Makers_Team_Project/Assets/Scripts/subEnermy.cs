using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subEnermy : MonoBehaviour
{
    public float speed = 4f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        //Destroy
    }
}
