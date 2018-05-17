using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherSystem.Internal
{
    /// <summary>
    /// The noise generators for the weather system
    /// </summary>
    public static class Generators
    {
        private static float seed = 123.45f;
        private static float temperatureOffset = 1000f;
        private static float humidityOffset = -1000f;
        private static float directionalOffset = 12050.0f;
        private static float intensityOffset = 8760.0f;

        /// <summary>
        /// The current perlin noise seed for the generators
        /// Should not be changed mid-game to avoid strange jumps in active weather
        /// </summary>
        public static float Seed
        {
            get
            {
                return seed;
            }

            set
            {
                seed = value;
            }
        }

        /// <summary>
        /// Get a perlin noise value for a given x,y position with an offset calculated from a drivingValue
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="maxX">The maximum possible value of x</param>
        /// <param name="maxY">The maximum possible value of y</param>
        /// <param name="scale">A scale for the output, applied before the offset value</param>
        /// <param name="offset">An offset value</param>
        /// <returns>The resultant float value</returns>
        public static float GetPerlinNoise(float x, float y, float maxX, float maxY, float scale, float offset)
        {
            float widthInput = ((float)x / maxX) * scale;
            widthInput += offset;
            float heightInput = ((float)y / maxY) * scale;
            heightInput += offset;

            float noise = Mathf.PerlinNoise(widthInput + seed, heightInput - seed);

            return noise;
        }

        /// <summary>
        /// Gets a perlin noise driven normalised temperature value
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="maxX">The maximum possible value of x</param>
        /// <param name="maxY">The maximum possible value of y</param>
        /// <param name="scale">A scale for the output, applied before the offset value</param>
        /// <param name="offset">An offset value</param>
        /// <returns>The resultant temperature value</returns>
        public static float GetTemperatureValue(float x, float y, float maxX, float maxY, float scale, float offset)
        {
            return GetPerlinNoise(x, y, maxX, maxY, scale, offset + temperatureOffset);
        }
        
        /// <summary>
        /// Gets a perlin noise driven normalised humidity value
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="maxX">The maximum possible value of x</param>
        /// <param name="maxY">The maximum possible value of y</param>
        /// <param name="scale">A scale for the output, applied before the offset value</param>
        /// <param name="offset">An offset value</param>
        /// <returns>The resultant humidityvalue</returns>
        public static float GetHumidityValue(float x, float y, float maxX, float maxY, float scale, float offset)
        {
            return GetPerlinNoise(x, y, maxX, maxY, scale, offset + humidityOffset);
        }

        /// <summary>
        /// Get a normalised vector driven by perlin noise
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="maxX">The maximum possible value of x</param>
        /// <param name="maxY">The maximum possible value of y</param>
        /// <param name="scale">A scale for the output, applied before the offset value</param>
        /// <param name="offset">An offset value</param>
        /// <returns>The resultant vector</returns>
        public static Vector2 GetDirectionalNoise(float x, float y, float maxX, float maxY, float scale, float offset)
        {
            Vector2 direction = new Vector2();
            direction.x = -GetPerlinNoise(x, y, maxX, maxY, scale, offset + directionalOffset);
            direction.x += GetPerlinNoise(x, y, maxX, maxY, scale, offset + directionalOffset * 0.3f);
            
            direction.y = GetPerlinNoise(x, y, maxX, maxY, scale, offset - directionalOffset);
            direction.y -= GetPerlinNoise(x, y, maxX, maxY, scale, offset + directionalOffset * 0.3f);
            return direction.normalized;
        }

        /// <summary>
        /// Get a normalised intensity driven by perlin noise
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="maxX">The maximum possible value of x</param>
        /// <param name="maxY">The maximum possible value of y</param>
        /// <param name="scale">A scale for the output, applied before the offset value</param>
        /// <param name="offset">An offset value</param>
        /// <returns>The resultant intensity value between 0 and 1</returns>
        public static float GetIntensityNoise(float x, float y, float maxX, float maxY, float scale, float offset)
        {
            return GetPerlinNoise(x, y, maxX, maxY, scale, offset + intensityOffset);
        }
    }
}
