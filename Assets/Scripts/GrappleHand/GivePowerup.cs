using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePowerup : MonoBehaviour
{
    [SerializeField]
    private PowerUp powerUp = PowerUp.ExtendedRange;
    [SerializeField]
    private float duration = 25;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindObjectOfType<GrappleHandController>().SetPowerUp(this.powerUp, this.duration);
        }
    }
}
