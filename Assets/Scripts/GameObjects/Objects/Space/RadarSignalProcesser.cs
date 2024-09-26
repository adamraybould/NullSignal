using System;
using System.Collections;
using Corruption.Objects.Interactive;
using Corruption.Astro;
using Corruption.Core;
using UnityEngine;

namespace Corruption.Objects
{
    public class RadarSignalProcesser : MonoBehaviour
    {
        public event Action<Signal> OnSignalReceived;
        public event Action<Signal, int> OnSignalProcess;
        public event Action OnSignalProcessComplete;
        
        public event Action<Signal> OnSignalSaved;
        public event Action<Signal> OnSignalDisplay;
        public event Action<Signal> OnSignalDeleted;
        
        public Signal LoadedSignal { get; private set; }
        public bool IsProcessing { get; private set; }
        public bool IsProcessed { get; private set; }
        public float ProcessPercentage { get; private set; }
        
        [SerializeField] private SignalManager m_signalManager;
        [SerializeField] private float m_processSpeed;
        [SerializeField] private float m_processSpeedReduction;

        [Space, SerializeField] private Button m_saveSignalButton;
        [SerializeField] private Button m_sellSignalButton;
        [SerializeField] private Button m_deleteSignalButton;
        [SerializeField] private Button m_displaySignalButton;
        
        // Processed Signal Data
        private Signal m_processedSignal;
        private int m_currentProcessStage = 0;
        
        private RadarSatellite m_radar;

        private void Awake()
        {
            m_radar = GetComponent<RadarSatellite>();
        }

        private void Start()
        {
            m_radar.OnDetectionComplete += OnDetectionComplete;
            m_saveSignalButton.OnInteract += SaveSignal;
            m_displaySignalButton.OnInteract += DisplaySignal;
            m_sellSignalButton.OnInteract += SellSignal;
            m_deleteSignalButton.OnInteract += DeleteSignal;
        }

        public void StartProcessing()
        {
            if (!IsProcessing)
            {
                BeginProcessingScan();
            }
        }

        private void OnDetectionComplete(Signal signal)
        {
            if (signal == null)
                return;

            LoadedSignal = signal;
            OnSignalReceived?.Invoke(signal);
        }

        private void BeginProcessingScan()
        {
            if (LoadedSignal == null)
                return;
            
            IsProcessing = true;
            IsProcessed = false;
            StartCoroutine(ProcessSignal(LoadedSignal));
        }
        
        private IEnumerator ProcessSignal(Signal signal)
        {
            if (signal != null)
            {
                float elapsedTime = 0.0f;
                float totalScanTime = ((m_processSpeed * 3) * signal.GetDifficultyMultiplier()) - m_processSpeedReduction;
                totalScanTime = Mathf.Clamp(totalScanTime, 1.0f, 100.0f);
                ProcessPercentage = signal.DiscoveryPercentage;
                
                Debug.Log("Scan Time: " + totalScanTime);
                
                while (elapsedTime < totalScanTime)
                {
                    elapsedTime += Time.deltaTime;
                    ProcessPercentage = Mathf.Clamp01(elapsedTime / totalScanTime) * 100.0f;
                    signal.Scan(ProcessPercentage);

                    if (ProcessPercentage >= (m_currentProcessStage + 1) * (100.0f / 3))
                    {
                        m_currentProcessStage++;
                        OnSignalProcess?.Invoke(signal, m_currentProcessStage);
                    }

                    yield return null;
                }
                
                ProcessPercentage = 100.0f;
                
                IsProcessing = false;
                IsProcessed = true;
                m_processedSignal = signal;
                m_currentProcessStage = 0;
                
                OnSignalProcessComplete?.Invoke();
            }

            yield return null;
        }

        private void SaveSignal()
        {
            if (!IsProcessed)
                return;
            
            Debug.Log("Saving Signal");

            m_signalManager.SaveSignal(m_processedSignal);
            OnSignalSaved?.Invoke(m_processedSignal);
        }

        private void DeleteSignal()
        {
            if (!IsProcessed)
                return;
            
            Debug.Log("Deleting Signal");
            
            IsProcessed = false;
            IsProcessing = false;
            OnSignalDeleted?.Invoke(m_processedSignal);
        }

        private void DisplaySignal()
        {
            if (!IsProcessed)
                return;
            
            OnSignalDisplay?.Invoke(m_processedSignal);
        }

        private void SellSignal()
        {
            if (!IsProcessed)
                return;

            if (m_signalManager.SellSignal(m_processedSignal))
            {
                IsProcessed = false;
            }
        }
    }
}
