using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class SpotlightDetectPlayer : MonoBehaviour
{
    private Light light;

    private Transform player;

    void Start()
    {
        this.light = this.GetComponent<Light>();
        this.light.type = LightType.Spot;

        this.player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (this.DetectPlayer())
        {
            Debug.Log(this.name + " detects the player");
        }
    }

    private bool DetectPlayer()
    {
        Vector3 toPlayer = (this.player.position - this.transform.position).normalized;

        float angleToPlayer = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(toPlayer, this.transform.forward));

        if (angleToPlayer > this.light.spotAngle / 2)
        {
            return false;
        } else
        {
            RaycastHit hit = this.castToPlayer(toPlayer);
            if (hit.collider == null)
            {
                return false;
            } else if (hit.collider.CompareTag("Player"))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }

    private RaycastHit castToPlayer(Vector3 toPlayer)
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, toPlayer, out hit, this.light.range);
        return hit;
    }
}
