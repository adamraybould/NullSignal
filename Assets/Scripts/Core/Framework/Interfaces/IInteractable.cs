using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Core.Framework
{
    public interface IInteractable
    {
        public string InteractionPrompt { get; }
        public bool Interact();
    }
}
