using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Cinemachine;
using Shadout.Utilities;
using System;

public class CameraManager : Singleton<CameraManager>
{
    #region SerializeFields

    [SerializeField] private Player player;

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

    [Header("End Camera")]

    [SerializeField] private CinemachineVirtualCamera endCamera;

    #endregion

    #region Variables

    private float distanceTravelled;

    private float smoothSpeed = .125f;

    private List<CinemachineVirtualCamera> cameras;

    #endregion

    #region Unity Methods

    public void InitCameraManager()
    {
        cameras = new List<CinemachineVirtualCamera>
        {
            startCamera,
            runCamera,
            flyCamera,
            endCamera
        };

        player.ContenderStateChanged += OnContenderStateChanged;
    }

    private void OpenStartCam()
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

    private void OpenEndCam()
    {
        CloseAllCameras();
        endCamera.gameObject.SetActive(true);
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
        var direction = PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled) 
            - PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled + 1);

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
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

    #endregion

    #region Callbacks

    private void OnContenderStateChanged(ContenderState currentState, ContenderState newState)
    {
        switch (newState)
        {
            case ContenderState.WaitStart:
                OpenStartCam();
                break;
            case ContenderState.Fly:
                CloseAllCameras();
                flyCamera.gameObject.SetActive(true);
                break;
            case ContenderState.Run:
                CloseAllCameras();
                runCamera.gameObject.SetActive(true);
                break;
            case ContenderState.End:
                OpenEndCam();
                break;
        }
    }

    #endregion
}
