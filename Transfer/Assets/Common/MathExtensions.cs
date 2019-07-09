using UnityEngine;

namespace Common
{
    /// <summary>
    /// Contains general math functions to extend the default <see cref="Mathf"/> ones
    /// </summary>
    static class MathExtensions
    {
        /// <summary>
        /// Clamp the magnitude of a value, regardless of its sign.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum magnitude of the value. This should be positive</param>
        /// <param name="max">The maximum magnitude of the value. This should be positive</param>
        /// <returns>Magnitude clamped value</returns>
        public static float ClampMagnitude(float value, float min, float max)
        {
            return Mathf.Clamp(Mathf.Abs(value), min, max) * Mathf.Sign(value);
        }

        /// <summary>
        /// Interpolate between colours using HSV rather than RGB.
        /// This interpolates in a nicer way than a raw RGB lerp
        /// </summary>
        /// <param name="from">Colour to interpolate from</param>
        /// <param name="to">Colour to interpolate to</param>
        /// <param name="f">The interpolation factor between 0 and 1</param>
        /// <returns>The HSV interpolated colour</returns>
        public static Color LerpHSV(Color from, Color to, float f)
        {
            Color.RGBToHSV(from, out float fromH, out float fromS, out float fromV);
            Color.RGBToHSV(to, out float toH, out float toS, out float toV);

            return Color.HSVToRGB
            (
                Mathf.Lerp(fromH, toH, f),
                Mathf.Lerp(fromS, toS, f),
                Mathf.Lerp(fromV, toV, f)
            );
        }

        /// <summary>
        /// Gets the interpolation factor that <see cref="value"/> is between <see cref="from"/> and <see cref="too"/>
        /// and passes it through a smoothstep function.
        /// </summary>
        /// <param name="from">Lower bound</param>
        /// <param name="to">Upper bound</param>
        /// <param name="value">The value between from and to</param>
        /// <returns>Smoothstepped interpolation factor</returns>
        public static float InverseLerpSmoothstep(float from, float to, float value)
        {
            return Mathf.SmoothStep(0, 1, Mathf.InverseLerp(from, to, value));
        }
    }
}
