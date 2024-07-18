using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    private int level;

    private bool isgenerated = false;

    Transform target;
    Rigidbody rigid;
    Animator animator;
    Vector3 dir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isgenerated) {
            if (target == null)
            {
                StartCoroutine(DestroyBullet());
                return;
            }

            // 목표 방향 계산
            Vector3 dir = (target.position - transform.position).normalized;

            // 목표를 향해 이동 (이미지의 각도를 고정)
            transform.Translate(dir * 3f * Time.deltaTime, Space.World);
        }

    }

    public void Init(float damage, Vector3 dir, Transform target, int level)
    {
        //총알 데미지
        this.damage = damage;

        //총알 속도
        rigid.velocity = dir;

        //목표 지점
        this.target = target;

        this.level = level;
        animator.SetInteger("Level", level);
        Debug.Log(level);

        isgenerated = false;

        StartCoroutine(GenerateBullet());
    }

    IEnumerator DestroyBullet()
    {
        Debug.Log("destroying");
        isgenerated = false;
        rigid.velocity = Vector3.zero;
        animator.SetTrigger("Destroy");

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Bullet_Level1_Destroy") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Bullet_Level2_Destroy") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Bullet_Level3_Destroy"))
        {
            yield return null;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션이 끝날때까지 반복
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        Destroy(gameObject);
    }

    IEnumerator GenerateBullet()
    {
        rigid.velocity = Vector3.zero;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Bullet_Level1_Generate") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Bullet_Level2_Generate") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Bullet_Level3_Generate"))
        {
            yield return null;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션이 끝날때까지 반복
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        isgenerated = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            StartCoroutine(DestroyBullet());
    }
}
