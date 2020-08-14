using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(DoorLockPad))]
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
    private int expectedPasswordChunks = 2;
    [SerializeField]
    private Color wrongColor = Color.red;
    [SerializeField]
    private Color rightColor = Color.green;
    [SerializeField]
    private AudioClip wrongSFX;
    [SerializeField]
    private AudioClip rightSFX;
    [SerializeField]
    private AudioClip typeSFX;

    private Transform player;

    private bool solved;
    private bool savedByCheckpoint;

    private Color defaultColor;

    private bool animatingWrongAnswer;

    private DoorLockPad dlp;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").transform;

        this.defaultColor = this.screenRenderer.material.color;

        this.dlp = this.GetComponent<DoorLockPad>();

        this.inputField.onValueChanged.AddListener(this.OnType);

        LevelManager.onLevelReset += this.Reset;
        CheckpointBehavior.onSetCheckpoint += this.OnCheckpoint;
    }

    private void OnType(string value)
    {
        AudioSource.PlayClipAtPoint(this.typeSFX, this.transform.position);
    }

    void OnDestroy()
    {
        LevelManager.onLevelReset -= this.Reset;
        CheckpointBehavior.onSetCheckpoint -= this.OnCheckpoint;
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
            if (FindObjectOfType<PasswordManager>().VerifyPassword(text)
                && SeePasswordDisplay.seenCharacters.Count >= this.expectedPasswordChunks)
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
        AudioSource.PlayClipAtPoint(this.rightSFX, this.transform.position);
        this.screenRenderer.material.color = Color.green;
        this.solved = true;
        this.dlp.Unlock();
    }

    private void WrongAnswer()
    {
        StartCoroutine("WrongAnswerFlash", 0.1f);
    }

    private IEnumerator WrongAnswerFlash(float delay)
    {
        AudioSource.PlayClipAtPoint(this.wrongSFX, this.transform.position);
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
        if (!this.savedByCheckpoint)
        {
            this.inputField.text = "";
            this.solved = false;
            this.dlp.Lock();
            this.screenRenderer.material.color = this.defaultColor;
        }
    }

    private void OnCheckpoint()
    {
        this.savedByCheckpoint = this.solved;
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
