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
    [SerializeField]
    private AudioClip getPowerupSFX;

    private float rechargeTimer;

    private static List<GivePowerup> powerupTimers = new List<GivePowerup>();

    void Start()
    {
        this.rechargeTimer = 0;

        powerupTimers.Add(this);
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
                AudioSource.PlayClipAtPoint(this.getPowerupSFX, this.transform.position);
                this.rechargeTimer = this.rechargeDuration;
                GameObject.FindObjectOfType<GrappleHandController>().SetPowerUp(this.powerUp, this.duration);
            }
        }
    }

    public static void ResetPowerupTimers()
    {
        foreach(GivePowerup gp in powerupTimers)
        {
            gp.Reset();
        }
    }

    public void Reset()
    {
        this.rechargeTimer = 0;
    }
}
