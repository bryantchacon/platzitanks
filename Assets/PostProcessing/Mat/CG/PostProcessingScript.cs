using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessingScript : MonoBehaviour
{
    public Material effect;

    void Start()
    {
        SetPixelSize();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, effect);
    }

    private void SetPixelSize()
    {
        Vector2 aspectRatio = AspectRatio.GetAspectRatio(Screen.width, Screen.height);
        float minValue = Mathf.Min(aspectRatio.x, aspectRatio.y);

        float ratio_y = aspectRatio.x / minValue;
        float ratio_x = aspectRatio.y / minValue;

        float pixelSize = Mathf.Max(ratio_x, ratio_y);

        effect.SetFloat("_PixelSize", pixelSize);
        effect.SetFloat("_RatioY", ratio_y);
        effect.SetFloat("_RatioX", ratio_x);
    }
}