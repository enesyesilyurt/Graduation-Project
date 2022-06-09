using Shadout.Controllers;
using UnityEngine;

public class ExitPathState : IState
{
    private Agent agent;
    private AgentMovementController _agentMovementController;
    private float sideMove;
    private Transform referanceObject;
    private Transform followerObject;
    private float runSideMoveSpeed;

    public ExitPathState(Agent agent, AgentMovementController agentMovementController)
    {
        this.agent = agent;
        this._agentMovementController = agentMovementController;
        this.referanceObject = agentMovementController.ReferenceObject;
        this.followerObject = agentMovementController.FollowerObject;
        this.runSideMoveSpeed = agentMovementController.RunSideMoveSpeed;
    }

    public void OnEnter()
    {
        agent.target = PathManager.Instance.PathCreator.path.GetPointAtDistance
        (
            agent.DistanceTravelled + agent.TargetFront
        );
        
        if (Vector3.Distance(agent.transform.right + agent.transform.position, agent.target) > Vector3.Distance(-agent.transform.right + agent.transform.position, agent.target))
        {
            _agentMovementController.SideInput = -0.1f;
        }
        else
        {
            _agentMovementController.SideInput = 0.1f;
        }
    }

    public void OnExit()
    {
        _agentMovementController.SideInput = 0;
    }

    public void Tick()
    {
        sideMove += _agentMovementController.SideInput * Time.deltaTime;

        referanceObject.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled);
        referanceObject.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled + 1));

        Transform tempTransform;
        (tempTransform = agent.transform).position = Vector3.Lerp
        (
            agent.transform.position,
            PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled) + referanceObject.right * (sideMove * runSideMoveSpeed) + referanceObject.right * _agentMovementController.SidePosition + Vector3.up * _agentMovementController.playerHeight,
            .2f
        );

        followerObject.position = Vector3.Lerp(followerObject.position, tempTransform.position, .2f);
        followerObject.LookAt(agent.transform);
        agent.transform.rotation = followerObject.rotation;
    }
}
