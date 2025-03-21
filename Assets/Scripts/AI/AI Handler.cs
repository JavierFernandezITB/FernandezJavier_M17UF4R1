using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AIHandler : MonoBehaviour, IDamageable
{
    public int health = 100;
    public int meleeDamage = 25;
    public int rangedDamage = 15;
    public float fieldOfView = 30.0f;
    public float distanceOfView = 5;
    public EnemyType enemyType;
    public float rangedEnemyDistance = 5;
    public float meleeEnemyDistance = 2;

    public AIBaseState currentAiState; 
    public AIIdleState idleState = new AIIdleState();
    public AIAwareState awareState = new AIAwareState();
    public AIChasingState chasingState = new AIChasingState();
    public AIDeadState deadState = new AIDeadState();
    public AIAttackState attackingState = new AIAttackState();
    public AIRunAwayState runningAwayState = new AIRunAwayState();

    public NodeRegion currentNodeRegion;
    public Node currentNode;


    void Start()
    {
        currentAiState = idleState;
        currentAiState.EnterState(this);
    }

    void Update()
    {
        currentAiState.UpdateState(this);
    }

    public void DealDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            ChangeState(deadState);
        } else if (health <= 50)
        {
            ChangeState(runningAwayState);
        }
    }

    public void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void ChangeState(AIBaseState newState)
    {
        currentAiState.ExitState(this);
        currentAiState = newState;
        newState.EnterState(this);
    }

    public void RotateTowardsPlayer(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }

    public GameObject GetPlayerObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public bool IsPlayerInVision()
    {
        // Debug AI fov.
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * distanceOfView, Color.blue);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * distanceOfView, Color.blue);

        GameObject playerObject = GetPlayerObject();
        if (playerObject == null)
            return false;

        Vector3 directionTowardsPlayer = (playerObject.transform.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, directionTowardsPlayer);

        if (angle < fieldOfView / 2)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            if (distanceToPlayer < distanceOfView)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionTowardsPlayer, out hit, distanceOfView))
                {
                    Debug.DrawRay(transform.position, playerObject.transform.position, Color.red);
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool IsInAttackRange()
    {
        GameObject playerObject = GetPlayerObject();
        if (playerObject == null)
            return false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        if (enemyType == EnemyType.Melee && distanceToPlayer < meleeEnemyDistance)
            return true;
        else if (enemyType == EnemyType.Ranged && distanceToPlayer < rangedEnemyDistance)
            return true;
        
        return false;
    }
}
