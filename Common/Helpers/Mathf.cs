using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Supernova.Common
{
	public static class Mathf
	{
        /// <summary>
        /// Calculates random spread
        /// </summary>
        /// <param name="velocity">The velocity for the resulting projectile</param>
        /// <param name="angle">The max spread angle</param>
        /// <param name="num">The number of spreads to calculate</param>
        /// <returns></returns>
        public static Vector2[] RandomSpread(Vector2 velocity, float angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * 0.0344532925);
            float baseSpeed = (float)System.Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            double baseAngle = System.Math.Atan2(velocity.X, velocity.Y);
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

        public static Vector2 VelocityFPTP(Vector2 pos1, Vector2 pos2, float speed)
        {
            Vector2 move = pos2 - pos1;
            //speed2 = speed * 0.5;
            return move * (speed / (float)Math.Sqrt(move.X * move.X + move.Y * move.Y));
        }

        public static float Magnitude(Vector2 mag) => (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
    }
}
