﻿using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Golem : _02.Scirpts.Ingame.Entity.AbstractEnemy
{
    [SerializeField]
    AbstractConstruct target;

    int targetIndex;

    void Awake()
    {
        init();
        //modifying
        //PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
        hp = 100;
        speed = 2.0f;
        damage = 40;
        Search();
    }

    void FixedUpdate()
    {
        if (true && !iscollision)//시야에 있을 때
        {
            //Target = 시야에 있는 것
            Move(target);
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
            StartCoroutine(Attack(target));
            iscollision = true;
            rigid.isKinematic = true;
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
            if (transform.position == currentWaypoint)
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
        if (target != null)
        {
            target.OnDamaged(this, damage);
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            Search();
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
