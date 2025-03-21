using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasingState : AIBaseState
{
    NavMeshAgent agent;

    bool isPatrolingArea = false;

    public override void EnterState(AIHandler handler)
    {
        isPatrolingArea = false;
        agent = handler.GetComponent<NavMeshAgent>();
        if (handler.enemyType == EnemyType.Melee)
            agent.stoppingDistance = handler.meleeEnemyDistance;
        else if (handler.enemyType == EnemyType.Ranged)
            agent.stoppingDistance = handler.rangedEnemyDistance;
    }

    public override void UpdateState(AIHandler handler)
    {
        if (isPatrolingArea)
            return;

        bool isInRange = handler.IsInAttackRange();
        bool isInVision = handler.IsPlayerInVision();
        GameObject playerObject = handler.GetPlayerObject();

        if (!isInRange)
        {
            agent.destination = playerObject.transform.position;
        }

        Vector3 directionTowardsPlayer = (playerObject.transform.position - handler.transform.position).normalized;
        handler.RotateTowardsPlayer(directionTowardsPlayer);

        if (isInRange && !isInVision)
            agent.stoppingDistance--;

        if (isInRange && isInVision)
        {
            handler.ChangeState(handler.attackingState);
        }

        if (!isInVision)
        {
            if (handler.health > 80)
            {
                isPatrolingArea = true;
                handler.StartCoroutine(PatrolArea(handler));
            } else
            {
                handler.ChangeState(handler.idleState);
            }
        }
    }

    public override void ExitState(AIHandler handler)
    {

    }

    private IEnumerator PatrolArea(AIHandler handler)
    {
        Debug.Log("Starting patrol!");

        Vector3 startPos = handler.transform.position;
        Vector3 patrolPoint = startPos + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

        NavMeshAgent agent = handler.GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolPoint);

        float patrolTime = 5f;
        float elapsedTime = 0f;

        while (elapsedTime < patrolTime)
        {
            elapsedTime += Time.deltaTime;
            if (Vector3.Distance(agent.transform.position, patrolPoint) < 1)
            {
                patrolPoint = startPos + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
                agent.SetDestination(patrolPoint);
            }
            yield return null;
        }

        Debug.Log("Patrol complete, going idle.");
        handler.ChangeState(handler.idleState);
    }

}
