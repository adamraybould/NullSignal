using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using UnityEngine;

namespace Corruption.Objects.Astro
{
    public class GalaxyMapData : MonoBehaviour
    {
        public List<StarSystem> DiscoveredStarSystems => m_discoveredStarSystems;
        
        [SerializeField] private List<StarSystem> m_discoveredStarSystems;

        public StarSystem GetStarSystemByName(string systemName)
        {
            foreach (StarSystem starSystem in DiscoveredStarSystems)
            {
                if (starSystem.SystemName == systemName)
                {
                    return starSystem;
                }
            }
            
            Debug.LogError("Error: No Star System with the Name '" + systemName + "' Could Be Found");
            return null;
        }
    }
}
