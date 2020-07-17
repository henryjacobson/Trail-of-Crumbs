using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPrimeBehavior : MonoBehaviour
{
    public float followDistance = 5f;
    public float speed = 6f;
    Transform player;
    bool following;
    float playerHeightOffset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        following = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(following)
        {
            var self2d = new Vector2(transform.position.x, transform.position.z);
            var player2d = new Vector2(player.position.x, player.position.z);
            if (Vector2.Distance(self2d, player2d) > followDistance)
            {
                var offset = (self2d - player2d).normalized * followDistance;
                var target = player.position + new Vector3(offset.x, playerHeightOffset, offset.y);
                transform.position = target;
            }
        }
        if (Mathf.Abs(transform.position.x) > 60 || Mathf.Abs(transform.position.z) > 60)
        {
            FindObjectOfType<LevelManager>().LevelWon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.Equals(player))
        {
            following = true;
            playerHeightOffset = transform.position.y - player.position.y;
        }
    }
}
