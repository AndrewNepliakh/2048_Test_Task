using UnityEngine;

namespace Managers.CubesManager
{
    public class ColorGenerator
    {
        public Color GetRateColor(int rate)
        {
            if (rate <= 0 || (rate & (rate - 1)) != 0)
            {
                Debug.LogWarning($"Rate {rate} is not a power of 2");
                return Color.white;
            }

            var order = (int)Mathf.Log(rate, 2);

            var hue = (order * 0.15f) % 1f;

            var saturation = 0.8f;
            var value = 0.9f;

            return Color.HSVToRGB(hue, saturation, value);
        }
    }
}