using _02.Scirpts.Ingame.Entity;

namespace _02.Scirpts.Events
{
    public class EntityDamageSO
    {

        public OnDamage OnDamageEvent;
        public OnDamage OnDamageLateEvent;
        
        private Entity damaged;
        private Entity damager;
        private int damage;

        public delegate void OnDamage(Entity damager, Entity damaged, int damage);

    }
}