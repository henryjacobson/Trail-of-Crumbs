using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlipper : MonoBehaviour
{
    public static ColorDroneSpotlightColor activeColor;

    void Start()
    {
        activeColor = ColorDroneSpotlightColor.Red;

        LevelManager.onLevelReset += this.ResetColor;
    }

    void OnDestroy()
    {
        LevelManager.onLevelReset -= this.ResetColor;
    }

    private void ResetColor()
    {
        Debug.Log("RESET");
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
