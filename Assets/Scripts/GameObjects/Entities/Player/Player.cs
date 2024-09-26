using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using Corruption.Core.Input;
using Corruption.Entities.Player.Components;
using UnityEngine;

namespace Corruption.Entities.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInteractor))]
    public class Player : MonoBehaviour
    {
        public event Action OnDeath;

        public int Health { get => m_healthValue; set => m_healthValue = value; }
        public int Stamina { get => m_staminaValue; set => m_staminaValue = value; }
        public int Hunger { get => m_hungerValue; set => m_hungerValue = value; }

        public bool IsFocused { get; private set; } // Is the Player currently Interacting with Items like a Monitor?
        
        [SerializeField] private int m_healthValue = 100;
        [SerializeField] private int m_staminaValue = 100;
        [SerializeField] private int m_hungerValue = 100;

        private List<IPlayerComponent> m_playerComponents;
        
        private void Awake()
        {
            m_playerComponents = new List<IPlayerComponent>();
            foreach (IPlayerComponent child in transform.GetComponentsInChildren<IPlayerComponent>())
            {
                m_playerComponents.Add(child);
            }
        }

        private void Start()
        {
            GameEvents.PlayerInteractionFocusEnter.Event += OnInteractiveFocusEnter;
            GameEvents.PlayerInteractionFocusExit.Event += OnInteractiveFocusExit;
        }

        public void Damage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Kill();
            }
        }

        private void Kill()
        {
            Health = 0;
            OnDeath?.Invoke();
        }

        private void OnInteractiveFocusEnter()
        {
            IsFocused = true;
            m_playerComponents.ForEach(obj => obj.OnInteractiveFocusEnter());
        }

        private void OnInteractiveFocusExit()
        {
            IsFocused = false;
            m_playerComponents.ForEach(obj => obj.OnInteractiveFocusExit());
        }
    }
}
