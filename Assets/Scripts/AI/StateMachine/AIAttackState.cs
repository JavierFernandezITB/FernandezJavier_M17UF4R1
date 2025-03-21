using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIBaseState
{
    public override void EnterState(AIHandler handler)
    {
        Debug.Log("Attacking!");
        handler.GetPlayerObject().GetComponent<PlayerObject>().DealDamage(handler.enemyType == EnemyType.Melee ? handler.meleeDamage : handler.rangedDamage);
        handler.StartCoroutine(Cooldown(handler));
    }

    public override void UpdateState(AIHandler handler)
    {
        
    }

    public override void ExitState(AIHandler handler)
    {

    }

    private IEnumerator Cooldown(AIHandler handler)
    {
        yield return new WaitForSeconds(.75f);
        handler.ChangeState(handler.chasingState);
    }
}
