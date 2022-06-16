using System.Collections;
using System.Collections.Generic;
using Shadout.Controllers;
using UnityEngine;

public class WaitStartState : IState
{
    private float distanceTravelled;
    private Transform agent;

    public WaitStartState(Transform agent, float distanceTravelled)
    {
        this.agent = agent;
        this.distanceTravelled = distanceTravelled;
    }

    public void OnEnter()
    {
        // agent.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled);
        // agent.position += agent.GetComponent<AgentMovementController>().ReferenceObject.right * agent.GetComponent<AgentMovementController>().SidePosition;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}
