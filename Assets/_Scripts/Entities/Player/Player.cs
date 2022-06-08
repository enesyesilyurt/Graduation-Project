using Shadout.Controllers;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementManager))]

public class Player : ContenderBase
{
    private void Update()
    {
        var closestDistance = PathManager.Instance.PathCreator.path.GetClosestPointOnPath(transform.position);
			
        if (Vector3.Distance(transform.position, closestDistance) > PathManager.Instance.RoadMeshCreator.roadWidth)
        {
            if (currentContenderState == ContenderState.Run || currentContenderState == ContenderState.Skate)
            {
                UpdateContenderState(ContenderState.Fly);
            }
        }
        else if(transform.position.y >= closestDistance.y - .5f)
        {
            if (currentContenderState == ContenderState.Fly)
            {
                UpdateContenderState(ContenderState.Run);
            }
        }
    }
}
