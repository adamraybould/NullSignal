using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Astro.Bodies
{
    public enum StellarBodyType
    {
        STAR,
        PLANET,
    }
    
    public abstract class StellarBody : ScriptableObject
    {
        public string Name => m_name;
        public StellarBodyType Type => type;
        public Material BodyMaterial => m_bodyMaterial;

        [SerializeField] protected string m_name;
        [SerializeField] protected StellarBodyType type;
        [SerializeField] private Material m_bodyMaterial;
        
        protected virtual void OnValidate()
        {
            
        }
        
        public abstract string GetBodyType();
        public string GetStringOfType() { return Type.ToString().Replace('_', ' '); }
    }
}
