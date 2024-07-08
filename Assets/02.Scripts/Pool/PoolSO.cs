using System;
using System.Collections.Generic;
using UnityEngine;

namespace _02.Scirpts.Pool
{
    public abstract class PoolSO<T> : ScriptableObject, IPool<T>
    {
        private bool isInitialized = false;

        public abstract IFactory<T> Factory { get; set; }

        protected Stack<T> Available = new Stack<T>();



        protected virtual T Create()
        {
            return Factory.Create();
        }
        

        public void init(int n)
        {
            if (isInitialized)
                return;
            for (int i = 0; i < n; i++)
            {
                Available.Push(Create());
            }
            isInitialized = true;
        }

        /// <summary>
        /// Pool에서 컴포넌트를 받아온다.
        /// 없을경우 생성
        /// </summary>
        /// <returns></returns>
        public virtual T Get()
        {
            return Available.Count > 0 ? Available.Pop() : Create();
        }

        /// <summary>
        /// Pool에 컴포넌트를 반환한다.
        /// </summary>
        /// <param name="e"></param>
        public virtual void Return(T e)
        {
            Available.Push(e);
        }
    }
}