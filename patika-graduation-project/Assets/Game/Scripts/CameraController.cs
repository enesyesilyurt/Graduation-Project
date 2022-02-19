using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Core;
using Cinemachine;

public class CameraController : MonoSingleton<CameraController>
{
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float smoothSpeed;

    [SerializeField]
    private PathCreator pathCreator;

    [SerializeField]
    private CinemachineVirtualCamera runCamera;
    
    [SerializeField]
    private CinemachineVirtualCamera flyCamera;

    private List<CinemachineVirtualCamera> cameras;

    private float speed = 9;

    private float distanceTravelled;


    private void Start() {
        Player.Instance.PlayerFlew += OnPlayerFlew;
        Player.Instance.PlayerRan += OnPlayerRan;

        cameras = new List<CinemachineVirtualCamera>();

        cameras.Add(runCamera);
        cameras.Add(flyCamera);

        OnPlayerRan();

    }

    private void Update() 
    {
        distanceTravelled = Player.Instance.DistanceTravelled;

        RunFollow();
        FlyFollow();
    }

    private void RunFollow()
    {
        Vector3 desiredPosition = pathCreator.path.GetPointAtDistance(distanceTravelled-15) + Vector3.up * 9;
        Vector3 smoothedPosition = Vector3.Lerp(runCamera.transform.position, desiredPosition, smoothSpeed);
        runCamera.transform.position = smoothedPosition;

        runCamera.transform.LookAt(Player.Instance.transform);
    }

    private void FlyFollow()
    {
        Vector3 desiredPosition = Player.Instance.transform.position + Player.Instance.transform.forward * -18 + Player.Instance.transform.up * 7;
        Vector3 smoothedPosition = Vector3.Lerp(flyCamera.transform.position, desiredPosition, smoothSpeed);
        flyCamera.transform.position = smoothedPosition;

        flyCamera.transform.LookAt(Player.Instance.transform);
    }

    private void CloseAllCameras()
    {
        foreach (var camera in cameras)
        {
            camera.gameObject.SetActive(false);
        }
    }

    private void OnPlayerFlew()
    {
        CloseAllCameras();
        flyCamera.gameObject.SetActive(true);
    }

    private void OnPlayerRan()
    {
        CloseAllCameras();
        runCamera.gameObject.SetActive(true);
    }

    
}
