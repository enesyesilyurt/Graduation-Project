using Shadout.Controllers;
using UnityEngine;

public class RunState : IState
{
    private Agent agent;
    private AgentMovementController agentMovementController;
    private Rigidbody rb;
    private float sideMove;
    private Transform referanceObject;
    private Transform followerObject;
    private float runSideMoveSpeed;

    public RunState(Agent agent, AgentMovementController agentMovementController,
        Rigidbody rb)
    {
        this.agent = agent;
        this.rb = rb;
        this.agentMovementController = agentMovementController;
        referanceObject = agentMovementController.ReferenceObject;
        followerObject = agentMovementController.FollowerObject;
        runSideMoveSpeed = agentMovementController.RunSideMoveSpeed;
    }
    public void OnEnter()
    {
        agentMovementController.playerHeight = 0;

        rb.velocity = Vector3.zero;

        var tempTransform = agent.transform;
        var rotation = tempTransform.rotation;
        tempTransform.rotation = rotation;
        agent.transform.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(PathManager.Instance.PathCreator.path.GetClosestDistanceAlongPath(agent.transform.position)
            ) + agentMovementController.SidePosition * Vector3.right;
        agent.transform.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled + 1));
        rotation = new Quaternion(0, rotation.y, 0, rotation.w);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        referanceObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled);
        referanceObject.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled + 1));

        agent.transform.position = Vector3.Lerp
        (
            agent.transform.position,
            PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled) + referanceObject.right * agentMovementController.SidePosition  + Vector3.up * agentMovementController.playerHeight,
            .2f
        );

        followerObject.position = Vector3.Lerp(followerObject.position, agent.transform.position, .2f);
        followerObject.LookAt(agent.transform);
        agent.transform.rotation = followerObject.rotation;
    }
}
