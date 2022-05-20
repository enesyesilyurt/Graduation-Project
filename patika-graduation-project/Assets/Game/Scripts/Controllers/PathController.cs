using Helpers;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class PathController : MonoSingleton<PathController>
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private RoadMeshCreator roadMeshCreator;

    public PathCreator PathCreator => pathCreator;
    public RoadMeshCreator RoadMeshCreator => roadMeshCreator;

}
