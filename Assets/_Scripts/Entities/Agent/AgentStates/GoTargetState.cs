using System.Collections;
using System.Collections.Generic;
using Shadout.Controllers;
using UnityEngine;

public class GoTargetState : IState
{
    private Agent agent;
    private Vector3 target;
    private Rigidbody rb;
    private float flySpeed;

    public GoTargetState(Agent agent, Rigidbody rb, float flySpeed)
    {
        this.agent = agent;
        this.target = agent.target;
        this.flySpeed = flySpeed;
        this.rb = rb;
    }

    public void OnEnter()
    {
        agent.transform.LookAt(target);
        agent.transform.LeanMoveY(agent.transform.position.y + 1, .2f).setEaseInCubic();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        rb.velocity = agent.transform.forward * (flySpeed * Time.deltaTime);
    }
}
