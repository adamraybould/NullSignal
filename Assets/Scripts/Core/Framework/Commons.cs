using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.Core.Framework
{
    public enum MenuID
    {
        MAIN_MENU = 0,
    }

    public enum GameDifficulty
    {
        EASY,
        NORMAL,
        HARD,
    }
    
    public enum SignalDifficulty
    {
        EASY,
        NORMAL,
        HARD,
        EXTREME,
        IMPOSSIBLE,
    }

    public enum EchoRarity
    {
        VERY_COMMON = 60,
        COMMON = 40,
        UNCOMMON = 15,
        RARE = 10,
        VERY_RARE = 4,
        EXOTIC = 1,
    }
    
    [Serializable]
    public struct SignalScanDifficulty
    {
        public SignalDifficulty Difficulty => m_difficulty;
        public float Multiplier => m_difficultyMultiplier;

        [SerializeField] private SignalDifficulty m_difficulty;
        [SerializeField] private float m_difficultyMultiplier;
    }

    [Serializable]
    public struct MinMaxRange
    {
        public float Min => m_min;
        public float Max => m_max;
        
        [SerializeField] private float m_min;
        [SerializeField] private float m_max;

        public float GetRandomRange()
        {
            return Random.Range(Min, Max);
        }
    }
}
