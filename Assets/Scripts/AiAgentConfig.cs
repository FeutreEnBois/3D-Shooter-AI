using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTimer = 1.0f;
    public float maxDistance = 1.0f;
    public float dieForce = 10f;
    public float dieForceY = 1.0f;
    public float maxSightDistance = 5.0f;
}
