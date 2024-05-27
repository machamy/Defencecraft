using _02.Scirpts.Events;
using UnityEngine;

namespace _02.Scirpts.Ingame.Entity
{
    public class Entity : MonoBehaviour
    {
        public EntityDamageSO EntityDamageChannel;
        
        public virtual void OnDamaged(Entity attacker, int damage)
        {
            EntityDamageChannel.OnDamageEvent.Invoke(attacker, this, damage);
        }
    }
}