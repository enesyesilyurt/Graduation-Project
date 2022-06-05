using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Cinemachine;
using NoName.Utilities;

public class CameraManager : Singleton<CameraManager>
{
    #region SerializeFields

    [SerializeField] private PlayerManager player;

    [Header("Start Camera")]

    [SerializeField] private CinemachineVirtualCamera startCamera;

    [Header("Fly Camera")]

    [SerializeField] private CinemachineVirtualCamera flyCamera;
    [SerializeField] private float flyOffsetZ;
    [SerializeField] private float flyOffsetY;

    [Header("Run Camera")]

    [SerializeField] private CinemachineVirtualCamera runCamera;
    [SerializeField] private float runOffsetZ;
    [SerializeField] private float runOffsetY;

    #endregion

    #region Variables

    private PathCreator pathCreator;
    private float distanceTravelled;

    private float smoothSpeed = .125f;

    private List<CinemachineVirtualCamera> cameras;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        pathCreator = PathManager.Instance.PathCreator;
        cameras = new List<CinemachineVirtualCamera>
        {
            startCamera,
            runCamera,
            flyCamera
        };

        player.PlayerStateChanged += OnPlayerStateChanged;
    }

    private void InitStartCam()
    {
        CloseAllCameras();
        startCamera.gameObject.SetActive(true);

        var position = player.transform.position;
        startCamera.transform.position = new Vector3
        (
            position.x + 15,
            position.y + 10,
            position.z + 23
        );
    }
    
    private void LateUpdate()
    {
        distanceTravelled = player.DistanceTravelled;

        RunFollow();
        //FlyFollow();
    }

    #endregion

    #region Methods

    private void RunFollow()
    {
        var direction = pathCreator.path.GetPointAtDistance(distanceTravelled) - pathCreator.path.GetPointAtDistance(distanceTravelled + 1);

        Vector3 desiredPosition = player.transform.position + direction * runOffsetZ + Vector3.up * runOffsetY;
        Vector3 smoothedPosition = Vector3.Lerp(runCamera.transform.position, desiredPosition, smoothSpeed);
        runCamera.transform.position = smoothedPosition;
        runCamera.transform.LookAt(player.transform);
    }

    private void FlyFollow()
    {
        var playerTransform = player.transform;
        Vector3 desiredPosition = playerTransform.position + playerTransform.forward * -flyOffsetZ + playerTransform.up * flyOffsetY;
        Vector3 smoothedPosition = Vector3.Lerp(flyCamera.transform.position, desiredPosition, smoothSpeed);
        flyCamera.transform.position = smoothedPosition;
        flyCamera.transform.LookAt(player.transform);
    }

    private void CloseAllCameras()
    {
        foreach (var virtualCamera in cameras)
        {
            virtualCamera.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Callbacks

    private void OnPlayerStateChanged(PlayerStates currentState, PlayerStates newState)
    {
        switch (newState)
        {
            case PlayerStates.WaitStart:
                InitStartCam();
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
