using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPrimeBehavior : MonoBehaviour
{
    public float followDistance = 5f;
    public float speed = 6f;
    public GameObject[] checkpoints;

    Transform player;
    bool following;
    float playerHeightOffset;
    Animator anim;
    CharacterController playerController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        following = false;
    }

    // Update is called once per frame
    void Update()
    {
        FacePlayer();
        if(following)
        {
            var self2d = new Vector2(transform.position.x, transform.position.z);
            var player2d = new Vector2(player.position.x, player.position.z);

            UpdateAnimation(self2d, player2d);

            if (Vector2.Distance(self2d, player2d) > followDistance)
            {
                var offset = (self2d - player2d).normalized * followDistance;
                var target = player.position + new Vector3(offset.x, playerHeightOffset, offset.y);
                transform.position = target;
            }
            else
            {
                var pos = transform.position;
                transform.position = new Vector3(pos.x, player.position.y + playerHeightOffset, pos.z);
            }
        }
        else
        {
            anim.SetInteger("State", 3);
        }
        if (Mathf.Abs(transform.position.x) > 60 || Mathf.Abs(transform.position.z) > 60)
        {
            FindObjectOfType<LevelManager>().LevelWon();
        }
    }

    void UpdateAnimation(Vector2 self2d, Vector2 player2d)
    {
        if (!playerController.isGrounded)
        {
            anim.SetInteger("State", 2);
        }
        else if (Vector2.Distance(self2d, player2d) > followDistance)
        {
            anim.SetInteger("State", 1);
        }
        else
        {
            anim.SetInteger("State", 0);
        }
    }

    void FacePlayer()
    {
        Vector3 lookRotation = player.position - transform.position;
        lookRotation.y = 0;
        Quaternion lookQuat = Quaternion.LookRotation(lookRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookQuat, Time.deltaTime * 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !following)
        {
            following = true;
            playerHeightOffset = transform.position.y - player.position.y;
            FindObjectOfType<LevelManager>().SetCheckPoint(other.transform.position);
            foreach (GameObject checkpoint in checkpoints)
            {
                checkpoint.SetActive(true);
            }
        }
    }

    public void Checkpoint()
    {
        if (following)
        {
            transform.position = player.position;
        }
    }
}
