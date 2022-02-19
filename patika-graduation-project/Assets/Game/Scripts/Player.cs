using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using PathCreation;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    [SerializeField]
    private GameObject wing;
    [SerializeField]
    private PathCreator pathCreator;

    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private float sideMoveSpeed = 5;

    [SerializeField]
    private Transform lookDirection;

    [SerializeField]
    private Transform lookObject;

    [SerializeField]
    private Transform hips;

    [SerializeField]
    private Transform follower;

    [SerializeField]
    private Transform mesh;

    [SerializeField]
    private float limit;

    private float distanceTravelled;

    public float DistanceTravelled => distanceTravelled;

    [SerializeField]
    private Transform temp;

    [SerializeField]
    private float flySpeed;

    private float verticleMove;
    private float sideMove;

    private bool startFly = false;
    private bool startRun = true;

    [SerializeField]
    private Animator animator;

    public event Action PlayerFlew;
    public event Action PlayerRan;

    private Rigidbody rb;


    bool isFly = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        distanceTravelled += speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, pathCreator.path.GetClosestPointOnPath(transform.position)) > limit)
        {
            Fly();
        }
        else
        {
            Move();
        }


    }

    private void Fly()
    {
        if (!isFly)
        {
            transform.LeanRotateX(12, .2f).setEaseInCubic().setOnComplete(() => startFly = true);
            transform.LeanMoveY(transform.position.y + 1, .2f).setEaseInCubic();
            wing.SetActive(true);
            wing.transform.localScale = Vector3.one * 0.01f;
            wing.transform.LeanScale(Vector3.one, .2f).setEaseInOutSine();
            isFly = true;
        }
        else if (startFly)
        {
            

            PlayerFlew?.Invoke();

            startFly = false;

            animator.SetTrigger("Fly");
        }
        else if (isFly && !startFly)
        {
            float sideInput = 0;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                sideInput = touch.deltaPosition.x;
            }

            transform.Rotate(Vector3.up * sideInput * Time.deltaTime * sideMoveSpeed);

            transform.eulerAngles = new Vector3(12, transform.eulerAngles.y, 0);

            rb.velocity = transform.forward * flySpeed * Time.deltaTime;
        }


    }

    private void Move()
    {
        if (isFly)
        {
            wing.transform.LeanScale(Vector3.one * 0.01f, .2f).setEaseInOutSine().setOnComplete(() => { wing.SetActive(false); startRun = true;distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position); });

            PlayerRan?.Invoke();
            transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            isFly = false;

            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);

            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            lookDirection.position = pathCreator.path.GetPointAtDistance(distanceTravelled + 1);
            transform.LookAt(lookDirection);

            animator.SetTrigger("Run");
            startRun=false;
            rb.velocity = Vector3.zero;
            sideMove = 0;
        }
        else if (startRun)
        {
            float sideInput = 0;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                sideInput = touch.deltaPosition.x;
            }
            //transform.Rotate(Vector3.up * sideInput * Time.deltaTime * sideMoveSpeed);

            //transform.eulerAngles = new Vector3(12, transform.eulerAngles.y, 0);

        

            sideMove += sideInput * Time.deltaTime;

            lookObject.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            lookDirection.position = pathCreator.path.GetPointAtDistance(distanceTravelled + 1);

            lookObject.LookAt(lookDirection);

            temp.rotation = lookObject.rotation;

            transform.position = Vector3.Lerp
            (
                transform.position,
                pathCreator.path.GetPointAtDistance(distanceTravelled) + temp.right * sideMove * sideMoveSpeed,
                .2f
            );

            follower.position = Vector3.Lerp(follower.position, transform.position, .2f);
            follower.LookAt(mesh);
            transform.rotation = follower.rotation;
        }
    }
}
