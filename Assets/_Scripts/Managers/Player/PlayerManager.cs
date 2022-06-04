using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerAnimationManager))]
[RequireComponent(typeof(PlayerMovementManager))]
[RequireComponent(typeof(PlayerVehicleManager))]

public class PlayerManager : MonoBehaviour
{
    #region Variables

    private PlayerMovementManager playerMovement;
    private PlayerStates currentPlayerState;
    public float DistanceTravelled => playerMovement.DistanceTravelled;

    #endregion

    #region Properties

    public PlayerStates CurrentPlayerState => currentPlayerState;

    #endregion

    #region Acitons

    public event Action<PlayerStates, PlayerStates> PlayerStateChanged;

    #endregion

    #region Unity Methods

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovementManager>();

        GameManager.Instance.GameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        var closestDistance = PathManager.Instance.PathCreator.path.GetClosestPointOnPath(transform.position);

        if (Vector3.Distance(transform.position, closestDistance) > PathManager.Instance.RoadMeshCreator.roadWidth)
        {
            if(currentPlayerState == PlayerStates.Run || currentPlayerState == PlayerStates.Skate) { UpdatePlayerState(PlayerStates.Fly); }
        }
        else if(transform.position.y >= closestDistance.y - .5f)
        {
            if (currentPlayerState == PlayerStates.Fly) { UpdatePlayerState(PlayerStates.Run); }
        }
    }

    #endregion

    #region Methods

    public void UpdatePlayerState(PlayerStates newState)
    {
        PlayerStateChanged?.Invoke(currentPlayerState, newState);
        currentPlayerState = newState;
    }

    private void OnGameStateChanged(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Start:
                UpdatePlayerState(PlayerStates.WaitStart);
                break;
            case GameStates.Game:
                UpdatePlayerState(PlayerStates.Run);
                break;
            case GameStates.End:
                break;
        }
    }

    #endregion
}
