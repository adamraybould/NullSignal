using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Core.Framework
{
    public abstract class InteractiveObject : MonoBehaviour, IInteractable
    {
        public event Action OnInteract;
        
        public string InteractionPrompt => m_prompt;
        [SerializeField] private string m_prompt;
        
        public virtual bool Interact()
        {
            OnInteract?.Invoke();
            return true;
        }
    }
}
