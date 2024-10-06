using System;
using _02.Scirpts.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _02.Scirpts.Ingame.Entity
{
    public abstract class AbstractConstruct : Entity
    {

        [HideInInspector] public int hp;
        [HideInInspector] public int maxhp = 300;
        [HideInInspector] public int[] size = new int[2] { 2, 2 };
        [HideInInspector] public int level = 1;

        protected bool isReadyToClick = true;
        
        
        public abstract void ConstructTower();
        public abstract void DestroyTower();
        public abstract void OnUpgrade();
        public abstract void OnClicked();
        
        public void Update()
        {
            if (isReadyToClick)
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
                            OnClicked();
                        }
                    }
                }
            }
        }



        public override void OnDamaged(Entity attacker, int damage)
        {
            base.OnDamaged(attacker, damage);
            
            hp -= damage;
            
            //hp가 바닥난다면 파괴
            if(hp < 0)
                DestroyTower();
        }
    }
}