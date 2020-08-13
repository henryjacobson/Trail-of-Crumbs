using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordInterface : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private float distanceToActivate = 2;

    private Transform player;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector3.Distance(this.player.position, this.transform.position) <= this.distanceToActivate) {
            this.inputField.ActivateInputField();
        } else
        {
            this.inputField.DeactivateInputField();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, this.distanceToActivate);
    }
}
