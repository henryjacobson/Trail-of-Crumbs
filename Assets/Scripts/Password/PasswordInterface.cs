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
    private Renderer screenRenderer;
    [SerializeField]
    private float distanceToActivate = 2;
    [SerializeField]
    private int passwordLength = 4;
    [SerializeField]
    private Color wrongColor = Color.red;
    [SerializeField]
    private Color rightColor = Color.green;

    private Transform player;

    private bool solved;

    private Color defaultColor;

    private bool animatingWrongAnswer;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;

        this.defaultColor = this.screenRenderer.material.color;

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
        if (!this.solved && !this.animatingWrongAnswer)
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
                this.RightAnswer();
            } else
            {
                this.WrongAnswer();
            }
        }
    }

    private void RightAnswer()
    {
        this.screenRenderer.material.color = Color.green;
        this.solved = true;
    }

    private void WrongAnswer()
    {
        StartCoroutine("WrongAnswerFlash", 0.1f);
    }

    private IEnumerator WrongAnswerFlash(float delay)
    {
        this.animatingWrongAnswer = true;   
        for(int i = 0; i < 3; i++)
        {
            this.screenRenderer.material.color = this.wrongColor;
            yield return new WaitForSeconds(delay);
            this.screenRenderer.material.color = this.defaultColor;
            yield return new WaitForSeconds(delay);
        }
        this.Reset();
        this.animatingWrongAnswer = false;
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
            && this.inputField.text.Length < this.passwordLength
            && !this.animatingWrongAnswer;
    }
}
