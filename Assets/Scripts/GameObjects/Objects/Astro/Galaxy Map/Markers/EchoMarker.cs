using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Core.Framework;
using Corruption.Objects.Astro;
using UnityEngine;

namespace Corruption.Objects.Astro
{
    public class EchoMarker : StellarMarker
    {
        public event Action OnDetected;

        public Echo Echo => m_echo;
        [SerializeField] private Echo m_echo;

        public override void Select(GalaxyMap galaxyMap)
        {
            base.Select(galaxyMap);

            StarSystem starSystem = Echo.Source as StarSystem;
            galaxyMap.OpenSystemMap(starSystem);
        }

        public void AttachEcho(Echo echo)
        {
            m_echo = echo;
        }

        public void Detect()
        {
            OnDetected?.Invoke();
        }
    }
}
