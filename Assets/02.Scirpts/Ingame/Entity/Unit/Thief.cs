using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : _02.Scirpts.Ingame.Entity.AbstractEnemy
{
    AbstractConstruct Target;

    void Start()
    {
        init();
        hp = 30;
        speed = 8.0f;
        damage = 30;
        Search();
    }

    void Update()
    {
        if (true && !iscollision)//시야에 있을 때
        {
            //Target = 시야에 있는 것
            Move(Target);
        }

        if (hp < 0)
        {
            Dead();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("building"))
        {
            StartCoroutine(Attack(Target));
            iscollision = true;
            rigid.isKinematic = true;
        }
    }
    protected override void Idle()
    {
        throw new System.NotImplementedException();
    }

    protected override void Move(AbstractConstruct target)
    {
        Vector3 dirVec = target.transform.position - transform.position;
        Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    protected override IEnumerator Attack(AbstractConstruct target)
    {
        if (target != null)
        {
            target.OnDamaged(damage);
            yield return new WaitForSeconds(1.0f);
        }

    }

    protected override void Damaged(int damage)
    {
        hp -= damage;
    }
    protected override void Dead()
    {
        Destroy(gameObject);
    }

    protected override void Search()
    {
        Target = FindObjectOfType<Nexus>();
        //if(시야에 확인되는 것이 있을 때){}
        Target = FindObjectOfType<AbstractConstruct>();
        if(Target != null)
        {
            Debug.Log(Target.name + " has detected!");
        }
        else
        {
            Debug.Log("nothing detected!");
        }
    }
}
