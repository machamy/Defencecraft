﻿using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : _02.Scirpts.Ingame.Entity.AbstractConstruct
{
    public void Start()
    {
        hp = 300;
        maxhp = 300;
        size = new int[2] { 3, 3 };
        level = 1;
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
    public override void OnDamaged(int damage)
    {
        hp -= damage;

        Debug.Log($"Tower hit, hp = {hp}");

        //hp가 바닥난다면 파괴
        if (hp < 0)
            DestroyTower();
    }

    public void Attack()
    {
        //적 공격 알고리즘 추가할 것.
    }
}
