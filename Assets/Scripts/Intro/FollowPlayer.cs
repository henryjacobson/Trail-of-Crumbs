using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerCamera;
    public float duration;
    
    Vector3 startPos;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(playerCamera.position.x, playerCamera.position.y, transform.position.z);
        transform.position = startPos;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, playerCamera.position, timer / duration);
    }
}
