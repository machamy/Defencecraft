﻿using _02.Scirpts.Ingame;
using _02.Scirpts.Ingame.Entity;
using _02.Scirpts.Ingame.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tower : _02.Scirpts.Ingame.Entity.AbstractConstruct
{
    public GameObject[] TowerSettingBtn = new GameObject[3]; //업그레이드 버튼이 0 취소가 1 철거가 2
    public GameObject bulletPrefab;
    public int damage;
    float bulletSpeed = 1f;
    float bulletDelay = 1f;

    Scanner scanner;
    Animator animator;

    float hprate;
    float timer;


    bool settingbtnactive = false;

    private void Awake()
    {
        scanner = GetComponent<Scanner>();
        animator = GetComponent<Animator>();

    }
    public void Start()
    {
        hp = 300;
        maxhp = 300;
        size = new int[2] { 2, 2 };
        level = 1;
    }

    public void Update()
    {
        base.Update();
        if(isReadyToClick) //업그레이드 중 동작 정지
        {
            timer += Time.deltaTime;

            if (timer > bulletDelay)
            {
                timer = 0f;
                Attack(); //bulletDelay 마다 Attack함수 실행 
            }
        }
    }

    public override void OnClicked()
    {
        UIManager.Instance.OnBuildingSetting(this.gameObject);
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
        StartCoroutine(DestroyCoroutine());

    }

    //업그레이드 이벤트가 발생했을 때
    public override void OnUpgrade()
    {
        hprate = (float)hp / maxhp;

        switch (level)
        {
            case 1:
                StartCoroutine(Upgrade(500));
                break;
            case 2:
                StartCoroutine(Upgrade(1000));
                break;
            default:
                //업그레이드 불가 상태입니다. 표시
                Debug.Log("업그레이드 불가 상태입니다.");
                break;

        };
        Debug.Log($"Tower upgrade complete, hp = {hp}");

    }

    IEnumerator Upgrade(int maxhp)
    {
        isReadyToClick = false;
        animator.SetInteger("Level", level + 1);
        animator.SetTrigger("Upgrade");

        level++;
        hp = Mathf.RoundToInt(maxhp * hprate);

        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션이 끝날때까지 반복
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        isReadyToClick = true;
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
        bullet.GetComponent<Bullet>().Init(damage, dir * bulletSpeed, scanner.nearestTarget, level);


    }

    IEnumerator DestroyCoroutine()
    {
        isReadyToClick = false;
        animator.SetTrigger("Destroy");
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션이 끝날때까지 반복
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        //SetTileNull(Mathf.RoundToInt((transform.position.x -2.5f) / 5f), Mathf.RoundToInt((transform.position.z - 2.5f)/ 5f));
        Destroy(gameObject);
    }

    void SetTileNull(int x, int y)
    {
        //건설 불가 지역으로 설정
        for (int i = 0; i < size[0]; i++)
        {
            for (int j = 0; j < size[1]; j++)
            {
                //타일정보 받아오기
  //              Tile tile = worldscript.GetTile(x + i, y + j);

                //건설 가능 지역으로 설정
 //               tile.Construct = null;
            }
        }
    }

}
