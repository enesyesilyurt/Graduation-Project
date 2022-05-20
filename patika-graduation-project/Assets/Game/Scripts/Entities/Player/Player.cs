using System;
using Helpers;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    // #region SerializeFields

    // #endregion

    // #region Properties


    // #endregion

    // #region Variables

    public float DistanceTravelled;

    // private PlayerStates playerState;

    // private bool isOnRoad = true;


    // #endregion

    // #region Acitons

    // public event Action<PlayerStates> AfterPlayerStateChanged;
    // public event Action<PlayerStates> BeforePlayerStateChanged;


    // #endregion

    // #region Unity Methods
    // private void Start()
    // {
    //     GameManager.Instance.GameStateChanged += OnGameStateChanged;
    // }

    // private void LateUpdate()
    // {
    //     DistanceTravelled += currentSpeed * Time.deltaTime;

    //     var closestDistance = pathCreator.path.GetClosestPointOnPath(transform.position);
    //     if (Vector3.Distance(transform.position, closestDistance) > roadMesh.roadWidth)
    //     {
    //         if(isOnRoad)
    //         {
    //             isOnRoad = false;
    //             UpdatePlayerState(PlayerStates.Fly);
    //         }
    //         else
    //             FlyMovement();
    //     }
    //     else if(transform.position.y >= closestDistance.y - .5f)
    //     {
    //         if (!isOnRoad)
    //         {
    //             isOnRoad = true;
    //             UpdatePlayerState(PlayerStates.Run);
    //         }
    //         else
    //             RunMovement();
    //     }
    //     else
    //         FlyMovement();
    // }

    // #endregion

    // #region Methods

    public void BoostSpeed(float boost)
    {
    //     var tempSpeed = currentSpeed;
    //     currentSpeed *= boost;
    //     LeanTween.delayedCall(2, ()=> 
    //     {
    //         if(currentSpeed*boost == tempSpeed) 
    //             currentSpeed = tempSpeed;
    //     });
    }

    public void UpdatePlayerState(PlayerStates newState)
    {
    //     BeforePlayerStateChanged?.Invoke(playerState);

    //     playerState = newState;

    //     switch (newState)
    //     {
    //         case PlayerStates.WaitStart:
    //             WaitStart();
    //             break;
    //         case PlayerStates.Run:
    //             SetMovementRun();
    //             break;
    //         case PlayerStates.Skate:
    //             SetMovementSkate();
    //             break;
    //         case PlayerStates.Fly:
    //             SetMovementFly();
    //             break;
    //         case PlayerStates.End:
    //             break;
    //     }

    //     AfterPlayerStateChanged?.Invoke(newState);
    }

    // private void OnGameStateChanged(GameStates newState)
    // {
    //     switch (newState)
    //     {
    //         case GameStates.Start:
    //             UpdatePlayerState(PlayerStates.WaitStart);
    //             break;
    //         case GameStates.Game:
    //             UpdatePlayerState(PlayerStates.Run);
    //             break;
    //         case GameStates.End:
    //             break;
    //     }
    // }

    // #endregion
}
