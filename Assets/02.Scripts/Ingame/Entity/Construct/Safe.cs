using _02.Scirpts.Ingame.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Safe : _02.Scirpts.Ingame.Entity.AbstractConstruct
{
    Animator animator;

    float hprate;

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
        base.Update();
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
        Debug.Log("Safe Built");
    }

    //금고가 파괴되었을 때
    public override void DestroyTower()
    {
        Debug.Log("Safe Destroyed");

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
        Debug.Log($"Safe hit, hp = {hp}");
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

        SetTileNull(Mathf.RoundToInt((transform.position.x - 2.5f) / 5f), Mathf.RoundToInt((transform.position.z - 2.5f) / 5f));
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
                //Tile tile = worldscript.GetTile(x + i, y + j);

                //건설 가능 지역으로 설정
                //tile.Construct = null;
            }
        }
    }
}
