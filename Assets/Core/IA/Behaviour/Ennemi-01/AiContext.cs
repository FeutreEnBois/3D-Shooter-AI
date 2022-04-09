using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// The context is a shared object every node has access to.
// Commonly used components and subsytems should be stored here
public class AiContext : Context
{
    public Animator animator;
    public Rigidbody physics;
    public NavMeshAgent agent;
    public SphereCollider sphereCollider;
    public BoxCollider boxCollider;
    public CapsuleCollider capsuleCollider;
    public CharacterController characterController;
    public AiWeapon weapon;
    // Add other game specific systems here

    public override Context CreateFromGameObject(GameObject gameObject)
    {
        animator = gameObject.GetComponent<Animator>();
        physics = gameObject.GetComponent<Rigidbody>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        characterController = gameObject.GetComponent<CharacterController>();
        weapon = gameObject.GetComponent<AiWeapon>();
        // Add whatever else you need here...

        return this;
    }
}
