using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using UnityEngine;

namespace Corruption.Objects.Astro
{
    public class StarSystemMarker : StellarMarker
    {
        public StarSystem StarSystem => m_starSystem;
        
        [SerializeField] private StarSystem m_starSystem;
        
        public override void Select(GalaxyMap galaxyMap)
        {
            base.Select(galaxyMap);
            
            galaxyMap.OpenSystemMap(m_starSystem);
        }

        public void AssignSystem(StarSystem starSystem)
        {
            m_starSystem = starSystem;
        }
    }
}
