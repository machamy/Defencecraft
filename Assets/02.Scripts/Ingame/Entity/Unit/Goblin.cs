using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : AbstractEnemy
{
    [SerializeField]
    private AbstractConstruct target;
    private bool isOnSearch = true;
    private bool onPathEnd = false;
    private Vector3[] path;
    private int targetIndex;

    void Start()
    {
        Initialize();
        SearchAndRequestPath();
    }

    void FixedUpdate()
    {
        if (ShouldSearchForTarget())
        {
            SearchAndRequestPath();
        }

        if (HasReachedPathEnd())
        {
            MoveTowardsTarget();
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
            HandleBuildingCollision();
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
        while (target != null && target.hp >= 0)
        {
            target.OnDamaged(this, damage);
            if (target.hp < 0)
            {
                ResetSearchState();
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
        UpdateKnownBuildings();
        SetClosestPriorityTarget();
    }

    protected override void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("AlongPath");
            StartCoroutine("AlongPath");
        }
    }

    private void Initialize()
    {
        init();
        hp = 50;
        speed = 5f;
        damage = 20;
        sightRadiusI = 5f;
        sightRadiusII = 10f;
    }

    private bool ShouldSearchForTarget()
    {
        return !target && !iscollision && isOnSearch;
    }

    private void SearchAndRequestPath()
    {
        isOnSearch = false;
        Search();
        if (target != null)
        {
            PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound, false);
        }
    }

    private bool HasReachedPathEnd()
    {
        return target && !onPathEnd;
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 nextPosition = direction * speed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextPosition);
    }

    private void HandleBuildingCollision()
    {
        iscollision = true;
        rigid.isKinematic = true;
        onPathEnd = false;
        StartCoroutine(Attack(target));
    }

    private void ResetSearchState()
    {
        iscollision = false;
        isOnSearch = true;
    }

    private void UpdateKnownBuildings()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightRadiusII);
        foreach (var hitCollider in hitColliders)
        {
            AbstractConstruct building = hitCollider.GetComponent<AbstractConstruct>();
            if (building != null && !knownBuildings.Contains(building))
            {
                knownBuildings.Add(building);
            }
        }
    }

    private void SetClosestPriorityTarget()
    {
        AbstractConstruct closestPriorityTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var building in knownBuildings)
        {
            float distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance <= sightRadiusI && (closestPriorityTarget == null || distance < closestDistance))
            {
                closestPriorityTarget = building;
                closestDistance = distance;
            }
        }

        target = closestPriorityTarget;
        Debug.Log(target != null ? $"{target.name} has detected!" : "nothing detected!");
    }
}