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
        protected bool iscollision = false;

        protected Vector3[] path;



        protected void init()
        {
            rigid = GetComponent<Rigidbody>();
            rigid.velocity = Vector3.zero;
            
        }

        protected abstract void OnPathFound(Vector3[] newpath, bool pathSuccessful);
        
        protected abstract void Idle();
        protected abstract IEnumerator Move(AbstractConstruct target);
        protected abstract IEnumerator Attack(AbstractConstruct target);
        
        protected abstract void Damaged(int damage);
        protected abstract void Search();
        protected abstract void Dead();

    }
}