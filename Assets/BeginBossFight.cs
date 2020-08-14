using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginBossFight : MonoBehaviour
{
    DisableControls controls;
    CrumbsBanksAI boss;

    private void Start()
    {
        boss = FindObjectOfType<CrumbsBanksAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        boss.SpotPlayer();

        Destroy(gameObject);
    }
}
