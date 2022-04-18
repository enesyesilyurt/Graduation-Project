using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skateboard : Collectable
{
    
    [SerializeField]
    private float speed;

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
    }
    protected override void Collect()
    {
        base.Collect();
        Player.Instance.UpdatePlayerState(PlayerStates.Skate);
    }
}
