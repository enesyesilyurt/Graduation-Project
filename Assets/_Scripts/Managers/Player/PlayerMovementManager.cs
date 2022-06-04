using System;
using PathCreation;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
	#region SerializedFields

	[Header("Run")]

    [SerializeField] private float runSpeed = 5;
    [SerializeField] private float runSideMoveSpeed = 5;

	[Header("Fly")]

    [SerializeField] private float flySpeed;
    [SerializeField] private float flySideMoveSpeed;

    [Header("Skate")]

    [SerializeField] private float skateSpeed;

	#endregion

	#region Variables

	private PlayerManager player;
    private PathCreator pathCreator;
    private Rigidbody rb;
    private Transform followerObject;
    private Transform referanceObject;
    private float distanceTravelled;
    private float currentSpeed;
    private float playerHeight;
    private float sideMove;

	#endregion

    #region Props

    public float DistanceTravelled => distanceTravelled;

    #endregion

	#region Unity Methods

	private void Awake() 
	{
        pathCreator = PathManager.Instance.PathCreator;
        var position = transform.position;
        referanceObject = Instantiate(new GameObject(), pathCreator.path.GetClosestPointOnPath(position), Quaternion.identity).transform;
        followerObject = Instantiate(new GameObject(), position - transform.forward * - .2f, Quaternion.identity).transform;

        distanceTravelled = 5;
		player = GetComponent<PlayerManager>();
		rb = GetComponent<Rigidbody>();
        InitMovement();

		player.PlayerStateChanged += OnPlayerStateChanged;
	}

    private void Update() 
    {
        distanceTravelled += currentSpeed * Time.deltaTime;

        switch (player.CurrentPlayerState)
        {
            case PlayerStates.Run:
                OnRoadMovement();
                break;
            case PlayerStates.Skate:
                OnRoadMovement();
                break;
            case PlayerStates.Fly:
                FlyMovement();
                break;
        }
    }

	#endregion

	#region Methods

    private void InitMovement()
    {
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
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

        referanceObject.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        referanceObject.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled + 1));

        Transform tempTransform;
        (tempTransform = transform).position = Vector3.Lerp
        (
            transform.position,
            pathCreator.path.GetPointAtDistance(distanceTravelled) + referanceObject.right * (sideMove * runSideMoveSpeed) + Vector3.up * playerHeight,
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
	
	private void SetMovementSkate()
    {
        currentSpeed = skateSpeed;
        playerHeight = .25f;
    }

	private void SetMovementFly()
    {
        transform.LeanRotateX(12, .2f).setEaseInCubic();
        transform.LeanMoveY(transform.position.y + 1, .2f).setEaseInCubic();
    }

	public void BoostSpeed(float boost)
    {
        var tempSpeed = currentSpeed;
        currentSpeed *= boost;
        LeanTween.delayedCall(2, ()=> 
        {
            if(Math.Abs(currentSpeed*boost - tempSpeed) < 0.01f) 
                currentSpeed = tempSpeed;
        });
    }

	#endregion

	#region Callbacks

    private void OnPlayerStateChanged(PlayerStates currentState, PlayerStates newState)
    {
        switch (newState)
        {
            case PlayerStates.WaitStart:
                if(currentState != PlayerStates.End) InitMovement();
                break;
            case PlayerStates.Run:
                SetMovementRun();
                break;
            case PlayerStates.Skate:
                SetMovementSkate();
                break;
            case PlayerStates.Fly:
                SetMovementFly();
                break;
            case PlayerStates.End:
                break;
        }
    }

	#endregion
}