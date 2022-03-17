using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 direction;
    public void Enter(AiAgent agent)
    {
        agent.ragdoll.ActivateRagdoll();
        direction.y = agent.config.dieForceY;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        agent.ui.gameObject.SetActive(false);
        agent.mesh.updateWhenOffscreen = true;
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Update(AiAgent agent)
    {
    }
}
