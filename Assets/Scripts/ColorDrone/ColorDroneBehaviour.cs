using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDroneBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> rotations;
    [SerializeField]
    private float rotateSpeed = 60;
    [SerializeField]
    private float waitBetweenPoints = 2.5f;
    [SerializeField]
    private float waitOffset = 0;

    private float waitTimer;

    private List<Quaternion> orientations;
    private Quaternion initialOrientation;

    void Start()
    {
        this.orientations = this.GetQuaternions(this.rotations);
        this.initialOrientation = this.orientations.Count > 0 ? this.orientations[0] : this.transform.rotation;
        this.StartTimer();
        this.waitTimer += this.waitOffset;

        LevelManager.onLevelReset += this.Reset;
    }

    void OnDestroy()
    {
        LevelManager.onLevelReset -= this.Reset;
    }

    private List<Quaternion> GetQuaternions(List<Vector3> rotations)
    {
        List<Quaternion> result = new List<Quaternion>();
        foreach(Vector3 rotation in rotations)
        {
            Quaternion q = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            result.Add(q);
        }
        return result;
    }

    void Update()
    {
        if (!LevelManager.isGameOver && this.orientations.Count > 0)
        {
            this.RotateTowardsTarget();
        }
    }

    private void RotateTowardsTarget()
    {
        if (this.waitTimer <= 0)
        {
            Quaternion target = this.orientations[0];
            if (this.transform.rotation == target)
            {
                CycleList<Quaternion>(this.orientations);
                target = this.orientations[0];
                this.StartTimer();
            }

            this.transform.rotation = Quaternion.RotateTowards(
                this.transform.rotation, target, this.rotateSpeed * Time.deltaTime);
        }
        else
        {
            this.waitTimer -= Time.deltaTime;
        }
    }

    private static void CycleList<T>(List<T> list)
    {
        T t = list[0];
        list.RemoveAt(0);
        list.Add(t);
    }

    private void Reset()
    {
        this.transform.rotation = this.initialOrientation;
        this.StartTimer();
        this.waitTimer += this.waitOffset;
    }

    private void StartTimer()
    {
        this.waitTimer = this.waitBetweenPoints;
    }
}
