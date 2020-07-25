using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandSFX : MonoBehaviour
{
    [SerializeField]
    public List<AudioClip> landSFX;

    private CharacterController cc;

    private bool previousGroundedState;

    void Start()
    {
        this.cc = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (previousGroundedState != this.cc.isGrounded && this.cc.isGrounded)
        {
            this.PlaySFX();
        }
        this.previousGroundedState = this.cc.isGrounded;
    }

    void PlaySFX()
    {
        int randIndex = Random.Range(0, this.landSFX.Count);
        AudioClip sfx = this.landSFX[randIndex];
        AudioSource.PlayClipAtPoint(sfx, this.transform.position);
    }
}
