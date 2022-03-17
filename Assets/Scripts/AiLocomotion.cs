using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AiLocomotion : MonoBehaviour
{
    public float maxTimer = 1.0f;
    public float maxDistance = 1.0f;

    public Transform playerTransform;
    NavMeshAgent agent;
    Animator animator;

    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            float distance = (playerTransform.position - agent.destination).sqrMagnitude;
            if(distance > maxDistance * maxDistance)
            {
                agent.destination = playerTransform.position;
            }
            timer = maxTimer;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
