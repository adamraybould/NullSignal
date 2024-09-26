using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.Objects.Astro
{
    public class StellarMarker : MonoBehaviour, IPoolable
    {
        public event Action ReturnToPool;
        
        public event Action OnSelected;
        public event Action OnHighlighted;
        public event Action OnDehighlighted;

        public virtual void Select(GalaxyMap galaxyMap)
        {
            OnSelected?.Invoke();
        }

        public virtual void Highlight(GalaxyMap galaxyMap)
        {
            OnHighlighted?.Invoke();
        }
        
        public virtual void Dehighlight(GalaxyMap galaxyMap)
        {
            OnDehighlighted?.Invoke();
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
    }
}
