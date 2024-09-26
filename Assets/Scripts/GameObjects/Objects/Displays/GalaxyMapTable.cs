using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using Corruption.Objects.Astro;
using UnityEngine;

namespace Corruption.Objects.Displays
{
    public class GalaxyMapTable : Focusable
    {
        [Header("Map Properties")] 
        [SerializeField] private GalaxyMap m_galaxyMap;
        
        public override void Focus()
        {
            m_galaxyMap.Activate();
            base.Focus();
        }

        public override bool UnFocus()
        {
            if (!m_galaxyMap.IsViewingSystemMap)
            {
                m_galaxyMap.Deactivate();
                return base.UnFocus();
            }

            return false;
        }
    }
}
