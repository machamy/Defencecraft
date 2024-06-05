using _02.Scirpts.Pool;
using UnityEngine;

namespace _02.Scirpts.Dialogue
{
    public class StandingPool : ComponentPoolSO<Standing>
    {
        [SerializeField] private StandingFactorySO _factory;
        public override IFactory<Standing> Factory {
            get { return _factory;}
            set {_factory = value as StandingFactorySO; } 
        }
    }
}