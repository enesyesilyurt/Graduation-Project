using NoName.Utilities;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private RoadMeshCreator roadMeshCreator;

    public PathCreator PathCreator => pathCreator;
    public RoadMeshCreator RoadMeshCreator => roadMeshCreator;

}
