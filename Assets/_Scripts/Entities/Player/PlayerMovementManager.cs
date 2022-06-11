using Shadout.Controllers;
using UnityEngine;

public class PlayerMovementManager : ContenderMovementBase
{
	#region Variables

    private float sideMove;

	#endregion

    #region Props

    #endregion

	#region Unity Methods

    protected override void Update() 
    {
        base.Update();
        Move();
    }

	#endregion

	#region Methods

    public override void Init()
    {
        base.Init();
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
		contender.ContenderStateChanged += OnContenderStateChanged;
    }

    private void Move()
    {
        switch (contender.CurrentContenderState)
        {
            case ContenderState.Run:
                OnRoadMovement();
                break;
            case ContenderState.Skate:
                OnRoadMovement();
                break;
            case ContenderState.Fly:
                FlyMovement();
                break;
        }
    }

	private void FlyMovement()
    {
        float sideInput = InputSystem.Instance.SideInput;
        var transform1 = transform;
        transform1.eulerAngles = new Vector3(12, transform1.eulerAngles.y, 0);
        rb.velocity = transform1.forward * (flySpeed * Time.deltaTime);

        transform.Rotate(Vector3.up * (sideInput * Time.deltaTime * flySideMoveSpeed));
    }

	private void OnRoadMovement()
    {
        float sideInput = InputSystem.Instance.SideInput;
        sideMove += sideInput * Time.deltaTime;

        referenceObject.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        referenceObject.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled + 1));

        Transform tempTransform;
        (tempTransform = transform).position = Vector3.Lerp
        (
            transform.position,
            pathCreator.path.GetPointAtDistance(distanceTravelled) + referenceObject.right * (sideMove * runSideMoveSpeed) + Vector3.up * playerHeight,
            .2f
        );

        followerObject.position = Vector3.Lerp(followerObject.position, tempTransform.position, .2f);
        followerObject.LookAt(transform);
        transform.rotation = followerObject.rotation;
    }

	private void SetMovementRun()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        playerHeight = 0;
        currentSpeed =  runSpeed;

        rb.velocity = Vector3.zero;

        var tempTransform = transform;
        var rotation = tempTransform.rotation;
        rotation = new Quaternion(0, rotation.y, rotation.z, rotation.w);
        tempTransform.rotation = rotation;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled + 1));

        sideMove = 0;
    }

	private void SetMovementFly()
    {
        transform.LeanRotateX(12, .2f).setEaseInCubic();
        transform.LeanMoveY(transform.position.y + 1, .2f).setEaseInCubic();
    }

	#endregion

	#region Callbacks

    private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
    {
        switch (newState)
        {
            case ContenderState.WaitStart:
                break;
            case ContenderState.Run:
                SetMovementRun();
                break;
            case ContenderState.Skate:
                SetMovementSkate();
                break;
            case ContenderState.Fly:
                SetMovementFly();
                break;
            case ContenderState.End:
                GameManager.Instance.UpdateGameState(GameStates.End);
                break;
        }
    }

	#endregion
}