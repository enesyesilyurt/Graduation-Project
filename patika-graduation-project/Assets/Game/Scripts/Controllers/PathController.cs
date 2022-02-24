using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    private PathCreator pathCreator;

    public PathCreator PathCreator => pathCreator;
}
