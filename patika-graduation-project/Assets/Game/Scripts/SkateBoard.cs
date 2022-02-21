using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateBoard : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private void Update() {
        transform.RotateAround(transform.position ,Vector3.up, speed * Time.deltaTime);
    }
}
