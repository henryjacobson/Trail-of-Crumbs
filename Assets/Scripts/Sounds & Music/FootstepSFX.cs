using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSFX : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> stepSFX;

    public void PlaySFX()
    {
        int randIndex = Random.Range(0, stepSFX.Count);
        AudioClip sfx = this.stepSFX[randIndex];
        AudioSource.PlayClipAtPoint(sfx, this.transform.position);
    }
}
