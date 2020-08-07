using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class ConductorBehavior : MonoBehaviour
{
    public float attackDistance = 5f;
    public Transform head;

    FSMStates state;
    Animator anim;
    NavMeshAgent agent;
    GameObject player;
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
        player = GameObject.FindGameObjectWithTag("Player");
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
        agent.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            anim.SetTrigger("nearPlayer");
            state = FSMStates.Attacking;
            player.GetComponent<Player_Movement>().enabled = false;
            Camera_Control[] controls = player.GetComponentsInChildren<Camera_Control>();
            foreach (Camera_Control control in controls)
            {
                control.enabled = false;
            }
        }
    }

    void AttackingUpdate()
    {
        mainCamera.transform.LookAt(head);
        agent.enabled = false;
        Vector3 target = player.transform.position;
        target.y = transform.position.y;
        transform.LookAt(target);
    }

    public void PlayerSeen()
    {
        anim.SetTrigger("playerSeen");
        state = FSMStates.Standing;
    }

    public void Attack()
    {

    }
}
