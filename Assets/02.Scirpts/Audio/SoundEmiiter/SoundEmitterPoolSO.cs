using _02.Scirpts.Pool;
using UnityEngine;

namespace _02.Scirpts.Audio
{
    [CreateAssetMenu(fileName = "SoundEmitterPoolSO", menuName = "Scriptable Object/SoundEmitter/SoundEmitterPoolSO" )]
    public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
    {
        [SerializeField] private SoundEmitterFactorySO _factory;
        public override IFactory<SoundEmitter> Factory
        {
            get { return _factory;}
            set {_factory = value as SoundEmitterFactorySO; } 
        }
    }
}