using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHideState : AiState
{
    private Coroutine MovementCoroutine;
    private Collider[] Colliders = new Collider[10]; // more is less performant, but more options;

    public void Enter(AiAgent agent)
    {
        if(agent.weapons != null)agent.weapons.DeactivateWeapon();

        agent.navMeshAgent.ResetPath();

        //Debug.Log("hiding...");
        agent.hideMovement.GainSight();
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Hide;
    }

    public void Update(AiAgent agent)
    {
        if (agent.hideMovement.hided)
        {
            agent.hideMovement.LoseSight();
            agent.hideMovement.destinationSet = false;
            agent.stateMachine.ChangeState(AiStateId.AttackPlayer);
        }
    }



}
