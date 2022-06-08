using Shadout.Controllers;
using UnityEngine;

public class ExitPathState : IState
{
    private float direction;
    private Agent agent;
    private AgentMovement agentMovement;
    private float sideMove;
    private Transform referanceObject;
    private Transform followerObject;
    private float runSideMoveSpeed;

    public ExitPathState(Agent agent, AgentMovement agentMovement, float direction)
    {
        this.direction = direction;
        this.agent = agent;
        this.agentMovement = agentMovement;
        this.referanceObject = agentMovement.ReferanceObject;
        this.followerObject = agentMovement.FollowerObject;
        this.runSideMoveSpeed = agentMovement.RunSideMoveSpeed;
    }

    public void OnEnter()
    {
        agentMovement.SideInput = 0.1f;
    }

    public void OnExit()
    {
        agentMovement.SideInput = 0;
    }

    public void Tick()
    {
        sideMove += agentMovement.SideInput * Time.deltaTime;

        referanceObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled);
        referanceObject.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled + 1));

        Transform tempTransform;
        (tempTransform = agent.transform).position = Vector3.Lerp
        (
            agent.transform.position,
            PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled) + referanceObject.right * (sideMove * runSideMoveSpeed) + Vector3.up * agentMovement.playerHeight,
            .2f
        );

        followerObject.position = Vector3.Lerp(followerObject.position, tempTransform.position, .2f);
        followerObject.LookAt(agent.transform);
        agent.transform.rotation = followerObject.rotation;
    }
}
