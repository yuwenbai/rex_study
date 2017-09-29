/**
 * @Author lyb
 * 无损压缩图片
 *
 */

using UnityEngine;

public class Tools_TexGzip
{
    public static Texture2D TexGzipBegin(Texture2D originalTexture, float scaleFactor)
    {
        Texture2D newTexture = new Texture2D(Mathf.CeilToInt(originalTexture.width * scaleFactor),
                                             Mathf.CeilToInt(originalTexture.height * scaleFactor),
                                             TextureFormat.RGB24, false);
        float scale = 1.0f / scaleFactor;
        int maxX = originalTexture.width - 1;
        int maxY = originalTexture.height - 1;
        for (int y = 0; y < newTexture.height; y++)
        {
            for (int x = 0; x < newTexture.width; x++)
            {
                float targetX = x * scale;
                float targetY = y * scale;
                int x1 = Mathf.Min(maxX, Mathf.FloorToInt(targetX));
                int y1 = Mathf.Min(maxY, Mathf.FloorToInt(targetY));
                int x2 = Mathf.Min(maxX, x1 + 1);
                int y2 = Mathf.Min(maxY, y1 + 1);

                float u = x * 1.0f / (newTexture.width - 1);
                float v = y * 1.0f / (newTexture.height - 1);
                newTexture.SetPixel(x, y, originalTexture.GetPixelBilinear(u, v));
            }
        }
        newTexture.Apply();

        return newTexture;
    }
}