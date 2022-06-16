using Shadout.Controllers;
using Shadout.Models;
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
        transform.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled);
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

        referenceObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled);
        referenceObject.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled + 1));

        transform.position = Vector3.Lerp
        (
            transform.position,
            PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled) + referenceObject.right * (sideMove * runSideMoveSpeed) + Vector3.up * playerHeight,
            .2f
        );

        followerObject.position = Vector3.Lerp(followerObject.position, transform.position, .2f);
        followerObject.LookAt(transform);
        transform.rotation = followerObject.rotation;
    }

	private void SetMovementRun()
    {
        distanceTravelled = PathManager.Instance.PathCreator.path.GetClosestDistanceAlongPath(transform.position);
        playerHeight = 0;
        currentSpeed =  runSpeed;

        rb.velocity = Vector3.zero;

        var tempTransform = transform;
        var rotation = tempTransform.rotation;
        rotation = new Quaternion(0, rotation.y, rotation.z, rotation.w);
        tempTransform.rotation = rotation;
        transform.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled + 1));

        sideMove = 0;
    }

	private void SetMovementFly()
    {
        transform.LeanRotateX(12, .2f).setEaseInCubic();
        transform.LeanMoveY(transform.position.y + 1, .2f).setEaseInCubic();
    }

	#endregion

    private void SetRotate(ContenderState newState)
    {
        transform.eulerAngles = Vector3.zero;
            transform.GetChild(0).transform.eulerAngles = Vector3.zero;

			if (newState == ContenderState.End)
			{
				transform.GetChild(0).transform.eulerAngles = Vector3.zero + Vector3.up * -90;
				transform.GetChild(0).transform.localPosition = Vector3.zero;
			}
    }

	#region Callbacks

    private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
    {
        switch (newState)
        {
            case ContenderState.WaitStart:
                SetRotate(newState);
                distanceTravelled = 5;

                    referenceObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled);
        		    referenceObject.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled + 1));

				    followerObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(DistanceTravelled - .2f) + Vector3.up * playerHeight;
        		    followerObject.LookAt(transform);

                    transform.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(5) + playerHeight * transform.up;
        		    transform.rotation = referenceObject.rotation;
                break;
            case ContenderState.Run:
                SetRotate(newState);
                SetMovementRun();
                break;
            case ContenderState.Skate:
                SetRotate(newState);
                SetMovementSkate();
                break;
            case ContenderState.Fly:
                SetMovementFly();
                break;
            case ContenderState.End:
                SetRotate(newState);
                if (GameManager.Instance.CurrentState != GameStates.End)
                {
                    UIManager.Instance.IsWinGame = true;
                    GameManager.Instance.UpdateGameState(GameStates.End);
                }
                break;
        }
    }

	#endregion
}