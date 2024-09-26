using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Entities.Player.Components
{
    public interface IPlayerComponent
    {
        public void OnInteractiveFocusEnter();
        public void OnInteractiveFocusExit();
    }
}
