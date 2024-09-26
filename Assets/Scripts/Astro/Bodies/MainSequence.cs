using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Astro.Bodies
{
    public enum Category
    {
        RED_DWARF,
        YELLOW_DWARF,
        ORANGE_DWARF,
        BLUE_DWARF,
        WHITE_DWARF,
        BLACK_DWARF,
    }
    
    [CreateAssetMenu(fileName = "New Main Sequence Star", menuName = "Space/Stars/Main Sequence")]
    public class MainSequence : Star
    {
        
    }
}
