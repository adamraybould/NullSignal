using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.Core
{
    public class SystemSettings : Singleton<SystemSettings>
    {
        [SerializedDictionary("Difficulty", "Multiplier"), SerializeField] private SerializedDictionary<SignalDifficulty, float> m_signalDifficulties; 

        public float GetSignalDifficultyMultiplier(SignalDifficulty difficulty)
        {
            if (m_signalDifficulties.Count <= 0)
            {
                Debug.LogError("No Signal Difficulties Defined");
                return 0.0f;
            }

            if (m_signalDifficulties.ContainsKey(difficulty))
            {
                return m_signalDifficulties[difficulty];
            }
            
            Debug.LogError(difficulty.ToString() + " Difficulty hasn't been Defined in System Settings");
            return 0.0f;
        }
    }
}
