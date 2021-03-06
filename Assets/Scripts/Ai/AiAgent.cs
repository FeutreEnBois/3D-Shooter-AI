using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public AiStateId currentState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;

    public Ragdoll ragdoll;
    public SkinnedMeshRenderer mesh;
    public UIHealthBar ui;
    public Transform playerTransform;
    public AiWeapon weapons;
    public AiSensor sensor;

    public EnemyMovement hideMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        hideMovement = GetComponent<EnemyMovement>();
        weapons = GetComponent<AiWeapon>();
        sensor = GetComponent<AiSensor>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        ui = GetComponentInChildren<UIHealthBar>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiFindWeaponState());
        stateMachine.RegisterState(new AiAttackPlayerState());
        stateMachine.RegisterState(new AiHideState());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
