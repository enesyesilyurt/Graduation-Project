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
        rotation = new Quaternion(0, rotation.y, rotation.z, rotation.w);
        tempTransform.rotation = rotation;
        agent.transform.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled);
        agent.transform.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled + 1));
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        referanceObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled);
        referanceObject.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled + 1));

        Transform tempTransform;
        (tempTransform = agent.transform).position = Vector3.Lerp
        (
            agent.transform.position,
            PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled) + referanceObject.right * agentMovementController.SidePosition  + Vector3.up * agentMovementController.playerHeight,
            .2f
        );

        followerObject.position = Vector3.Lerp(followerObject.position, tempTransform.position, .2f);
        followerObject.LookAt(agent.transform);
        agent.transform.rotation = followerObject.rotation;
    }
}
