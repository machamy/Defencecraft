using System;
using System.Collections.Generic;

namespace _02.Scirpts.Dictionary
{
    /// <summary>
    /// 인스펙터 창에서 수정 가능한 유사 딕셔너리
    /// </summary>
    /// <typeparam name="K">키값</typeparam>
    /// <typeparam name="V">밸류</typeparam>
    [Serializable]
    public class SerializableDict<K,V>
    {
        public static V Null = default;
        public List<SerializableData<K, V>> data;

        public V Get(K key)
        {
            foreach (var d in data)
            {
                if (Equals(d.key,key))
                    return d.value;
            }

            return Null;
        }

        public void Set(K key, V val)
        {
            foreach (var d in data)
            {
                if (Equals(d.key, key))
                {
                    d.value = val;
                    return;
                }
            }
            data.Add(new SerializableData<K, V>(key,val));
        }
    }

    [Serializable]
    public class SerializableData<K,V>
    {
        public K key;
        public V value;
        
        public SerializableData(K key, V val)
        {
            this.key = key;
            this.value = val;
        }
    }
}