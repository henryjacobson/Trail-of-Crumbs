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
    [SerializeField]
    private int passwordLength = 4;

    private Transform player;

    private bool solved;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;

        LevelManager.onLevelReset += this.Reset;
    }

    void OnDestroy()
    {
        LevelManager.onLevelReset -= this.Reset;
    }

    void Update()
    {
        this.SetInputEnabled();
        this.LockSelection();
        if (!this.solved)
        {
            this.CheckPasswordSolved();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, this.distanceToActivate);
    }

    private void CheckPasswordSolved()
    {
        string text = this.inputField.text;
        if (text.Length >= this.passwordLength)
        {
            if (FindObjectOfType<PasswordManager>().VerifyPassword(text))
            {
                this.solved = true;
            } else
            {
                Debug.Log(text + " " + FindObjectOfType<PasswordManager>().GetPassword());
                this.Reset();
            }
        }
    }

    private void Reset()
    {
        this.inputField.text = "";
    }

    private void LockSelection()
    {
        this.inputField.caretPosition = this.inputField.text.Length;
    }

    private void SetInputEnabled()
    {
        if (this.CanUseInterface())
        {
            this.inputField.ActivateInputField();
        }
        else
        {
            this.inputField.DeactivateInputField();
        }
    }

    private bool CanUseInterface()
    {
        return !this.solved 
            && Vector3.Distance(this.player.position, this.transform.position) <= this.distanceToActivate
            && this.inputField.text.Length < this.passwordLength;
    }
}
