using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class ConductorBehavior : MonoBehaviour
{
    public float attackDistance = 5f;
    public Transform head;
    public AudioClip attackSFX;
    public AudioClip spottedSFX;

    FSMStates state;
    Animator anim;
    NavMeshAgent agent;
    GameObject player;
    Camera mainCamera;
    bool dead;

    enum FSMStates
    {
        Driving,
        Standing,
        Running,
        Attacking,
        PlayerKilled
    }

    // Start is called before the first frame update
    void Start()
    {
        state = FSMStates.Driving;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
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
            agent.enabled = false;
            foreach (Camera_Control control in controls)
            {
                control.enabled = false;
            }
            AudioSource.PlayClipAtPoint(attackSFX, transform.position);
        }
    }

    void AttackingUpdate()
    {
        mainCamera.transform.LookAt(head);
        player.transform.LookAt(new Vector3(head.position.x, player.transform.position.y, head.position.z));
        Vector3 target = player.transform.position;
        target.y = transform.position.y;
        transform.LookAt(target);
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Attack") && info.normalizedTime >= .4)
        {
            state = FSMStates.PlayerKilled;
            FindObjectOfType<LevelManager>().LevelLost();
        }
    }

    public void PlayerSeen()
    {
        anim.SetTrigger("playerSeen");
        state = FSMStates.Standing;
        AudioSource.PlayClipAtPoint(spottedSFX, transform.position);
    }

    public void Attack()
    {
        player.GetComponent<Player_Movement>().enabled = true;
        Camera_Control[] controls = player.GetComponentsInChildren<Camera_Control>();
        foreach (Camera_Control control in controls)
        {
            control.enabled = true;
        }
        dead = true;
        anim.enabled = false;
        agent.enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }
}
