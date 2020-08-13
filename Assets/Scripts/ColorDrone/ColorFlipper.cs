using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlipper : MonoBehaviour
{
    public static ColorDroneSpotlightColor activeColor;

    void Start()
    {
        activeColor = ColorDroneSpotlightColor.Red;
    }

    public static void FlipColor()
    {
        switch(activeColor)
        {
            case ColorDroneSpotlightColor.Red:
                activeColor = ColorDroneSpotlightColor.Blue;
                break;
            case ColorDroneSpotlightColor.Blue:
                activeColor = ColorDroneSpotlightColor.Red;
                break;
            default:
                return;
        }
    }
}
