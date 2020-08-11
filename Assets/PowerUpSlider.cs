using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSlider : MonoBehaviour
{
    [SerializeField]
    private float backgroundDarkenMultiplier = 0.2f;

    private Slider slider;
    private Image fillImage;
    private Image bgImage;

    void Start()
    {
        this.slider = this.GetComponent<Slider>();
        this.fillImage = this.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        this.bgImage = this.transform.Find("Background").GetComponent<Image>();
    }

    void Update()
    {
        float value = this.slider.value;
        if (value <= 0)
        {
            this.bgImage.enabled = false;
            this.fillImage.enabled = false;
        } else
        {
            this.bgImage.enabled = true;
            this.fillImage.enabled = true;
        }
    }

    public void SetValue(float value)
    {
        this.slider.value = value;
    }

    public void SetMax(float value)
    {
        this.slider.maxValue = value;
    }

    public void SetColor(Color color)
    {
        this.fillImage.color = color;

        Color bgColor = color * this.backgroundDarkenMultiplier;
        this.bgImage.color = bgColor;
    }
}
