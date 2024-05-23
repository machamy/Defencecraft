﻿using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Safe : AbstractConstruct
{
    [HideInInspector] public int hp;

    private int maxhp = 300;
    private int[] size = new int[2] { 2, 2 };
    private int level = 1;

    //처음 만들어질 때 변수 초기화
    public override void ConstructTower()
    {
        hp = 300;
        maxhp = 300;
        level = 1;
        Debug.Log("Safe Built");
    }

    //금고가 파괴되었을 때
    public override void DestroyTower()
    {
        Debug.Log("Safe Destroyed");

        //Object Destroy

    }

    //업그레이드 이벤트가 발생했을 때
    public override void Upgrade()
    {
        float hprate = hp / maxhp;

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
        Debug.Log($"Safe upgrade complete, hp = {hp}");

    }

    //공격받는 이벤트가 발생했을 때
    public override void Damaged(int damage)
    {
        hp -= damage;

        Debug.Log($"Safe hit, hp = {hp}");

        //hp가 바닥난다면 파괴
        if (hp < 0)
            DestroyTower();
    }
}
