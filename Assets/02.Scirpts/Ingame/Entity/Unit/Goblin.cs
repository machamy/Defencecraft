using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Goblin : _02.Scirpts.Ingame.Entity.AbstractEnemy
 {
    [SerializeField]
    AbstractConstruct target;
    
    int targetIndex;
    void Awake()
    {
        init();
        
        //modifying
        //PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
        hp = 50;
        speed = 4.0f;
        damage = 20;
        Search();
    }
    
    void FixedUpdate()
    {
        if(true && !iscollision)//시야에 있을 때
        {
            //Target = 시야에 있는 것
            Move(target);
        }

        if(hp < 0)
        {
            Dead();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("building"))
        {
            iscollision = true;
            rigid.isKinematic = true;
            StartCoroutine(Attack(target));
        }
    }
    protected override void Idle()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Move(AbstractConstruct target)
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length) 
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);  
            yield return null;
        }
    }
    protected override IEnumerator Attack(AbstractConstruct target)
    {
        while (target != null && !(target.hp < 0))
        { 
            target.OnDamaged(this, damage);
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
        target = FindObjectOfType<Nexus>();
        //if(시야에 확인되는 것이 있을 때){}
        target = FindObjectOfType<AbstractConstruct>();
        if (target != null)
        {
            Debug.Log(target.name + " has detected!");
        }
        else
        {
            Debug.Log("nothing detected!");
        }
    }

    protected override void OnPathFound(Vector3[] newpath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newpath;
            StopCoroutine("Move");
            StartCoroutine("Move");
        }
    }
}


