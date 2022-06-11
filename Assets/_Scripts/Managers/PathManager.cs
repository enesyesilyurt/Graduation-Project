using Shadout.Utilities;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using Shadout.Models;
using System;
using Shadout.Controllers;
using UnityEditor;

public class PathManager : Singleton<PathManager>
{
    [SerializeField]
    private FinalRoad finalRoad;
    
    private PathCreator pathCreator;
    private RoadMeshCreator roadMeshCreator;

    public PathCreator PathCreator => pathCreator;
    public RoadMeshCreator RoadMeshCreator => roadMeshCreator;

    public void InitPathManager()
    {
        LevelManager.Instance.levelCompleted += OnlevelCompleted;
        pathCreator = LevelManager.Instance.Level.PathCreator;
        roadMeshCreator = LevelManager.Instance.Level.RoadMeshCreator;

        ContenderManager.Instance.Init();
        finalRoad.InitFinalRoad();
        CameraManager.Instance.InitCameraManager();
    }

    private void OnlevelCompleted()
    {
        pathCreator = LevelManager.Instance.Level.PathCreator;
        roadMeshCreator = LevelManager.Instance.Level.RoadMeshCreator;
    }
}
