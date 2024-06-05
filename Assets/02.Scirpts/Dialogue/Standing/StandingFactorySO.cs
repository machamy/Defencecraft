using UnityEngine;

namespace _02.Scirpts.Dialogue
{
    public class StandingFactorySO : FactorySO<Standing>
    {
        [SerializeField] private Standing standingPrefab;
        public override Standing Create()
        {
            return Instantiate(standingPrefab);
        }
    }
}