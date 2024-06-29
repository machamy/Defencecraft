using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02.Scirpts.Dictionary
{
    /// <summary>
    /// 인스펙터 창에서 수정 가능한 유사 딕셔너리
    /// </summary>
    /// <typeparam name="K">키값</typeparam>
    /// <typeparam name="V">밸류</typeparam>
    [Serializable]
    public class SerializableDict<K,V> :Dictionary<K,V>, ISerializationCallbackReceiver
    {
        public static V Null = default;
        public List<SerializableData<K, V>> dataList = new List<SerializableData<K, V>>();
        
        // public V Get(K key)
        // {
        //     foreach (var d in data)
        //     {
        //         if (Equals(d.key,key))
        //             return d.value;
        //     }
        //
        //     return Null;
        // }
        //
        // public void Set(K key, V val)
        // {
        //     foreach (var d in data)
        //     {
        //         if (Equals(d.key, key))
        //         {
        //             d.value = val;
        //             return;
        //         }
        //     }
        //     data.Add(new SerializableData<K, V>(key,val));
        // }
        //
        
        /// <summary>
        /// 리스트 -> 딕셔너리
        /// </summary>
        public void OnBeforeSerialize()
        {
            dataList.Clear();
            foreach (KeyValuePair<K, V> pair in this)
            {
                dataList.Add(new SerializableData<K, V>(pair.Key,pair.Value));
            }
        }

        /// <summary>
        /// 딕셔너리 -> 리스트
        /// </summary>
        public void OnAfterDeserialize()
        {
            this.Clear();
            foreach (var data in dataList)
            {
                if (this.Keys.Contains(data.key))
                    data.key = default(K);
                if (!TryAdd(data.key,data.value))
                {
                    Debug.LogWarning($"같은 키 값은 들어갈 수 없습니다 (key: {data.key})");
                };
            }
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