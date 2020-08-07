using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class ConductorBehavior : MonoBehaviour
{
    public float attackDistance = 5f;

    FSMStates state;
    Animator anim;
    NavMeshAgent agent;
    Transform player;
    Camera mainCamera;

    enum FSMStates
    {
        Driving,
        Standing,
        Running,
        Attacking
    }

    // Start is called before the first frame update
    void Start()
    {
        state = FSMStates.Driving;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case FSMStates.Standing:
                StandingUpdate();
                break;
            case FSMStates.Running:
                RunningUpdate();
                break;
            case FSMStates.Attacking:
                AttackingUpdate();
                break;
        }
    }

    void StandingUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Running"))
        {
            state = FSMStates.Running;
            agent.enabled = true;
        }
    }

    void RunningUpdate()
    {
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            anim.SetTrigger("nearPlayer");
            state = FSMStates.Attacking;
        }
    }

    void AttackingUpdate()
    {
        mainCamera.transform.LookAt(transform);
        agent.enabled = false;
        Vector3 target = player.position;
        target.y = transform.position.y;
        transform.LookAt(target);
    }

    public void PlayerSeen()
    {
        anim.SetTrigger("playerSeen");
        state = FSMStates.Standing;
    }
}
