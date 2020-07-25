using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPooler : MonoBehaviour
    {
        [Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        public List<Pool> Pools;
        public Dictionary<string, Queue<GameObject>> PoolDictionary;

        public static ObjectPooler Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            PoolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in Pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                PoolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!PoolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }
            if (PoolDictionary[tag].Count == 0)
            {
                Debug.LogWarning($"Pool with tag {tag} currently has no objects.");
                return null;
            }
            GameObject obj = PoolDictionary[tag].Dequeue();

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            return obj;
        }

        public void ReturnToPool(string tag, GameObject obj)
        {
            if (!PoolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return;
            }

            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            Debug.Log(obj.transform.position);
            obj.SetActive(false);
            PoolDictionary[tag].Enqueue(obj);
        }

        public void AddToPool(string tag, int amount)
        {
            if (!PoolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return;
            }

            for (int i = 0; i < amount; i++)
            {
                GameObject obj = Instantiate(Pools[Pools.IDByTag(tag)].prefab);
                obj.SetActive(false);
                PoolDictionary[tag].Enqueue(obj);
            }
        }
    }
}
