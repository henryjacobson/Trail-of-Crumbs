using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPrimeBehavior : MonoBehaviour
{
    public float followDistance = 5f;
    public float speed = 6f;
    Transform player;
    bool following;
    float height;
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
            if(Vector3.Distance(transform.position, player.position) > followDistance)
            {
                var target = player.position;
                target.y = height;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
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
            Debug.Log("Collision Enter");
            following = true;
            height = transform.position.y;
        }
    }
}
