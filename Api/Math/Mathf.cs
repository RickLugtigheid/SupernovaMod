using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SupernovaMod.Api.Helpers
{
    /// <summary>
    /// A class containing eXtra math methods.
    /// </summary>
    public static class Mathf
	{
		/// <summary>
		/// The circle constant, the circumference of the unit circle in radians.
		/// </summary>
		// 6.2831855f and 6.28318530717959
		public const double Tau = (double)6.2831853071795864769252867666M;

		/// <summary>
		/// Returns the inverse of the absolute value for <paramref name="x"/>.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static float InvAbs(float x) => -MathF.Abs(x);
		/// <summary>
		/// Clamps the <paramref name="value"/> between 0 and 1.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static float Clamp01(float value) => MathHelper.Clamp(value, 0, 1);
		/// <summary>
		/// Loops the value t, so that it is never larger than length and never smaller than 0.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static float Repeat(float t, float length) => MathHelper.Clamp(t - MathF.Floor(t / length * length), 0.0f, length);

		/// <summary>
		/// Determines where a value lies between two points.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="value"></param>
		/// <returns>A value between zero and one, representing where the "value" parameter falls within the range defined by a and b.</returns>
		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
			{
				return (value - a) / (a - b);
			}
			return 0;
		}

		/// <summary>
		/// Same as Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.
		/// </summary>
		/// <param name="a">The start angle. A float expressed in degrees.</param>
		/// <param name="b">The end angle. A float expressed in degrees.</param>
		/// <param name="t">The interpolation value between the start and end angles. This value is clamped to the range [0, 1].</param>
		/// <returns>Returns the interpolated float result between angle <paramref name="a"/> and angle <paramref name="b"/>, based on the interpolation value <paramref name="t"/>.</returns>
		public static float LerpAngleDeg(float a, float b, float t)
		{
			float delta = Repeat((b - a), 360);
			if (delta > 180)
			{
				delta -= 360;
			}
			return a + delta * Clamp01(t);
		}
		/// <summary>
		/// Same as Lerp but makes sure the values interpolate correctly when they wrap around <see cref="Tau"/>.
		/// </summary>
		/// <param name="a">The start angle. A float expressed in radians.</param>
		/// <param name="b">The end angle. A float expressed in radians.</param>
		/// <param name="t"></param>
		/// <returns>Returns the interpolated float result between angle <paramref name="a"/> and angle <paramref name="b"/>, based on the interpolation value <paramref name="t"/> in radians.</returns>
		public static float LerpAngle(float a, float b, float t)
		{
			float difference = (float)((b - a) % Mathf.Tau);
			float distance = (float)(((2 * difference) % Mathf.Tau) - difference);
			return a + distance * t;
		}

		#region Vector Math
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static float Magnitude(this Vector2 value) => MathF.Sqrt(value.X * value.X + value.Y * value.Y);

		/// <summary>
		/// Calculates the velocity From Postion <paramref name="pos1"/> To Position <paramref name="pos2"/> with the given <paramref name="speed"/>.
		/// </summary>
		/// <param name="pos1"></param>
		/// <param name="pos2"></param>
		/// <param name="speed"></param>
		/// <returns></returns>
		public static Vector2 VelocityFPTP(Vector2 pos1, Vector2 pos2, float speed)
        {
            Vector2 move = pos2 - pos1;
            return move * (speed / Magnitude(move));
        }

		/// <summary>
		/// Calculates random spread
		/// </summary>
		/// <param name="velocity">The velocity for the resulting projectile</param>
		/// <param name="angle">The max spread angle</param>
		/// <param name="num">The number of spreads to calculate</param>
		/// <returns></returns>
		[Obsolete("This is an old method and should be improved, and / or renamed")]
		public static Vector2[] RandomSpread(Vector2 velocity, float angle, int num)
		{
			var posArray = new Vector2[num];
			float spread = (float)(angle * 0.0344532925);
			float baseSpeed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
			double baseAngle = Math.Atan2(velocity.X, velocity.Y);
			double randomAngle;

			// Calculate the given number of spreads
			//
			for (int i = 0; i < num; ++i)
			{
				// Calculate a random angle
				randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				// Calculate the projecile position that should be used to follow this angle
				posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
			}
			return posArray;
		}
		#endregion
	}
}
