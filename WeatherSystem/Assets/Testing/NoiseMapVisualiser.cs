using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapVisualiser : MonoBehaviour
{
    [SerializeField]
    protected new Renderer renderer;

    [SerializeField]
    [Range(1, 2056)]
    protected int textureWidth = 256;
    [SerializeField]
    [Range(1, 2056)]
    protected int textureHeight = 256;
    [SerializeField]
    [Range(0.1f, 100.0f)]
    protected float scale = 1.0f;
    [SerializeField]
    protected float fixedOffsetX = 0.0f;
    [SerializeField]
    protected float fixedOffsetY = 0.0f;
    [SerializeField]
    protected float maxX = 1000.0f;
    [SerializeField]
    protected float maxY = 1000.0f;

    // Use this for initialization
    protected virtual void Start()
    {
        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        RefreshTexture();
    }

    protected virtual void RefreshTexture()
    {
        Texture2D newTexture = new Texture2D(textureWidth, textureHeight);

        Vector2 center = new Vector2((textureWidth / 2), (textureHeight / 2));

        float xOffsetPos = transform.position.x - (int)center.x + fixedOffsetX;
        //z here because players move latterally along the x/z plane
        float yOffsetPos = transform.position.z - (int)center.y + fixedOffsetY;

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float output = WeatherSystem.Internal.Generators.GetPerlinNoise(xOffsetPos + x, yOffsetPos + y, maxX, maxY, scale, 0.00f);//Time.timeSinceLevelLoad * 0.05f);
                newTexture.SetPixel(x, y, new Color(output,output,output));
            }
        }

        renderer.material.mainTexture = MarkCenter(center, newTexture, Color.green);
        newTexture.Apply();
    }

    protected Texture2D MarkCenter(Vector2 center, Texture2D texture, Color color)
    {
        Color[] colors = new Color[] { color, color, color, color, color, color, color, color, color };
        texture.SetPixels((int)center.x - 1, (int)center.y - 1, 3, 3, colors);
        return texture;
    }
}