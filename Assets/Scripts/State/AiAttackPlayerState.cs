using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackPlayerState : AiState
{
    public void Enter(AiAgent agent)
    {
        agent.weapons.ActivateWeapon();
        agent.weapons.SetTarget(agent.playerTransform);
        //agent.navMeshAgent.stoppingDistance = 8.0f;
        //agent.weapons.SetFiring(true);
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }

    public AiStateId GetId()
    {
        return AiStateId.AttackPlayer;
    }

    public void Update(AiAgent agent)
    {
        ///TODO
        /// Quand le joueur est en Attack state il doit follow du regard le joueur
        ///

        agent.transform.LookAt(agent.playerTransform);
        //agent.navMeshAgent.destination = agent.playerTransform.position;
        UpdateFiring(agent);
        if (agent.playerTransform.GetComponent<Health>().IsDead())
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }

        /*if (Vector3.Distance(agent.playerTransform.position, agent.transform.position) >= 0.1f)
        {
            Debug.Log("switch");
            agent.stateMachine.ChangeState(AiStateId.Hide);
        }*/

    }

    private void UpdateFiring(AiAgent agent)
    {
        if (agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            agent.weapons.SetFiring(true);
        }
        else
        {
            agent.weapons.SetFiring(false);
        }
    }
}
