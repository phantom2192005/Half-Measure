using System;
using UnityEngine;

namespace Half_Measure.Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] NetworkManager photonManager;

        public event Action<GameState> OnGameStateChanged;
    }

    public enum GameState
    {
        MainMenu,
        CreateRoom,
        Playing,
        Pause,
        Exit
    }
}

