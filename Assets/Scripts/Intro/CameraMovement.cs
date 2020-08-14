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
    public float afterRot = 0f;
    public float duration;

    public bool useStart = false;
    public Vector3 start;

    Quaternion initRot;
    float timer;
    float rotDone;
    Quaternion destQuat;
    bool doneRotating;

    private void OnEnable()
    {
        if (useStart)
        {
            transform.localPosition = start;
        }
    }

    private void Start()
    {


        if (moveForward)
            direction =
                transform.forward;
        initRot = transform.rotation;
        timer = 0;
        destQuat = Quaternion.Euler(destRot);
        rotDone = duration - afterRot;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;
        if (rotate && timer >= rotDelay && timer < rotDone)
        {
            float t = (timer - rotDelay) / (rotDone - rotDelay);
            transform.rotation = Quaternion.Slerp(initRot, destQuat, t);
        }
        else if (rotate && timer > rotDelay && !doneRotating)
        {
            doneRotating = true;
            transform.rotation = destQuat;
        }
    }
}
