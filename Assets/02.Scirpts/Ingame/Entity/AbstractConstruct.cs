using _02.Scirpts.Events;
using UnityEngine;

namespace _02.Scirpts.Ingame.Entity
{
    public abstract class AbstractConstruct : Entity
    {

        [HideInInspector] public int hp;
        [HideInInspector] public int maxhp = 300;
        [HideInInspector] public int[] size = new int[2] { 2, 2 };
        [HideInInspector] public int level = 1;

        public abstract void ConstructTower();
        public abstract void DestroyTower();
        public abstract void OnUpgrade();

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