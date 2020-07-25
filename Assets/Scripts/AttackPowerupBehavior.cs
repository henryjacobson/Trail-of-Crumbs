using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerupBehavior : MonoBehaviour
{
    public float powerupTime = 10f;
    public float rechargeTime = 20f;

    float rechargeTimer;
    bool recharging;

    // Start is called before the first frame update
    void Start()
    {
        recharging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (recharging)
        {
            rechargeTimer -= Time.deltaTime;
            if (rechargeTimer <= 0)
            {
                recharging = false;
                gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("GrappleHand"))
        {
            GrappleHandController hand = GameObject.FindGameObjectWithTag("GrappleHand").GetComponent<GrappleHandController>();
            hand.AttackPowerup(powerupTime);
            gameObject.SetActive(false);
            recharging = true;
            rechargeTimer = rechargeTime;
        }
    }
}
