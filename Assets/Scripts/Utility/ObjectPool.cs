using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEditor;
using UnityEngine;

namespace Corruption.Utility
{
    public class PoolObject : MonoBehaviour
    {
        public Action<PoolObject> OnReturn;
        
        public Pool Pool { get; private set; }
        public Transform Parent { get; private set; }

        private IPoolable m_poolable;
        
        public void Create(Pool pool, IPoolable poolable, Transform parent)
        {
            Pool = pool;
            m_poolable = poolable;
            Parent = parent;

            m_poolable.ReturnToPool += OnReturnToPool;
        }

        private void OnReturnToPool() { OnReturn?.Invoke(this); }
    }
    
    [Serializable]
    public class Pool
    {
        public string ID => m_id;
        public GameObject PoolableObject => m_poolableObject;
        public int Count => m_count;

        [SerializeField] private string m_id;
        [SerializeField] private GameObject m_poolableObject;
        [SerializeField] private int m_count;

        private List<PoolObject> m_pool = new List<PoolObject>();
        private int m_activePoolObjects;
        
        public void AddToPool(GameObject poolableObject, IPoolable poolable, Transform parent)
        {
            PoolObject poolObject = poolableObject.AddComponent<PoolObject>();
            poolObject.Create(this, poolable, parent);
            poolObject.OnReturn += OnReturnToPool;
            
            m_pool.Add(poolObject);
        }

        public PoolObject GetIdlePoolObject()
        {
            if (m_activePoolObjects < m_count)
            {
                foreach (PoolObject poolObject in m_pool)
                {
                    if (!poolObject.gameObject.activeSelf)
                    {
                        m_activePoolObjects++;
                        return poolObject;
                    }
                }
            }

            Debug.LogError("Max Pool Count Reached for Pool '" + m_id + "'");
            return null;
        }

        public void ResetPool()
        {
            m_poolableObject = null;
            m_pool.Clear();
        }
        
        private void OnReturnToPool(PoolObject poolObject)
        {
            poolObject.transform.parent = poolObject.Parent;
            poolObject.gameObject.transform.localPosition = Vector3.zero;
            poolObject.gameObject.SetActive(false);
            
            m_activePoolObjects--;
        }
    }
    
    public class ObjectPool : Singleton<ObjectPool>
    {
        [SerializeField] private List<Pool> m_pools;
        
        protected override void Awake()
        {
            base.Awake();
            
            // Instantiate Object Pools
            foreach (Pool pool in m_pools)
            {
                Transform poolHolder = transform.Find(pool.ID);
                if (poolHolder == null)
                {
                    poolHolder = new GameObject(pool.ID).transform;
                    poolHolder.SetParent(transform);
                    poolHolder.localPosition = Vector3.zero;
                }

                for (int i = 0; i < pool.Count; i++)
                {
                    GameObject gameObject = Instantiate(pool.PoolableObject, poolHolder);
                    gameObject.SetActive(false);
                    pool.AddToPool(gameObject, gameObject.GetComponent<IPoolable>(), poolHolder);
                }
            }
        }

        public GameObject GetPooledObject(string ID)
        {
            foreach (Pool pool in m_pools)
            {
                if (pool.ID == ID)
                {
                    GameObject gameObject = pool.GetIdlePoolObject().gameObject;
                    gameObject.SetActive(true);
                    return gameObject;
                }
            }
            
            Debug.LogError("Unable to Get Pooled Object from Pool '" + ID + "'");
            return null;
        }
        
        public T GetPooledObject<T>(string ID, Transform parent)
        {
            foreach (Pool pool in m_pools)
            {
                if (pool.ID == ID)
                {
                    GameObject gameObject = pool.GetIdlePoolObject().gameObject;
                    gameObject.transform.parent = parent;
                    gameObject.transform.position = Vector3.zero;
                    gameObject.SetActive(true);
                    
                    return gameObject.GetComponent<T>();
                }
            }
            
            Debug.LogError("Unable to Get Pooled Object from Pool '" + ID + "'");
            return default(T);
        }

        private void OnValidate()
        {
            foreach (Pool pool in m_pools)
            {
                if (pool.PoolableObject == null)
                    continue;
                
                if (pool.PoolableObject.GetComponent<IPoolable>() == null)
                {
                    EditorUtility.DisplayDialog("Invalid Type", "Game Object isn't Poolable", "OK");
                    pool.ResetPool();
                }
            }
        }
    }
}
