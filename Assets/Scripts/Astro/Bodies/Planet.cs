using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Astro.Bodies
{
    public enum PlanetType
    {
        // Terrestrial
        EARTH_LIKE,
        DESERT,
        OCEAN,
        FOREST,
        TUNDRA,
        MOUNTAINOUS,
        VOLCANIC,
        BARREN,
        RUINED,
        
        // Gas Giants
        STANDARD,
        METALLIC,
        ICE,
        RINGED,
        DIAMOND_RAIN,
        EXOTIC,
    }
    
    public class Planet : StellarBody
    {
        [Header("Planet Properties")]
        [SerializeField] protected PlanetType m_planetType;

        public override string GetBodyType() { return m_planetType.ToString().Replace('_', ' '); }
    }
}
