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
    private GameObject powerupTimerTextObject;
    [SerializeField]
    private GameObject powerupIndicatorObject;

    private float rechargeTimer;
    
    

    void Start()
    {
        this.rechargeTimer = 0;
        if (powerupTimerTextObject == null)
        {
            this.powerupTimerTextObject = GameObject.Find("PowerupTimerText");
        }
    }

    void Update()
    {
        if (rechargeTimer > 0)
        {
            this.rechargeTimer -= Time.deltaTime;

            LevelManager lm = GameObject.FindObjectOfType<LevelManager>()
                .GetComponent<LevelManager>();
                
            Transform lmTransform = lm.transform;
            
            Text powerTime = powerupTimerTextObject.GetComponent<Text>();
            
            powerTime.text = 
                    rechargeTimer.ToString("f2");
                    
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
            this.rechargeTimer = this.rechargeDuration;
            GameObject.FindObjectOfType<GrappleHandController>().SetPowerUp(this.powerUp, this.duration);
        }
    }
}
