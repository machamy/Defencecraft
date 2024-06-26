using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;

public class Tower : _02.Scirpts.Ingame.Entity.AbstractConstruct
{
    public Scanner scanner;
    public GameObject bulletPrefab;
    public float bulletSpeed = 1f;
    public float bulletDelay = 1f;
    public int damage;

    float timer;

    private void Awake()
    {
        scanner = GetComponent<Scanner>();
    }
    public void Start()
    {
        hp = 300;
        maxhp = 300;
        size = new int[2] { 3, 3 };
        level = 1;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if(timer > bulletDelay)
        {
            timer = 0f;
            Attack(); //bulletDelay 마다 Attack함수 실행 
        }
    }

    //처음 만들어질 때 변수 초기화
    public override void ConstructTower()
    {
        hp = 300;
        maxhp = 300;
        level = 1;
        Debug.Log("Tower Built");
    }

    //타워가 파괴되었을 때
    public override void DestroyTower()
    {
        Debug.Log("Tower Destroyed");

        //Object Destroy

    }

    //업그레이드 이벤트가 발생했을 때
    public override void OnUpgrade()
    {
        float hprate = (float)hp / maxhp;

        switch (level)
        {
            case 1:
                maxhp = 500;
                break;
            case 2:
                maxhp = 1000;
                break;
            default:
                //업그레이드 불가 상태입니다. 표시
                Debug.Log("업그레이드 불가 상태입니다.");
                break;

        };
        hp = Mathf.RoundToInt(maxhp * hprate);
        level++;
        Debug.Log($"Tower upgrade complete, hp = {hp}");

    }

    //공격받는 이벤트가 발생했을 때
    public override void OnDamaged(Entity attacker, int damage)
    {
        base.OnDamaged(attacker, damage);
        Debug.Log($"Tower hit, hp = {hp}");
    }

    public void Attack()
    {
        //타겟이 받은게 없다면 return 
        if (!scanner.nearestTarget)
            return;

        // 오브젝트 풀링 기능
        GameObject bullet = Instantiate(bulletPrefab);

        //scanner에서 target 정보 받기
        Vector3 targetpos = scanner.nearestTarget.transform.position;
        Vector3 dir = targetpos - transform.position;
        dir = dir.normalized;

        //bullet init
        Transform bullettransform = bullet.transform;
        bullettransform.position = transform.position;
        bullettransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, dir, scanner.nearestTarget);


    }

}
