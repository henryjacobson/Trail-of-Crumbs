using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GivePowerup : MonoBehaviour
{
    [SerializeField]
    private PowerUp powerUp = PowerUp.ExtendedRange;
    [SerializeField]
    private float duration = 25;
    [SerializeField]
    private float rechargeDuration = 30;
    [SerializeField]
    private GameObject powerupIndicatorObject;

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

        this.UpdatePowerupIndicator();
    }

    private void UpdatePowerupIndicator()
    {
        if (this.rechargeTimer > 0)
        {
            this.powerupIndicatorObject.SetActive(false);
        } else
        {
            this.powerupIndicatorObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && this.rechargeTimer <= 0)
        {
            bool noPowerupActive = !FindObjectOfType<GrappleHandController>().AnyPowerupActive();

            if (noPowerupActive)
            {
                this.rechargeTimer = this.rechargeDuration;
                GameObject.FindObjectOfType<GrappleHandController>().SetPowerUp(this.powerUp, this.duration);
            }
        }
    }
}
