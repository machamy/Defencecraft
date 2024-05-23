using UnityEngine;

namespace _02.Scirpts
{
    
    /// <summary>
    /// 팩토리 인터페이스를 스크립터블 오브젝트화 한 클래스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FactorySO<T> : ScriptableObject, IFactory<T>
    {
        public abstract T Create();
    }
}