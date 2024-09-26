using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Corruption.Objects
{
    public class SignalVisual : MonoBehaviour
    {
        public Echo Echo => echo;
        
        [FormerlySerializedAs("pulse")] [SerializeField] private Echo echo;
        [SerializeField] private int m_minLifeTime;
        [SerializeField] private int m_maxLifeTime;

        [SerializeField] private ParticleSystem m_PS;

        private void Awake()
        {
            Create(echo);
        }

        private void Start()
        {
            //gameObject.name = "Signal_" + Pulse.Body.Name + "_";
        }

        public void Create(Echo echo)
        {
            this.echo = echo;
            
            var main = m_PS.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(m_minLifeTime, m_maxLifeTime);
            //main.startColor = Echo.SignalColor;
        }
    }
}
