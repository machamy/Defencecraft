using UnityEngine;

namespace _02.Scirpts.Ingame.Entity
{
    public abstract class AbstractConstruct : Entity
    {
        public abstract void ConstructTower();
        public abstract void DestroyTower();
        public abstract void Upgrade();
        public abstract void Damaged(int damage);

    }
}