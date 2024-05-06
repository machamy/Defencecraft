using Unity.VisualScripting;
using UnityEngine;

namespace _02.Scirpts.Pool
{
    
    /// <summary>
    /// 단일 Component에 대한 풀
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component
    {
        private Transform _poolRoot;

        private Transform PoolRoot
        {
            get
            {
                if (_poolRoot == null)
                {
                    _poolRoot = new GameObject(name).transform;
                    _poolRoot.SetParent(_parent);
                }

                return _poolRoot;
            }
        }

        private Transform _parent;


        /// <summary>
        /// 풀의 부모를 설정한다.
        /// </summary>
        /// <param name="t"></param>
        public void SetParent(Transform t)
        {
            _parent = t;
            PoolRoot.SetParent(_parent);
        }

        public override T Get()
        {
            T component = base.Get();
            component.gameObject.SetActive(true);
            return component;
        }
        
        public override void Return(T component)
        {
            component.transform.SetParent(PoolRoot.transform); // 풀 오브젝트에 넣는다
            component.gameObject.SetActive(false);
            base.Return(component);
        }
        
        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(PoolRoot.transform);
            newMember.gameObject.SetActive(false);
            return newMember;
        }
    }
}