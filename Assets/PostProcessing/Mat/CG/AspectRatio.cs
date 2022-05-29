using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AspectRatio
{
    public static Vector2 GetAspectRatio(int width, int height)
    {
        float ratio_w = (float)width / (float)height;
        int ratio_h = 0;

        while (true)
        {
            ratio_h++;
            if (System.Math.Round(ratio_w * ratio_h, 2) == Mathf.RoundToInt(ratio_w * ratio_h))
            {
                break;
            }
        }
        Vector2 acpectRatio = new Vector2((float)System.Math.Round(ratio_w * ratio_h, 2), ratio_h);
        return acpectRatio;
    }
}