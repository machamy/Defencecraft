using UnityEngine;

namespace _02.Scirpts.Pool
{
    public class GameObjectPoolSO: PoolSO<GameObject>
    {
        private Transform _poolRoot;
        [SerializeField] private GameObject prefab = default;
        private IFactory<GameObject> _factory;

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
        
        public override GameObject Get()
        {
            GameObject component = base.Get();
            component.gameObject.SetActive(true);
            return component;
        }
        
        public override void Return(GameObject component)
        {
            component.transform.SetParent(PoolRoot.transform); // 풀 오브젝트에 넣는다
            component.gameObject.SetActive(false);
            base.Return(component);
        }

        public override IFactory<GameObject> Factory {
            get => _factory ??= new GameObjectFactory(prefab); 
            set => _factory = value as GameObjectFactory;
        }

        protected override GameObject Create()
        {
            GameObject newMember = base.Create();
            newMember.transform.SetParent(PoolRoot.transform);
            newMember.gameObject.SetActive(false);
            return newMember;
        }
        
        internal class GameObjectFactory : IFactory<GameObject>
        {
            internal GameObject BaseObject;

            internal GameObjectFactory(GameObject baseObject)
            {
                this.BaseObject = baseObject;
            }
        
            public GameObject Create()
            {
                throw new System.NotImplementedException();
            }
        }
    }


    
}