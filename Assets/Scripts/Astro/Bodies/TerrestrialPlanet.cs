using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Astro.Bodies
{
    [CreateAssetMenu(fileName = "Terrestrial Planet", menuName = "Space/System/Planets/Terrestrial")]
    public class TerrestrialPlanet : Planet
    {
        private void OnEnable()
        {
            type = StellarBodyType.PLANET;
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();

            type = StellarBodyType.PLANET;
        }
    }
}
