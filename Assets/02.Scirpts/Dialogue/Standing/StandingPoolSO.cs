using _02.Scirpts.Pool;
using UnityEngine;

namespace _02.Scirpts.Dialogue
{
    [CreateAssetMenu(fileName = "NewStandingPool", menuName = "Dialogue/StandingPool")]
    public class StandingPoolSO : ComponentPoolSO<Standing>
    {
        [SerializeField] private StandingFactorySO _factory;
        public override IFactory<Standing> Factory {
            get { return _factory;}
            set {_factory = value as StandingFactorySO; } 
        }
    }
}