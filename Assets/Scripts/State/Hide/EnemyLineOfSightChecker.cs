using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float FieldOfView = 90f;
    public LayerMask LineOfSightLayer;

    public delegate void GainSightEvent(Transform target);
    public GainSightEvent onGainSight;
    public delegate void LoseSightEvent(Transform target);
    public LoseSightEvent onLoseSight;

    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        
        if (!CheckLineOfSight(other.transform))
        {
            Debug.Log(other.name);
            CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("exit");
        onLoseSight?.Invoke(other.transform);
        if (CheckForLineOfSightCoroutine != null)
        {
            StopCoroutine(CheckForLineOfSightCoroutine);
        }
    }

    private bool CheckLineOfSight(Transform target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, direction);
        if (dotProduct >= Mathf.Cos(FieldOfView))
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, /*Collider.radius*/Mathf.Infinity, LineOfSightLayer))
            {
                Debug.Log("hit!");
                Debug.DrawRay(transform.position, direction * 10, Color.red, 1);
                onGainSight?.Invoke(target);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, direction * 10, Color.blue, 1);
                Debug.Log("Did not Hit");
            }

        }
        return false;
    }

    private IEnumerator CheckForLineOfSight(Transform target)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        
        while (!CheckLineOfSight(target))
        {
            
            yield return wait;
        }
    }
}
