using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Astro.Bodies
{
    [CreateAssetMenu(fileName = "Gas Giant", menuName = "Space/System/Planets/Gas Giant")]
    public class GasGiantPlanet : Planet
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
