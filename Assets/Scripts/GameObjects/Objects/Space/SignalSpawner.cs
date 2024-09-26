using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.Objects
{
    public class SignalSpawner : MonoBehaviour
    {
        public static SignalSpawner Instance { get; private set; }

        public List<SignalVisual> ActiveSignals => m_activeSignals;
        
        [SerializeField] private List<Echo> m_signals; // List of possible Signals

        [SerializeField] private int m_maxSignals;
        [SerializeField] private BoxCollider m_spawnRegion;
        
        private List<SignalVisual> m_activeSignals;
        
        private void Awake()
        {
            InitialiseSingleton();

            m_activeSignals = new List<SignalVisual>();
        }

        private void Start()
        {
            for (int i = 0; i < m_maxSignals; i++)
            {
                SpawnSignal();
            }
        }

        private void SpawnSignal()
        {
            // Don't Spawn Above Max Signals
            if (ActiveSignals.Count >= m_maxSignals)
                return;

            Vector3 randomSpawnPos = GetRandomPositionInRegion();
            GameObject signalGameObject = ObjectPool.Instance.GetPooledObject("Signal");
            signalGameObject.transform.position = randomSpawnPos;
            
            SignalVisual signal = signalGameObject.GetComponent<SignalVisual>();
            signal.Create(GetRandomSignal());
            m_activeSignals.Add(signal);
        }

        private Vector3 GetRandomPositionInRegion()
        {
            Bounds bounds = m_spawnRegion.bounds;
            float offsetX = m_spawnRegion.transform.position.x +  Random.Range(-bounds.extents.x, bounds.extents.x);
            float offsetY = m_spawnRegion.transform.position.y + Random.Range(-bounds.extents.y, bounds.extents.y);
            float offsetZ = m_spawnRegion.transform.position.z + Random.Range(-bounds.extents.z, bounds.extents.z);

            return new Vector3(offsetX, offsetY, offsetZ);
        }

        private Echo GetRandomSignal()
        {
            int index = Random.Range(0, m_signals.Count - 1);
            return m_signals[index];
        }
        
        private void InitialiseSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}
