using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goblin : _02.Scirpts.Ingame.Entity.AbstractEnemy
    {

    AbstractConstruct Target;

    


    // Start is called before the first frame update
    void Start()
    {
        hp = 50;
        speed = 4.0f;
        damage = 20;
        Search();
    }

        // Update is called once per frame
    void Update()
    {
        if(true)//시야에 있을 때
        {
            //Target = 시야에 있는 것
            Move(Target);
        }

        if(hp < 0)
        {
            Dead();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
           
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
            target.Damaged(damage);
            yield return new WaitForSeconds(1.0f);
        }
        

        else
        {
            throw new System.NotImplementedException();
        }
        //if(target.)
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
        Target = FindObjectOfType<AbstractConstruct>();

    }
}


