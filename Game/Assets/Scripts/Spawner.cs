using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Spawner : MonoBehaviour
    {
        public Vector2 SpawnRangeY;
        public Vector2 RangeX;
        public float Speed;
        public float MinSpawnDistance;
        public SpawnObject[] Objects;

        private int _allCount;
        private int _activeCount;
        private Coroutine _coroutine;
        private float _lastY;
        private float _newY;

        [Serializable] public struct SpawnObject
        {
            public GameObject prefab;
            public int count;
            [HideInInspector] public List<GameObject> objects;
            [HideInInspector] public List<bool> activeObjects;
            [HideInInspector] public Coroutine coroutine;
        }

        private void Awake()
        {
            _allCount = 0;
            for (int i = 0; i < Objects.Length; i++)
            {
                Objects[i].objects = new List<GameObject>();
                Objects[i].activeObjects = new List<bool>();
                for (int j = 0; j < Objects[i].count; j++)
                {
                    GameObject obj = Instantiate(Objects[i].prefab, transform);
                    obj.SetActive(false);
                    SpawnedObject s = obj.GetComponent<SpawnedObject>();
                    if (s != null)
                    {
                        s.Spawner = this;
                        s.ID = i;
                        s.ObjectID = j;
                    }
                    Objects[i].objects.Add(obj);
                    Objects[i].activeObjects.Add(false);
                    _allCount++;
                }
            }
            Spawn();
        }

        private void Update()
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                for (int j = 0; j < Objects[i].objects.Count; j++)
                {
                    if (!Objects[i].activeObjects[j])
                        continue;
                    Objects[i].objects[j].transform.position = new Vector2(Objects[i].objects[j].transform.position.x - Time.deltaTime * Speed * StatController.Instance.Speed, Objects[i].objects[j].transform.position.y);
                    if (Objects[i].objects[j].transform.position.x < RangeX.x)
                    {
                        Return(i, j);
                    }
                }
            }
        }

        public void Spawn()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(SpawnCoroutine());
        }

        public IEnumerator SpawnCoroutine()
        {
            _newY = UnityEngine.Random.Range(SpawnRangeY.x, SpawnRangeY.y);
            while (true)
            {
                if (_activeCount < _allCount)
                {
                    while (!SpawnByID(UnityEngine.Random.Range(0, Objects.Length), _newY)) { }
                    _lastY = _newY;
                    _newY = UnityEngine.Random.Range(SpawnRangeY.x, SpawnRangeY.y);
                    float min = 0;
                    if (MinSpawnDistance > Mathf.Abs(_lastY - _newY))
                        min = Mathf.Sqrt(Mathf.Pow(MinSpawnDistance, 2) - Mathf.Pow(Mathf.Abs(_lastY = _newY), 2)) / Speed / StatController.Instance.Speed;
                    if (min > (RangeX.y - RangeX.x) / _allCount / Speed / StatController.Instance.Speed)
                        yield return new WaitForSeconds(min);
                    else
                    {
                        if (_allCount > 1)
                            yield return new WaitForSeconds(UnityEngine.Random.Range((RangeX.y - RangeX.x) / _allCount / Speed / StatController.Instance.Speed, (RangeX.y - RangeX.x) / (_allCount - 1) / Speed / StatController.Instance.Speed));
                        else if (_allCount == 1)
                            yield return new WaitForSeconds((RangeX.y - RangeX.x) / Speed / StatController.Instance.Speed);
                    }
                }
                else
                    yield return new WaitForSeconds(1);
            }
        }

        public bool SpawnByID(int id, float y)
        {
            for (int i = 0; i < Objects[id].objects.Count; i++)
            {
                if (Objects[id].activeObjects[i])
                    continue;
                Objects[id].objects[i].SetActive(true);
                Objects[id].objects[i].transform.position = new Vector2(RangeX.y, UnityEngine.Random.Range(SpawnRangeY.x, SpawnRangeY.y));
                Objects[id].activeObjects[i] = true;
                _activeCount++;
                return true;
            }
            return false;
        }

        public void Return(int id, int objectID)
        {
            Objects[id].objects[objectID].SetActive(false);
            Objects[id].activeObjects[objectID] = false;
            _activeCount--;
        }

        public void ReturnAll()
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                for (int j = 0; j < Objects[i].objects.Count; j++)
                {
                    if (Objects[i].activeObjects[j])
                        Return(i, j);
                }
            }
        }
    }
}
