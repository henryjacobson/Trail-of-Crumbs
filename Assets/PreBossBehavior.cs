using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBossBehavior : MonoBehaviour
{
    public GameObject preBoss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(preBoss, other.gameObject.transform);

            Camera.main.GetComponent<AudioSource>().clip = null;

            Destroy(gameObject);
        }
    }
}
