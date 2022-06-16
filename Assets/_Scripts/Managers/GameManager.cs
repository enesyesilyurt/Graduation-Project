using System;
using Shadout.Models;
using Shadout.Utilities;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameStates currentState;
    public GameStates CurrentState => currentState;

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
    }

    #endregion

    #region Methods

    private void LoadGame()
    {
        SaveSystem.Instance.InitSaveSystem();
    }

    public void UpdateGameState(GameStates newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case GameStates.Start:
                InputSystem.Instance.Clicked += OnClicked;
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

    private void OnClicked()
    {
        InputSystem.Instance.Clicked -= OnClicked;
        UpdateGameState(GameStates.Game);
    }

    #endregion
}
