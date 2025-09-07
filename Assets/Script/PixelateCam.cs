using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelateCam : MonoBehaviour
{
    [Range(1, 10)] public float pixelate;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;

        // Use Mathf.RoundToInt() to convert the float result to an integer
        int width = Mathf.RoundToInt(source.width / pixelate);
        int height = Mathf.RoundToInt(source.height / pixelate);

        // Make sure the width and height are at least 1 pixel
        if (width < 1) width = 1;
        if (height < 1) height = 1;

        RenderTexture resultTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
        resultTexture.filterMode = FilterMode.Point;

        Graphics.Blit(source, resultTexture);
        Graphics.Blit(resultTexture, destination);

        RenderTexture.ReleaseTemporary(resultTexture);
    }
}
