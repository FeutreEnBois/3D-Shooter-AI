using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawSphere : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }
}
