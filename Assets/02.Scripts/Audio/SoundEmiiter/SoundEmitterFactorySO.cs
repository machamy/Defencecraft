
using UnityEngine;

namespace _02.Scirpts.Audio
{
    [CreateAssetMenu(fileName = "NewSoundEmitterFactory", menuName = "SoundEmitter/Factory")]
    public class SoundEmitterFactorySO : FactorySO<SoundEmitter>
    {
        [SerializeField] private SoundEmitter soundEmitterPrefab;
        
        public override SoundEmitter Create()
        {
            return Instantiate(soundEmitterPrefab);
        }
    }
}