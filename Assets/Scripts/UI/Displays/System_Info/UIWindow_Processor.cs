using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Objects;
using TMPro;
using UnityEngine;

namespace Corruption.UI.Display
{
    public class UIWindow_Processor : UIWindow
    {
        [SerializeField] private RadarSignalProcesser m_signalProcessor;
        
        [Header("Processor UI")] 
        [SerializeField] private GameObject m_signalDetectedUI;
        [SerializeField] private TextMeshProUGUI m_detectionHeader;
        [SerializeField] private TextMeshProUGUI m_starID;
        [SerializeField] private TextMeshProUGUI m_starType;
        [SerializeField] private TextMeshProUGUI m_starClass;
        [SerializeField] private TextMeshProUGUI m_processPercentage;
        
        [Space, SerializeField] private GameObject m_noSignalDetectedUI;

        private bool m_isProcessing;

        private void Start()
        {
            m_signalProcessor.OnSignalReceived += OnSignalReceived;
            m_signalProcessor.OnSignalProcess += OnSignalProcess;
            m_signalProcessor.OnSignalProcessComplete += OnSignalProcessComplete;
            m_signalProcessor.OnSignalSaved += ctx => OnSignalSaved();
            m_signalProcessor.OnSignalDeleted += ctx => OnSignalDeleted();
        }

        private void Update()
        {
            m_processPercentage.text = m_signalProcessor.ProcessPercentage.ToString("0") + "%";
        }

        private void OnSignalReceived(Signal signal)
        {
            m_detectionHeader.text = "Processing Signal";
            m_starID.text = "?";
            m_starType.text = "?";
            m_starClass.text = "?";
            
            m_animator.SetTrigger("Intro");
        }

        private void OnSignalProcess(Signal signal, int stage)
        {
            switch (stage)
            {
                case 1:
                {
                    m_starID.text = signal.StellarBody.Name; 
                    PlayAudio(1);
                    break;
                }
                
                case 2:
                {
                    m_starType.text = signal.StellarBody.GetBodyType(); 
                    PlayAudio(1);
                    break;
                }
                
                case 3:
                {
                    m_starClass.text = signal.StellarBody.Type.ToString();
                    break;
                }
            }
        }

        private void OnSignalProcessComplete()
        {
            m_isProcessing = false;
            m_detectionHeader.text = "Signal Processed";
            m_animator.SetTrigger("Complete");
            PlayAudio(2);
        }

        private void OnSignalSaved()
        {
            m_animator.SetTrigger("Saved");
        }

        private void OnSignalDeleted()
        {
            m_animator.SetTrigger("Deleted");
        }
        
        public void Anim_CanBeginProcessing() { m_signalProcessor.StartProcessing(); }
    }
}
