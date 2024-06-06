using System;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Represents the saved data for a player in the game
    /// This class includes all relevant information about a player's state and progress
    /// 
    /// </summary>
    [Serializable]
    public class PlayerSaveData
    {
        [Header("Character Info")]
        public string playerName = "";
        public string playerClass = "";

        [Header("Time Played")]
        public string secondsPlayed = "";
        public string timeStamp;
    }
}