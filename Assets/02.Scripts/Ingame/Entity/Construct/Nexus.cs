using System.Collections;
using System.Collections.Generic;
using _02.Scirpts.Ingame.Entity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Nexus : _02.Scirpts.Ingame.Entity.AbstractConstruct
{
    //처음 시작할 때 변수 초기화 (OnEnable을 써야할지 질문해야함) 
    public void Start()
    {
        hp = 300;
        maxhp = 300;
        size = new int[2] {3, 3};
        level = 1;
    }


    public void Update()
    {
        base.Update();
    }

    //넥서스는 게임이 생성될 때 지어지기 때문에 큰 의미 x
    public override void ConstructTower()
    {
        Debug.Log("Nexus Built");
    }

    //넥서스가 파괴되었을 때
    public override void DestroyTower()
    {
        Debug.Log("Nexus Destroyed");
        //게임 오버
    }

    //업그레이드 이벤트가 발생했을 때
    public override void OnUpgrade()
    {
        float hprate = hp / maxhp;

        maxhp = 500;
        hp = Mathf.RoundToInt(maxhp * hprate);

        Debug.Log($"Nexus upgrade complete, hp = {hp}");
        
    }

    public override void OnClicked()
    {
        // Do Nothing
    }

    //공격받는 이벤트가 발생했을 때
    public override void OnDamaged(Entity attacker, int damage)
    {
        base.OnDamaged(attacker, damage);
        Debug.Log($"Nexus hit, hp = {hp}");
    }
}
