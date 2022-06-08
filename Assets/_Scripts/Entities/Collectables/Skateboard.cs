using Shadout.Controllers;
using UnityEngine;

public class Skateboard : Collectable
{
    [SerializeField]
    private float speed;

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
    }
    
    protected override void Collect(ContenderBase contender)
    {
        base.Collect(contender);
        contender.UpdateContenderState(ContenderState.Skate);
    }
}
