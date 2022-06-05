using System;
using NoName.Utilities;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action<GameStates> GameStateChanged;

    private void Awake() 
    {
        InputSystem.Instance.Clicked += OnClicked;
    }

    private void Start() 
    {
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
