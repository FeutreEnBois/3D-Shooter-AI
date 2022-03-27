using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFindWeaponState : AiState
{
    GameObject pickup;
    GameObject[] pickups = new GameObject[1];
    public void Enter(AiAgent agent)
    {
        pickup = null;
        agent.navMeshAgent.speed = 5;
        
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.FindWeapon;
    }

    public void Update(AiAgent agent)
    {
        //Find Pickup
        if (!pickup)
        {
            pickup = findPickup(agent);

            if (pickup)
            {
                CollectPickup(agent, pickup);
            }
        }

        // Wander
        if (!agent.navMeshAgent.hasPath)
        {
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            Vector3 min = worldBounds.min.position;
            Vector3 max = worldBounds.max.position;

            Vector3 randomPosition = new Vector3(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z));

            agent.navMeshAgent.destination = randomPosition;
        }

        if (agent.weapons.HasWeapon())
        {
            agent.stateMachine.ChangeState(AiStateId.AttackPlayer);
        }
    }

    private GameObject findPickup(AiAgent agent)
    {
        int count = agent.sensor.Filter(pickups, "Pickup");
        if(count > 0)
        {
            return pickups[0];
        }
        return null;
    }

    void CollectPickup(AiAgent agent, GameObject pickup)
    {
        agent.navMeshAgent.destination = pickup.transform.position;
    }
}
