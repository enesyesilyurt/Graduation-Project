using System;
using System.Collections;
using System.Collections.Generic;
using Shadout.Controllers;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private AgentMovement agentMovement;
    private PlayerStates currentPlayerState = PlayerStates.End;
    public float DistanceTravelled => agentMovement.DistanceTravelled;
    public PlayerStates CurrentPlayerState => currentPlayerState;
    public event Action<PlayerStates, PlayerStates> PlayerStateChanged;

    public Vector3 target;

    private StateMachine stateMachine;
    private List<IState> states;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agentMovement = GetComponent<AgentMovement>();

        GameManager.Instance.GameStateChanged += OnGameStateChanged;
        InitStateMachine();
    }

    private void Update()
    {
        var closestDistance = PathManager.Instance.PathCreator.path.GetClosestPointOnPath(transform.position);

        if (Vector3.Distance(transform.position, closestDistance) > PathManager.Instance.RoadMeshCreator.roadWidth)
        {
            if (currentPlayerState == PlayerStates.Run || currentPlayerState == PlayerStates.Skate)
            {
                target = PathManager.Instance.PathCreator.path.GetPointAtDistance
                (
                    PathManager.Instance.PathCreator.path.GetClosestTimeOnPath(transform.position) + 80
                );
                UpdatePlayerState(PlayerStates.Fly);
            }
        }
        else if (transform.position.y >= closestDistance.y - .5f)
        {
            if (currentPlayerState == PlayerStates.Fly)
            {
                UpdatePlayerState(PlayerStates.Run);
            }
        }

        stateMachine.Tick();
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine();
        states = new List<IState>();

        var waitStart = new WaitStartState(transform, DistanceTravelled);
        var runState = new RunState(this, agentMovement, rb);
        var exitPathState = new ExitPathState(this, GetComponent<AgentMovement>(), 1);
        var goTargetState = new GoTargetState(this, rb, agentMovement.FlySpeed);

        states.Add(waitStart);
        states.Add(runState);
        states.Add(exitPathState);
        states.Add(goTargetState);

        stateMachine.AddTransition
        (
            waitStart,
            runState,
            () => currentPlayerState == PlayerStates.Run
        );

        stateMachine.AddTransition
        (
            runState,
            exitPathState,
            () => Vector3.Distance(PathManager.Instance.PathCreator.path.GetPointAtDistance
        (
            PathManager.Instance.PathCreator.path.GetClosestTimeOnPath(transform.position) + 80
        ), transform.position) < 20
        );

        stateMachine.AddTransition
        (
            exitPathState,
            goTargetState,
            () => currentPlayerState == PlayerStates.Fly
        );

        stateMachine.AddTransition
        (
            goTargetState,
            runState,
            () => currentPlayerState == PlayerStates.Run
        );

        stateMachine.SetState(waitStart);
    }

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
}
