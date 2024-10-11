using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class L_Golem : _02.Scirpts.Ingame.Entity.AbstractEnemy
{
    [SerializeField]
    private AbstractConstruct target;
    bool isOnSearch = true;
    bool onPathEnd = false;
    Vector3[] path;

    int targetIndex;

    void Start()
    {
        init();

        hp = 100;
        speed = 2.0f;
        damage = 40;

        Search();

        //move along path
        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound, false);

        // make path same height with enemy
        for (int i = 0; i < path.Length; i++)
        {
            path[i].y = transform.position.y;
        }
    }

    void FixedUpdate()
    {
        if (!target && !iscollision && isOnSearch)//시야에 있을 때
        {
            isOnSearch = false;
            Search();
            PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound, false);
        }

        if (transform.position == path[path.Length - 1])
        {
            onPathEnd = true;
        }
        if (onPathEnd)
        {
            //move to target after path
            Vector3 dirVec = target.transform.position - transform.position;
            Vector3 nextVec = dirVec.normalized * speed * Time.deltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("building"))
        {
            iscollision = true;
            rigid.isKinematic = true;
            onPathEnd = false;
            StartCoroutine(Attack(target));
        }
    }
    protected override void Idle()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator AlongPath()
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

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
    protected override IEnumerator Attack(AbstractConstruct target)
    {
        while (target != null && !(target.hp < 0))
        {
            target.OnDamaged(this, damage);
            print(target.hp);
            if (target.hp < 0)
            {
                iscollision = false;
                isOnSearch = true;
            }
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
        AbstractConstruct target = FindObjectOfType<Nexus>();
        //if(시야에 확인되는 것이 있을 때){}
        if(true)
        {
            target = FindObjectOfType<AbstractConstruct>();

        }
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
            StopCoroutine("AlongPath");
            StartCoroutine("AlongPath");
        }
    }

   
}
