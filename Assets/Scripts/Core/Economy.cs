using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.Core
{
    public class Economy : Singleton<Economy>
    {
        public event Action OnPurchase;
        public event Action<int> OnSold;
        
        public int Credits { get; private set; } // Total number of Credits the Player has

        public void Sell(int value)
        {
            Credits += value;
            OnSold?.Invoke(Credits);
        }
    }
}
