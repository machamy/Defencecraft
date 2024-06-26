using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    Transform target;
    Rigidbody rigid;
    Vector3 dir;
    bool isready = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (target == null)
        {
            transform.Translate(Vector3.forward * 3f * Time.deltaTime);
            return;
        }

        dir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);

        transform.Translate(Vector3.forward * 3f * Time.deltaTime);
    }

    public void Init(float damage, Vector3 dir, Transform target)
    {
        //총알 데미지
        this.damage = damage;

        //총알 속도
        rigid.velocity = dir * 3f;

        //목표 지점
        this.target = target;

        isready = true;



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
