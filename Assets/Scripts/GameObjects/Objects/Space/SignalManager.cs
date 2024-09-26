using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Core;
using Corruption.Core.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.Objects
{
    public class SignalManager : Singleton<SignalManager>
    {
        public event Action<Signal> OnSignalSaved;
        public event Action<Signal> OnSignalDeleted;
        public event Action<Signal> OnSignalSold;
        
        private Dictionary<string, Signal> m_savedSignals; // List of Saved Signals that have been Processed

        protected override void Awake()
        {
            base.Awake();
            m_savedSignals = new Dictionary<string, Signal>();
        }
        
        public bool SaveSignal(Signal signal)
        {
            string signalID = signal.GetID();
            if (!m_savedSignals.ContainsKey(signalID))
            {
                m_savedSignals.Add(signalID, signal);
                OnSignalSaved?.Invoke(signal);
                return true;
            }

            Debug.LogError("Signal '" + signalID + "' Has Already Been Saved");
            return false;
        }

        public bool DeleteSignal(Signal signal)
        {
            string signalID = signal.GetID();
            if (m_savedSignals.ContainsKey(signalID))
            {
                m_savedSignals.Remove(signalID);
                OnSignalDeleted?.Invoke(signal);
                return true;
            }

            Debug.LogError("No Entry of Signal '" + signalID + "' Exists");
            return false;
        }

        public bool SellSignal(Signal signal)
        {
            string signalID = signal.GetID();
            if (m_savedSignals.ContainsKey(signalID))
            {
                Economy.Instance.Sell(signal.GetSellValue());
                DeleteSignal(signal);
                
                OnSignalSold?.Invoke(signal);
                return true;
            }
            
            //Debug.LogError("No Entry of Signal '" + data.ID + "' Exists");
            return false;
        }
    }
}
