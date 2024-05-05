using UnityEngine;

namespace _02.Scirpts
{
    public abstract class FactorySO<T> : ScriptableObject, IFactory<T>
    {
        public abstract T Create();
    }
}