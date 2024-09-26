using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Core.Framework;
using Corruption.Objects.Astro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Corruption.UI.World
{
    public class UI_StarSystemMarker : UI_StellarMarker
    {
        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI m_systemNameText;

        private StarSystemMarker m_starSystemMarker;

        protected override void Awake()
        {
            base.Awake();
            m_starSystemMarker = m_stellarMarker as StarSystemMarker;
        }

        public override void Display()
        {
            m_systemNameText.text = m_starSystemMarker.StarSystem.SystemName;
            
            base.Display();
        }
    }
}
