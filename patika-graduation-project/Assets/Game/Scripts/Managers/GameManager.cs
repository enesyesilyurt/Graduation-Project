using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public event Action<GameStates> GameStateChanged;

    private void Start() 
    {
        InputSystem.Instance.Clicked += OnClicked;
        UpdateGameState(GameStates.Start);  
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

    private void OnClicked(Touch touch)
    {
        if(touch.phase != TouchPhase.Began) return;
        
        InputSystem.Instance.Clicked -= OnClicked;
        UpdateGameState(GameStates.Game);
    }

}
