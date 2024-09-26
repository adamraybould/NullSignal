using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Corruption.Astro;
using Corruption.Core.Framework;
using Corruption.Objects.Astro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.Objects
{
    public class EchoSpawner : MonoBehaviour
    {
        public event Action<Vector3, Echo> OnSpawnEcho;
        
        [SerializeField] private EchoRadar m_radar;
        
        [Header("Echo Properties")]
        [SerializedDictionary("Rarity", "Echo"), SerializeField] private SerializedDictionary<EchoRarity, List<Echo>> m_echoes; 
        
        private BoxCollider m_spawnRegion;

        [Header("Echo Spawn Properties")] 
        [SerializeField] private MinMaxRange m_distanceOffset;

        private void Awake()
        {
            m_spawnRegion = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            RunChecks();

            m_radar.OnPulse += SpawnEchoes;
        }

        public void SpawnEchoes(float pulseRange)
        {
            List<Echo> echoes = GetPossibleEchoes();
            if (echoes.Count > 0)
            {
                foreach (Echo echo in echoes)
                {
                    if (!m_radar.HasEchoBeenDiscovered(echo))
                    {
                        StarSystem system = echo.Source as StarSystem;
                        Debug.Log("Detected Echo: " + system.SystemName);
                        Vector3 echoPosition = GetEchoPosition(echo);
                        OnSpawnEcho?.Invoke(echoPosition, echo);
                    }
                }
            }
        }

        private List<Echo> GetPossibleEchoes()
        {
            List<Echo> possibleEchoes = new List<Echo>();
            foreach (KeyValuePair<EchoRarity, List<Echo>> echos in m_echoes)
            {
                int randomPercentage = Random.Range(0, 100);
                if (randomPercentage <= (int)echos.Key)
                {
                    int randomIndex = Random.Range(0, echos.Value.Count);
                    possibleEchoes.Add(echos.Value[randomIndex]);
                }
            }

            return possibleEchoes;
        }
        
        private Vector3 GetPositionWithinSpawnRegion()
        {
            Bounds bounds = m_spawnRegion.bounds;
            float offsetX = m_spawnRegion.transform.position.x +  Random.Range(-bounds.extents.x, bounds.extents.x);
            float offsetY = m_spawnRegion.transform.position.y + Random.Range(-bounds.extents.y, bounds.extents.y);
            float offsetZ = m_spawnRegion.transform.position.z + Random.Range(-bounds.extents.z, bounds.extents.z);

            return new Vector3(offsetX, offsetY, offsetZ);
        }

        private Vector3 GetEchoPosition(Echo echo)
        {
            int randomDistanceMultiplier = Random.value < 0.5f ? -1 : 1;

            float randomDistance = (echo.Source.Distance + m_distanceOffset.GetRandomRange()) * randomDistanceMultiplier;
            return new Vector3(randomDistance, Mathf.Clamp(Mathf.Abs(randomDistance), 20, 150), randomDistance);
        }
        
        private void RunChecks()
        {
            if (m_radar == null)
            {
                Debug.LogError("ERROR: Radar Not Defined. Deactivating");
                enabled = false;
            }
            
            if (m_spawnRegion == null)
            {
                Debug.LogError("ERROR: Spawn Region Not Defined for Pulse Spawner. Deactivating");
                enabled = false;
            }

            if (m_echoes.Count <= 0)
            {
                Debug.Log("No Pulses Defined within the Pulse Spawner");
            }
        }
    }
}
