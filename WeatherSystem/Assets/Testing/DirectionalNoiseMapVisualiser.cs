using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeatherSystem.Internal;

public class DirectionalNoiseMapVisualiser : NoiseMapVisualiser
{
    protected float trackedX = 0.0f;
    protected float trackedY = 0.0f;
    [SerializeField]
    [Range(0.1f, 10.0f)]
    protected float directionStrength = 3.0f;
    [SerializeField]
    protected bool windDrivenMap = true;
    [SerializeField]
    protected UnityEngine.UI.Text outputText;
    [SerializeField]
    protected GameObject samplePointMarker;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void RefreshTexture()
    {
        if (windDrivenMap)
        {
            WindDrivenMap();
        }
        else
        {
            DirectionVisualised();
        }
    }

    protected virtual void DirectionVisualised()
    {
        Texture2D newTexture = new Texture2D(textureWidth, textureHeight);

        Vector2 center = new Vector2((textureWidth / 2), (textureHeight / 2));

        float xOffsetPos = transform.position.x - center.x + fixedOffsetX;
        //z here because players move latterally along the x/z plane
        float yOffsetPos = transform.position.z - center.y + fixedOffsetY;
        
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                Vector2 wind = Generators.GetDirectionalNoise(xOffsetPos + x, yOffsetPos + y, maxX, maxY, scale, Time.timeSinceLevelLoad * 0.1f); //0.0f);
                //Make between 0->1 for visualisation
                wind.x = (wind.x + 1) / 2.0f;
                wind.y = (wind.y + 1) / 2.0f;
                newTexture.SetPixel(x, y, new Color(wind.x, wind.y, 0.0f));
            }
        }

        renderer.material.mainTexture = MarkCenter(center, newTexture, Color.green);
        newTexture.Apply();
    }

    protected virtual void WindDrivenMap()
    {
        Texture2D newTexture = new Texture2D(textureWidth, textureHeight);

        Vector2 center = new Vector2((textureWidth / 2), (textureHeight / 2));

        float xOffsetPos = transform.position.x - center.x + fixedOffsetX;
        //z here because players move latterally along the x/z plane
        float yOffsetPos = transform.position.z - center.y + fixedOffsetY;

        
        Vector2 wind = Generators.GetDirectionalNoise(xOffsetPos, yOffsetPos, maxX, maxY, scale, Time.timeSinceLevelLoad * 0.05f);
        wind *= directionStrength;
        trackedX += wind.x;
        trackedY += wind.y;
        xOffsetPos += trackedX;
        yOffsetPos += trackedY;
        if (outputText != null)
        {
            outputText.text = "Wind Info: TrackedX,TrackedY = " + trackedX.ToString("#.##") + trackedY.ToString(", #.##");
        }
        if (samplePointMarker != null)
        {
            samplePointMarker.transform.position = new Vector3(xOffsetPos, 0, yOffsetPos);
        }
        Debug.DrawLine(transform.position, transform.position + new Vector3(xOffsetPos, 0.1f, yOffsetPos).normalized, Color.magenta);

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float output = WeatherSystem.Internal.Generators.GetPerlinNoise(xOffsetPos + x, yOffsetPos + y, maxX, maxY, scale, 0.00f);//Time.timeSinceLevelLoad * 0.05f);
                newTexture.SetPixel(x, y, new Color(output, output, output));
            }
        }

        renderer.material.mainTexture = MarkCenter(center, newTexture, Color.green);
        newTexture.Apply();
    }
}