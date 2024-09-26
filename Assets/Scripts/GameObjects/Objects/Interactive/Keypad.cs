using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.Objects.Interactive
{
    public class Keypad : MonoBehaviour, IInteractable
    {
        public string InteractionPrompt => "Keypad";
        
        public bool Interact()
        {
            //GameEvents.MenuRequest.Trigger(MenuID.SYSTEM_MAP);
            return true;
        }
    }
}
