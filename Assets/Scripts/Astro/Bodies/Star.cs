using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.Astro.Bodies
{
    public enum StarType
    {
        NULL,
        PROTOSTAR,
        MAIN_SEQUENCE,
        RED_GIANT,
        WHITE_DWARF,
        NEUTRON_STAR,
        BLACK_HOLE,
    }
    
    public enum SpectralClassification
    {
        NULL,
        O,
        B,
        A,
        F,
        G,
        K,
        M,
        NA,
    }
    
    public class Star : StellarBody
    {
        public StarType StarType => m_starType;
        public SpectralClassification Classification => m_classification;
        public float SolarMass => m_solarMass;
        
        [Header("Star Properties")]
        [SerializeField] private StarType m_starType;
        [SerializeField] private SpectralClassification m_classification;
        [SerializeField] private float m_solarMass;
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (m_starType == StarType.NEUTRON_STAR || m_starType == StarType.BLACK_HOLE || m_starType == StarType.WHITE_DWARF)
            {
                m_classification = SpectralClassification.NA;
            }
        }
        
        public override string GetBodyType() { return m_starType.ToString().Replace('_', ' '); }
    }
}
