using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPHive.Game
{
    public class ObjectPooling : Singleton<ObjectPooling>
    {
        public ObjectToPool[] objectsToPool;

        public delegate void OnGetFromPool();
        public static event OnGetFromPool GotFromPool;

        void Awake()
        {
            InitilaizePool();
        }

        public GameObject GetFromPool(string name)
        {
            GameObject _objectToReturn = null;
            foreach (ObjectToPool pool in objectsToPool)
            {
                if (pool.name == name)
                {
                    for (int i = 0; i < pool.pooledObjects.Count; i++)
                    {
                        if (!pool.pooledObjects[i].activeInHierarchy)
                            _objectToReturn = pool.pooledObjects[i];
                    }

                    if (_objectToReturn == null)
                    {
                        for (int i = 0; i < pool.expandAmount; i++)
                        {
                            GameObject _pooled = Instantiate(pool.objectToPool, transform);
                            _pooled.name = pool.name;
                            _pooled.SetActive(false);
                            pool.pooledObjects.Add(_pooled);
                            _objectToReturn = _pooled;
                        }
                    }
                }
            }
            GotFromPool?.Invoke();
            return _objectToReturn;
        }
        public void Deposit(GameObject gameObject)
        {
            foreach (ObjectToPool pool in objectsToPool)
            {
                foreach (GameObject go in pool.pooledObjects)
                {
                    if (gameObject == go)
                    {
                        go.transform.SetParent(transform);

                        gameObject.SetActive(false);
                        gameObject.transform.ResetTransform();

                        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                            rigidbody.ResetVelocity();

                        return;
                    }
                }
            }

            Debug.LogError("Trying to deposit an object that isn't in the pool.");
        }

        public void DepositAll()
        {
            foreach (ObjectToPool pool in objectsToPool)
            {
                foreach (GameObject gameObject in pool.pooledObjects)
                {
                    gameObject.SetActive(false);
                    gameObject.transform.ResetTransform();
                    gameObject.transform.SetParent(transform);

                    if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                        rigidbody.ResetVelocity();
                }
            }
        }

        public void Clear()
        {
            foreach (ObjectToPool pool in objectsToPool)
            {
                foreach (GameObject gameObject in pool.pooledObjects)
                {
                    Destroy(gameObject);
                }
                pool.pooledObjects.Clear();
            }
        }

        public void InitilaizePool()
        {
            foreach (ObjectToPool pool in objectsToPool)
            {
                for (int i = 0; i < pool.poolCount; i++)
                {
                    GameObject _pooled = Instantiate(pool.objectToPool, transform);
                    _pooled.name = pool.name;
                    _pooled.SetActive(false);
                    pool.pooledObjects.Add(_pooled);
                }
            }
        }

    }
}
