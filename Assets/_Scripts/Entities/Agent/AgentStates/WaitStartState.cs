using System.Collections;
using System.Collections.Generic;
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
        agent.position = PathManager.Instance.PathCreator.path.GetPointAtDistance(distanceTravelled);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}
