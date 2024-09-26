using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro.Bodies;
using UnityEngine;

namespace Corruption.Astro
{
    [Serializable]
    public class Signal
    {
        public event Action OnDiscovered;

        public StellarBody StellarBody => m_stellarBody; // Get Rid of this Eventually!
        
        public bool Discovered { get; private set; }
        public float DiscoveryPercentage { get; private set; }

        private string m_ID;
        private Echo m_echo;
        private StellarBody m_stellarBody;
        
        public Signal(Echo echo)
        {
            m_ID = Guid.NewGuid().ToString();
            m_echo = echo;
            //m_stellarBody = pulse.Body;
            
            Discovered = false;
            DiscoveryPercentage = 0.0f;
        }

        public void Scan(float percentage)
        {
            DiscoveryPercentage = percentage;
            if (DiscoveryPercentage >= 100.0f)
            {
                Discovered = true;
                DiscoveryPercentage = 100.0f;
                
                OnDiscovered?.Invoke();
            }
        }
        
        public string GetID() { return m_ID; } 
        public string GetBodyName() { return Discovered ? m_stellarBody.Name : "???"; }
        public string GetBodyType() { return Discovered ? m_stellarBody.GetBodyType() : "???"; }
        
        public float GetDifficultyMultiplier() { return m_echo.GetDifficultyMultiplier(); }
        public int GetSellValue() { return 0; }
    }
}
