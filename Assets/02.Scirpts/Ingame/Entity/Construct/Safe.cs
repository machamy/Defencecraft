using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Safe : _02.Scirpts.Ingame.Entity.AbstractConstruct
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    public void Start()
    {
        hp = 300;
        maxhp = 300;
        size = new int[2] {2, 2 };
        level = 1;
    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            //UI 요소 안에 마우스가 있으면 리턴
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            // 카메라에서 클릭 위치로의 레이캐스트 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이캐스트가 콜라이더에 닿았는지 확인
            if (Physics.Raycast(ray, out hit))
            {
                // 클릭된 오브젝트가 자신인지 확인
                if (hit.collider.gameObject == gameObject)
                {
                    // 함수 실행
                    OnUpgrade();
                }
            }
        }
    }

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
    public override void OnUpgrade()
    {
        float hprate = hp / maxhp;

        switch (level)
        {
            case 1:
                animator.SetInteger("Level", level + 1);
                animator.SetTrigger("Upgrade");
                maxhp = 500;
                break;
            case 2:
                animator.SetInteger("Level", level + 1);
                animator.SetTrigger("Upgrade");
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
    public override void OnDamaged(Entity attacker, int damage)
    {
        base.OnDamaged(attacker, damage);
        Debug.Log($"Safe hit, hp = {hp}");
    }
}
