using System;
using Helpers;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    #region SerializeFields

    [Header("Run")]

    [SerializeField]
    private float runSpeed = 5;

    [SerializeField]
    private float runSideMoveSpeed = 5;

    [Header("Fly")]

    [SerializeField]
    private float flySpeed;

    [SerializeField]
    private float flySideMoveSpeed;

    [Header("Skate")]
    [SerializeField]
    private float skateSpeed;

    [Header("Components")]

    [SerializeField]
    private Animator animator;

    [Header("Objects")]

    [SerializeField]
    private PathCreator pathCreator;

    [SerializeField]
    private GameObject wing;

    [SerializeField]
    private GameObject skateBoard;

    #endregion

    #region Properties

    public float DistanceTravelled => distanceTravelled;

    #endregion

    #region Variables

    private float playerHeight;

    private float currentSpeed;

    private Transform followerObject;

    private Transform referanceObject;

    private PlayerStates playerState;

    private float distanceTravelled;

    private float sideMove;

    private bool isOnRoad = true;

    private Rigidbody rb;

    private RoadMeshCreator roadMesh;

    #endregion

    #region Acitons

    public event Action<PlayerStates> PlayerStateChanged;

    #endregion

    #region Unity Methods
    private void Start()
    {
        InitMovement();
        rb = GetComponent<Rigidbody>();
        roadMesh = pathCreator.GetComponent<RoadMeshCreator>();
        UpdatePlayerState(PlayerStates.WaitStart);
        LeanTween.delayedCall(2, ()=> UpdatePlayerState(PlayerStates.Run));
    }

    private void WaitStart()
    {
        animator.SetTrigger("Idle");
    }

    private void LateUpdate()
    {
        distanceTravelled += currentSpeed * Time.deltaTime;

        var closestDistance = pathCreator.path.GetClosestPointOnPath(transform.position);
        if (Vector3.Distance(transform.position, closestDistance) > roadMesh.roadWidth)
        {
            if(isOnRoad)
            {
                isOnRoad = false;
                UpdatePlayerState(PlayerStates.Fly);
            }
            else
                FlyMovement();
        }
        else if(transform.position.y >= closestDistance.y - .5f)
        {
            if (!isOnRoad)
            {
                isOnRoad = true;
                UpdatePlayerState(PlayerStates.Run);
            }
            else
                RunMovement();
        }
        else
            FlyMovement();
    }

    #endregion

    #region Methods

    private void InitMovement()
    {
        referanceObject = Instantiate(new GameObject(), pathCreator.path.GetClosestPointOnPath(transform.position), Quaternion.identity).transform;
        followerObject = Instantiate(new GameObject(), transform.position, Quaternion.identity).transform;
    }

    public void BoostSpeed(float boost)
    {
        var tempSpeed = currentSpeed;
        currentSpeed *= boost;
        LeanTween.delayedCall(2, ()=> currentSpeed = tempSpeed);
    }

    private void Skate()
    {
        skateBoard.transform.localScale = Vector3.one * 0.01f;
        skateBoard.LeanScale(Vector3.one, .7f).setEaseOutCubic();
        animator.SetTrigger("Skate");
        skateBoard.SetActive(true);
        currentSpeed = skateSpeed;

        playerHeight = .25f;
    }

    private void FlyMovement()
    {
        float sideInput = 0;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            sideInput = touch.deltaPosition.x;
        }

        transform.Rotate(Vector3.up * sideInput * Time.deltaTime * flySideMoveSpeed);
        transform.eulerAngles = new Vector3(12, transform.eulerAngles.y, 0);

        rb.velocity = transform.forward * flySpeed * Time.deltaTime;
    }

    private void StartFly()
    {
        //skateBoard.LeanMoveY(skateBoard.transform.position.y - 6, 1.4f).setOnComplete(() => skateBoard.SetActive(false));
        skateBoard.SetActive(false);

        transform.LeanRotateX(12, .2f).setEaseInCubic();
        animator.SetTrigger("Fly");

        transform.LeanMoveY(transform.position.y + 1, .2f).setEaseInCubic();
        wing.SetActive(true);
        wing.transform.localScale = Vector3.one * 0.01f;
        wing.transform.LeanScale(Vector3.one, .2f).setEaseInOutSine();
    }

    private void GetDownOnRoad()
    {
        wing.transform.LeanScale(Vector3.one * 0.01f, .2f).setEaseInOutSine().setOnComplete(() => 
        { 
            wing.SetActive(false); 
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        });

        //skateBoard.LeanMoveY(skateBoard.transform.position.y - 6, 1.4f).setOnComplete(() => skateBoard.SetActive(false));

        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        playerHeight = 0;
        currentSpeed =  runSpeed;

        rb.velocity = Vector3.zero;

        followerObject.position = pathCreator.path.GetPointAtDistance(distanceTravelled - .2f);

        transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled + 1));

        animator.SetTrigger("Run");
        sideMove = 0;
    }

    private void RunMovement()
    {
        float sideInput = 0;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            sideInput = touch.deltaPosition.x;
        }
        sideMove += sideInput * Time.deltaTime;

        referanceObject.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        referanceObject.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled + 1));

        transform.position = Vector3.Lerp
        (
            transform.position,
            pathCreator.path.GetPointAtDistance(distanceTravelled) + referanceObject.right * sideMove * runSideMoveSpeed + Vector3.up * playerHeight,
            .2f
        );

        followerObject.position = Vector3.Lerp(followerObject.position, transform.position, .2f);
        followerObject.LookAt(transform);
        transform.rotation = followerObject.rotation;
    }

    public void UpdatePlayerState(PlayerStates newState)
    {
        playerState = newState;

        switch (newState)
        {
            case PlayerStates.WaitStart:
                WaitStart();
                break;
            case PlayerStates.Run:
                GetDownOnRoad();
                break;
            case PlayerStates.Skate:
                Skate();
                break;
            case PlayerStates.Fly:
                StartFly();
                break;
            case PlayerStates.End: break;
        }

        PlayerStateChanged?.Invoke(newState);
    }

    #endregion
}
