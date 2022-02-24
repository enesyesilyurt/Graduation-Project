using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "My Objects", menuName = "Vehicle")]
public class PlayerVehicleScriptableObject : ScriptableObject
{
    [SerializeField]
    private GameObject vehicle;
    [SerializeField]
    private Vector3 playerRotation;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private string animationName;


    public GameObject Vehicle => vehicle;
    public Vector3 PlayerRotation => playerRotation;
    public float Speed => speed;
    public float HorizontalSpeed => horizontalSpeed;
    public string PlayerAnimationName => animationName;

}
