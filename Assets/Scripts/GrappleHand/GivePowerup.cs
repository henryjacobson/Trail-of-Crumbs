using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePowerup : MonoBehaviour
{
    [SerializeField]
    private PowerUp powerUp = PowerUp.ExtendedRange;
    [SerializeField]
    private float duration = 25;
    [SerializeField]
    private float rechargeDuration = 30;

    private float rechargeTimer;

    void Start()
    {
        this.rechargeTimer = 0;
    }

    void Update()
    {
        if (rechargeTimer > 0)
        {
            this.rechargeTimer -= Time.deltaTime;
        } else
        {
            rechargeTimer = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && this.rechargeTimer > 0)
        {
            this.rechargeTimer = this.rechargeDuration;
            GameObject.FindObjectOfType<GrappleHandController>().SetPowerUp(this.powerUp, this.duration);
        }
    }
}
