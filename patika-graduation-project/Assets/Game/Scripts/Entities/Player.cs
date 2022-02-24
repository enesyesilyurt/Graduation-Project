using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private Transform lookDirection;

    [SerializeField]
    private Transform lookObject;

    [SerializeField]
    private Transform follower;

    #endregion

    #region Properties

    public float DistanceTravelled => distanceTravelled;

    #endregion

    #region Variables

    private float distanceTravelled;

    private float sideMove;

    private bool startFly = false;

    private bool startRun = true;

    private bool isFly = false;

    private bool isSkate = false;

    private Rigidbody rb;

    private RoadMeshCreator roadMesh;

    #endregion

    #region Acitons

    public event Action PlayerStartedFly;
    public event Action PlayerStartedRun;

    #endregion

    #region Unity Methods
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        roadMesh = pathCreator.GetComponent<RoadMeshCreator>();
    }

    private void LateUpdate()
    {
        distanceTravelled += runSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, pathCreator.path.GetClosestPointOnPath(transform.position)) > roadMesh.roadWidth)
        {
            Fly();
        }
        else
        {
            OnRoad();
        }
    }

    /*private void OnTriggerEnter(Collider other) {
        var skateBoardCollectable = other.GetComponent<SkateBoard>();
        if(skateBoardCollectable)
        {
            other.gameObject.SetActive(false);
            Skate();
        }
    }*/

    #endregion

    #region Methods

    private void Skate()//skate gameobject,animation skate, speed float, playerHeight float
    {
        skateBoard.transform.localScale = Vector3.one * 0.01f;
        skateBoard.LeanScale(Vector3.one, .7f).setEaseOutCubic();
        transform.LeanMoveY(.7f,.2f);
        isSkate = true;
        animator.SetTrigger("Skate");
        skateBoard.SetActive(true);
        runSpeed = skateSpeed;
    }

    private void Fly()//playerRotation, playerHeight, wing gameobject,animation name, playerSpeed float, playerSideMoveSpeed float,
    {
        if (!isFly)
        {
            isFly = true;
            
            isSkate = false;
            skateBoard.LeanMoveY(skateBoard.transform.position.y-6,1.4f).setOnComplete( ()=> skateBoard.SetActive(false));
            
            transform.LeanRotateX(12, .2f).setEaseInCubic().setOnComplete(() => startFly = true);
            transform.LeanMoveY(transform.position.y + 1, .2f).setEaseInCubic();
            wing.SetActive(true);
            wing.transform.localScale = Vector3.one * 0.01f;
            wing.transform.LeanScale(Vector3.one, .2f).setEaseInOutSine();
        }
        else if (startFly)
        {
            startFly = false;
            animator.SetTrigger("Fly");
            PlayerStartedFly?.Invoke();
        }
        else if (isFly && !startFly)
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
    }

    private void OnRoad()//animation name,
    {
        if (isFly)
        {
            wing.transform.LeanScale(Vector3.one * 0.01f, .2f).setEaseInOutSine().setOnComplete(() => { wing.SetActive(false); startRun = true; distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position); });
            skateBoard.LeanMoveY(skateBoard.transform.position.y-6,1.4f).setOnComplete( ()=> skateBoard.SetActive(false));

            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);

            rb.velocity = Vector3.zero;
            lookDirection.position = pathCreator.path.GetPointAtDistance(distanceTravelled + 1);

            transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.LookAt(lookDirection);

            isFly = false;
            startRun = false;

            animator.SetTrigger("Run");
            sideMove = 0;
            PlayerStartedRun?.Invoke();
        }
        else if (startRun)
        {
            float sideInput = 0;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                sideInput = touch.deltaPosition.x;
            }
            sideMove += sideInput * Time.deltaTime;

            lookObject.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            lookDirection.position = pathCreator.path.GetPointAtDistance(distanceTravelled + 1);
            lookObject.LookAt(lookDirection);

            if (isSkate)
            {
                transform.position = Vector3.Lerp
                (
                    transform.position,
                    pathCreator.path.GetPointAtDistance(distanceTravelled) + lookObject.right * sideMove * runSideMoveSpeed + Vector3.up * .3f,
                    .2f
                );
            }
            else
            {
                transform.position = Vector3.Lerp
                (
                    transform.position,
                    pathCreator.path.GetPointAtDistance(distanceTravelled) + lookObject.right * sideMove * runSideMoveSpeed,
                    .2f
                );
            }
            

            follower.position = Vector3.Lerp(follower.position, transform.position, .2f);
            follower.LookAt(transform);
            transform.rotation = follower.rotation;
        }
    }

    #endregion
}
