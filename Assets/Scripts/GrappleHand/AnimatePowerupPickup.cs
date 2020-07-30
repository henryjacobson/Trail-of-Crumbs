using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePowerupPickup : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 90;
    [SerializeField]
    float bobRate = .8f;
    [SerializeField]
    float bobAmplitude = .25f;

    private float originY;
    private Quaternion initialRotation;

    void Start()
    {
        this.originY = this.transform.position.y;
        this.initialRotation = this.transform.rotation;
    }

    void Update()
    {
        this.Bob();
        this.Rotate();
    }

    private void Bob()
    {
        float t = Time.time;
        t *= this.bobRate * 2 * Mathf.PI;

        float yOffset = this.bobAmplitude * ((Mathf.Sin(t) + 1) / 2);
    }

    private void Rotate()
    {
        Quaternion ySpin = Quaternion.AngleAxis(this.rotateSpeed * Time.time, Vector3.up);
        this.transform.rotation = ySpin * this.initialRotation;
    }
}
