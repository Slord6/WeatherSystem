using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDirectionVisualiser : NoiseMapVisualiser
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void RefreshTexture()
    {
        Vector2 output = WeatherSystem.Internal.Generators.GetDirectionalNoise(transform.position.x, transform.position.z, maxX, maxY, scale, Time.timeSinceLevelLoad * 0.05f);
        Debug.DrawLine(transform.position, transform.position + new Vector3(output.x, 0, output.y), Color.blue);
    }
}
