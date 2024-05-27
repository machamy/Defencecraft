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
        //�Ѿ� ������
        this.damage = damage;

        //�Ѿ� �ӵ�
        rigid.velocity = dir * 3f;
    }
}
