using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Init(float damage, Vector3 dir)
    {
        //ÃÑ¾Ë µ¥¹ÌÁö
        this.damage = damage;

        //ÃÑ¾Ë ¼Óµµ
        rigid.velocity = dir * 3f;
    }
}
