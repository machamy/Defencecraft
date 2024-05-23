using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _02.Scirpts.Ingame.Entity
{
    public abstract class AbstractEnemy : Entity
    {
        protected Rigidbody rigid;
        protected int hp;
        protected float speed;
        protected int damage;
        private void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }
        protected abstract void Idle();
        protected abstract void Move(AbstractConstruct target);
        protected abstract IEnumerator Attack(AbstractConstruct target);
        
        protected abstract void Damaged(int damage);
        protected abstract void Search();
        protected abstract void Dead();

    }
}