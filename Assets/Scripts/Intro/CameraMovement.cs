using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool moveForward = false;
    public Vector3 direction;
    public float speed;

    public bool rotate = false;
    public Vector3 destRot;
    public float rotDelay = 0f;
    public float duration;

    Quaternion initRot;
    float timer;
    Quaternion destQuat;

    private void Start()
    {
        if (moveForward)
            direction =
                transform.forward;
        initRot = transform.rotation;
        timer = 0;
        destQuat = Quaternion.Euler(destRot);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;
        if (rotate && timer >= rotDelay)
        {
            float t = (timer - rotDelay) / (duration - rotDelay);
            transform.rotation = Quaternion.Slerp(initRot, destQuat, t);
        }
    }
}
