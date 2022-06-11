using System;
using Shadout.Models;
using Shadout.Utilities;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Events

    public event Action<GameStates> GameStateChanged;

    #endregion

    #region Unity Methods

    private void Awake() 
    {
        LoadGame();
    }

    private void Start() 
    {
        UpdateGameState(GameStates.Start);

        InputSystem.Instance.Clicked += OnClicked;
    }

    #endregion

    #region Methods

    private void LoadGame()
    {
        SaveSystem.Instance.InitSaveSystem();
    }

    public void UpdateGameState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Start:
                break;
            case GameStates.Game:
                break;
            case GameStates.End:
                break;
        }
        GameStateChanged?.Invoke(newState);
    }

    #endregion

    #region CallBacks

    private void OnClicked(Touch touch)
    {
        if(touch.phase != TouchPhase.Began) return;
        
        InputSystem.Instance.Clicked -= OnClicked;
        UpdateGameState(GameStates.Game);
    }

    #endregion
}
