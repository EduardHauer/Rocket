using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Spawner : MonoBehaviour
    {
        public static Spawner Instance;

        public Vector2 MinRange;
        public Vector2 MaxRange;
        public Vector2Int Grid;
        public string[] PoolTags;

        private ObjectPooler _objectPooler;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _objectPooler = ObjectPooler.Instance;
            Spawn(0);
        }

        public void Spawn(int id)
        {
            float x = 0;
            float y = 0;
            if (MinRange.x == MaxRange.x)
                x = MinRange.x;
            else if (Grid.x > 0)
                x = Random.Range(0, Grid.x) * (MaxRange.x - MinRange.x) / Grid.x + MinRange.x;
            else
                x = Random.Range(MinRange.x, MaxRange.x);
            if (MinRange.y == MaxRange.y)
                y = MinRange.y;
            else if (Grid.y > 0)
                y = Random.Range(0, Grid.y) * (MaxRange.y - MinRange.y) / (Grid.y - 1) + MinRange.y;
            else
                y = Random.Range(MinRange.y, MaxRange.y);
            GameObject obj = _objectPooler.SpawnFromPool(PoolTags[id], new Vector3(x, y), Quaternion.identity);
            //if (obj.GetComponent<Meteorite>() != null)
            //    obj.GetComponent<Meteorite>().Speed = StatController.Instance.Speed;
        }

        public void SpawnAll(int id)
        {
            while (_objectPooler.PoolDictionary[PoolTags[id]].Count > 0)
                Spawn(id);
        }
    }
}
