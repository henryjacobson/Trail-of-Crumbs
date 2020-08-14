using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HideIfNoText : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private Image image;

    void Start()
    {
        this.image = this.GetComponent<Image>();
    }

    void Update()
    {
        this.image.enabled = this.text.text != "";
    }
}
