using Shadout.Controllers;
using UnityEngine;

public class RunState : IState
{
    private Agent agent;
    private AgentMovement agentMovement;
    private Rigidbody rb;
    private float sideMove;
    private Transform referanceObject;
    private Transform followerObject;
    private float runSideMoveSpeed;

    public RunState(Agent agent, AgentMovement agentMovement, 
        Rigidbody rb)
    {
        this.agent = agent;
        this.agentMovement = agentMovement;
        this.rb = rb;
        this.referanceObject = agentMovement.ReferanceObject;
        this.followerObject = agentMovement.FollowerObject;
        this.runSideMoveSpeed = agentMovement.RunSideMoveSpeed;
    }
    public void OnEnter()
    {
        agentMovement.playerHeight = 0;

        rb.velocity = Vector3.zero;

        var tempTransform = agent.transform;
        var rotation = tempTransform.rotation;
        rotation = new Quaternion(0, rotation.y, rotation.z, rotation.w);
        tempTransform.rotation = rotation;
        agent.transform.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled);
        agent.transform.LookAt(PathManager.Instance.PathCreator.path.GetPointAtDistance(agent.DistanceTravelled + 1));

        sideMove = 0;

        Debug.Log("on run");
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        //sideMove += agentMovement.SideInput * Time.deltaTime;

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
