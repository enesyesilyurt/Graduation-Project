using System;
using System.Collections.Generic;
using Shadout.Controllers;
using UnityEngine;

[RequireComponent(typeof(AgentMovementController))]

public class Agent : ContenderBase
{
    [SerializeField] private float targetDistance;
    [SerializeField] private float targetFront;

    [HideInInspector] public Vector3 target;

    public float TargetDistance => targetDistance;
    public float TargetFront => targetFront;

    private StateMachine stateMachine;
    private List<IState> states;
    private Rigidbody rb;
    private float timer = 0;
    private float controlTime = 2;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ContenderStateChanged += OnContenderStateChanged;
        InitStateMachine();
    }

    private void Update()
    {
        if (timer < controlTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            var closestDistance = PathManager.Instance.PathCreator.path.GetClosestPointOnPath(transform.position);

            if (Vector3.Distance(transform.position, closestDistance) > PathManager.Instance.RoadMeshCreator.roadWidth)
            {
                if (currentContenderState == ContenderState.Run || currentContenderState == ContenderState.Skate)
                {
                    UpdateContenderState(ContenderState.Fly);
                }
            }
            else if (transform.position.y >= closestDistance.y - .5f)
            {
                if (currentContenderState == ContenderState.Fly)
                {
                    UpdateContenderState(ContenderState.Run);
                }
            }

        }
        
        stateMachine.Tick();
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine();
        states = new List<IState>();

        var waitStart = new WaitStartState(transform, DistanceTravelled);
        var runState = new RunState(this, (AgentMovementController)contenderMovement, rb);
        var exitPathState = new ExitPathState(this, (AgentMovementController)contenderMovement);
        var goTargetState = new GoTargetState(this, rb, contenderMovement.FlySpeed);

        states.Add(waitStart);
        states.Add(runState);
        states.Add(exitPathState);
        states.Add(goTargetState);

        stateMachine.AddTransition
        (
            waitStart,
            runState,
            () => CurrentContenderState == ContenderState.Run
        );

        stateMachine.AddTransition
        (
            runState,
            exitPathState,
            () =>
            {
                Vector3 position;
                return Vector3.Distance(PathManager.Instance.PathCreator.path.GetPointAtDistance
                (
                    PathManager.Instance.PathCreator.path.GetClosestTimeOnPath((position = transform.position)) + targetFront
                ), position) < targetDistance;
            });

        stateMachine.AddTransition
        (
            exitPathState,
            goTargetState,
            () => CurrentContenderState == ContenderState.Fly
        );

        stateMachine.AddTransition
        (
            goTargetState,
            runState,
            () => CurrentContenderState == ContenderState.Run
        );

        stateMachine.SetState(waitStart);
    }

    private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
    {
        if (newState == ContenderState.Run)
        {
            timer = 0;
        }
    }
}
