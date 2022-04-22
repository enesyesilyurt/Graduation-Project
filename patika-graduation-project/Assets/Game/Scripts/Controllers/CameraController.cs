using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Helpers;
using Cinemachine;

public class CameraController : MonoSingleton<CameraController>
{
    #region SerializeFields

    [Header("Start Camera")]
    [SerializeField]
    private CinemachineVirtualCamera startCamera;
    
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
        Player.Instance.PlayerStateChanged += OnPlayerStateChanged;

        cameras = new List<CinemachineVirtualCamera>();

        cameras.Add(startCamera);
        cameras.Add(runCamera);
        cameras.Add(flyCamera);
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
        var direction = pathCreator.path.GetPointAtDistance(distanceTravelled) - pathCreator.path.GetPointAtDistance(distanceTravelled + 1);
        
        Vector3 desiredPosition = Player.Instance.transform.position + direction * runOffsetZ + Vector3.up * runOffsetY;
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

    private void OnPlayerStateChanged(PlayerStates newState)
    {
        switch(newState)
        {
            case PlayerStates.WaitStart:
                CloseAllCameras();
                startCamera.gameObject.SetActive(true);
                break;
            case PlayerStates.Fly:
                CloseAllCameras();
                flyCamera.gameObject.SetActive(true);
                break;

            case PlayerStates.Run:
                CloseAllCameras();
                runCamera.gameObject.SetActive(true);
                break;
        }
    }

    #endregion
}
