using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRunAwayState : AIBaseState
{
    private float runAwayDistance = 10f;
    private NavMeshAgent agent;
    private Transform playerTransform;

    public override void EnterState(AIHandler handler)
    {
        Debug.Log("AI: Running away from player! D:");

        agent = handler.GetComponent<NavMeshAgent>();
        playerTransform = handler.GetPlayerObject().transform;

        RunAway();
    }

    public override void UpdateState(AIHandler handler)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            RunAway();
        }
    }

    public override void ExitState(AIHandler handler)
    {

    }

    private void RunAway()
    {
        Vector3 runDirection = (agent.transform.position - playerTransform.position).normalized;
        Vector3 runTarget = agent.transform.position + runDirection * runAwayDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(runTarget, out hit, runAwayDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("AI: No valid escape path found!");
        }
    }
}
