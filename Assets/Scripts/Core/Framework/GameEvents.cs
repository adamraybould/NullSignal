using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Core.Framework
{
    public struct GameEvent
    {
        public event Action Event;

        public void Trigger()
        {
            Event?.Invoke();
        }
    }
    
    public struct GameEvent<T>
    {
        public event Action<T> Event;

        public void Trigger(T var)
        {
            Event?.Invoke(var);
        }
    }
    
    public static class GameEvents
    {
        public static GameEvent OnPlayerDeath;

        public static GameEvent PlayerInteractionFocusEnter; // On the Player Focusing into Monitors and other Similar Items
        public static GameEvent PlayerInteractionFocusExit; // On the Player Exiting Focus into Monitors and other Similar Items

        // UI Events
        public static GameEvent<bool> RequestLockMouse;
        public static GameEvent<MenuID> MenuRequest;
        
        public static GameEvent<MenuID> OnMenuOpened;
        public static GameEvent<MenuID> OnMenuClosed;
    }
}
