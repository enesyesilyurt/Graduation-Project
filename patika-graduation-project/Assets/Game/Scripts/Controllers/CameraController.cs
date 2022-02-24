using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Helpers;
using Cinemachine;

public class CameraController : MonoSingleton<CameraController>
{
    #region SerializeFields
    
    [Header("Fly Camera")]

    [SerializeField]
    private float flyOffsetZ;

    [SerializeField]
    private float flyOffsetY;
    
    [SerializeField]
    private CinemachineVirtualCamera flyCamera;

    [Header("Run Camera")]

    [SerializeField]
    private float runOffsetZ;

    [SerializeField]
    private float runOffsetY;

    [SerializeField]
    private CinemachineVirtualCamera runCamera;

    [Header("Objects")]
    
    [SerializeField]
    private PathCreator pathCreator;

    #endregion
    
    #region Variables

    private float distanceTravelled;

    private float smoothSpeed = .125f;

    private List<CinemachineVirtualCamera> cameras;

    #endregion

    #region Unity Methods

    private void Start() {
        Player.Instance.PlayerStartedFly += OnPlayerStartedFly;
        Player.Instance.PlayerStartedRun += OnPlayerStartedRun;

        cameras = new List<CinemachineVirtualCamera>();

        cameras.Add(runCamera);
        cameras.Add(flyCamera);

        OnPlayerStartedRun();
    }

    private void Update() 
    {
        distanceTravelled = Player.Instance.DistanceTravelled;

        RunFollow();
        FlyFollow();
    }

    #endregion

    #region Methods

    private void RunFollow()
    {
        Vector3 desiredPosition = pathCreator.path.GetPointAtDistance(distanceTravelled-runOffsetZ) + Vector3.up * runOffsetY;
        Vector3 smoothedPosition = Vector3.Lerp(runCamera.transform.position, desiredPosition, smoothSpeed);
        runCamera.transform.position = smoothedPosition;
        runCamera.transform.LookAt(Player.Instance.transform);
    }

    private void FlyFollow()
    {
        Vector3 desiredPosition = Player.Instance.transform.position + Player.Instance.transform.forward * -flyOffsetZ + Player.Instance.transform.up * flyOffsetY;
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

    #endregion

    #region Callbacks

    private void OnPlayerStartedFly()
    {
        CloseAllCameras();
        flyCamera.gameObject.SetActive(true);
    }

    private void OnPlayerStartedRun()
    {
        CloseAllCameras();
        runCamera.gameObject.SetActive(true);
    }

    #endregion
}
