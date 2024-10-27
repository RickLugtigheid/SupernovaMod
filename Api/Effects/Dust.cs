using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace SupernovaMod.Api.Effects
{
	public static class DrawDust
	{
		public static void MakeExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float speed = 1f, int alpha = 0, float scale = 1f, bool noGravity = false, bool noLight = false, bool noLightEmmittance = false) => MakeExplosion(position, spawnRadius, dustType, amount, speed, speed, alpha, alpha, scale, scale, noGravity, noLight, noLightEmmittance);

		public static void MakeExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed, float maxSpeed, int alpha = 0, float scale = 1f, bool noGravity = false, bool noLight = false, bool noLightEmmittance = false) => MakeExplosion(position, spawnRadius, dustType, amount, minSpeed, maxSpeed, alpha, alpha, scale, scale, noGravity, noLight, noLightEmmittance);

		public static void MakeExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed, float maxSpeed, int minAlpha, int maxAlpha, float scale = 1f, bool noGravity = false, bool noLight = false, bool noLightEmmittance = false) => MakeExplosion(position, spawnRadius, dustType, amount, minSpeed, maxSpeed, minAlpha, maxAlpha, scale, scale, noGravity, noLight, noLightEmmittance);

		public static void MakeExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed, float maxSpeed, int minAlpha, int maxAlpha, float minScale, float maxScale, bool noGravity = false, bool noLight = false, bool noLightEmmittance = false)
		{
			for (int i = 0; i < amount; i++)
			{
				Vector2 spawnPosition = position + Main.rand.NextVector2Circular(spawnRadius, spawnRadius);
				Dust newDust = Dust.NewDustPerfect(spawnPosition, dustType);
				newDust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(minSpeed, maxSpeed);
				newDust.alpha = Main.rand.Next(minAlpha, maxAlpha);
				newDust.scale = Main.rand.NextFloat(minScale, maxScale);
				newDust.noGravity = noGravity;
				newDust.noLight = noLight;
				newDust.noLightEmittence = noLightEmmittance;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="point1"></param>
		/// <param name="point2"></param>
		/// <param name="dusttype"></param>
		/// <param name="scale"></param>
		/// <param name="armLength"></param>
		/// <param name="color"></param>
		/// <param name="density"></param>
		public static void Electricity(Vector2 point1, Vector2 point2, int dustType, float scale = 1, int armLength = 30, Color color = default, float density = 0.05f, float randomRotationMod = 1.58f)
		{
			int nodeCount = (int)Vector2.Distance(point1, point2) / armLength;
			Vector2[] nodes = new Vector2[nodeCount + 1];

			nodes[nodeCount] = point2; //adds the end as the last point

			for (int k = 1; k < nodes.Count(); k++)
			{
				//Sets all intermediate nodes to their appropriate randomized dot product positions
				nodes[k] = Vector2.Lerp(point1, point2, k / (float)nodeCount) +
					(k == nodes.Count() - 1 ? Vector2.Zero : Vector2.Normalize(point1 - point2).RotatedBy(randomRotationMod) * Main.rand.NextFloat(-armLength / 2, armLength / 2));

				//Spawns the dust between each node
				Vector2 prevPos = k == 1 ? point1 : nodes[k - 1];
				for (float i = 0; i < 1; i += density)
				{
					Dust d = Dust.NewDustPerfect(Vector2.Lerp(prevPos, nodes[k], i), dustType, Vector2.Zero, 0, color, scale);
					d.noGravity = true;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="size"></param>
		/// <param name="dustType"></param>
		/// <param name="dustCount"></param>
		public static void Ring(Vector2 position, Vector2 velocity, Vector2 size, int dustType = DustID.BlueFlare, int dustCount = 30, float dustScale = 1)
		{
			for (int i = 0; i < dustCount; i++)
			{
				(float sin, float cos) = MathF.SinCos(MathHelper.ToRadians(i * 360 / dustCount));

				float amplitudeX = cos * size.X / 2f;
				float amplitudeY = sin * size.Y;

				Dust dust = Dust.NewDustPerfect(position + new Vector2(amplitudeX, amplitudeY), dustType, -velocity, Scale: dustScale);
				dust.noGravity = true;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="size"></param>
		/// <param name="dustType"></param>
		/// <param name="dustCount"></param>
		public static void RingScaleOutwards(Vector2 position, Vector2 velocity, Vector2 size, int dustType = DustID.BlueFlare, int dustCount = 30, float dustScale = 1)
		{
			for (int i = 0; i < dustCount; i++)
			{
				(float sin, float cos) = MathF.SinCos(MathHelper.ToRadians(i * 360 / dustCount));

				float amplitudeX = cos * size.X / 2f;
				float amplitudeY = sin * size.Y;

				float rot = (new Vector2(amplitudeX, amplitudeY)).ToRotation();
				Dust dust = Dust.NewDustPerfect(position + new Vector2(amplitudeX, amplitudeY), dustType, -velocity.RotatedBy(rot), Scale: dustScale);
				dust.noGravity = true;
			}
		}

		public static void PentacleElectric(Vector2 position, Vector2 velocity, Vector2 size, int dustType, int dustCount = 30, float dustScale = 1)
        {
            Vector2? lastPoint = null;

            for (int i = 0; i < dustCount; i++)
            {
                (float sin, float cos) = MathF.SinCos(MathHelper.ToRadians(i * 360 / dustCount));

                float amplitudeX = cos * size.X;
                float amplitudeY = sin * size.Y;

				Vector2 point = position + new Vector2(amplitudeX, amplitudeY);
                if (lastPoint != null)
                {
                    Electricity(point, lastPoint.Value, dustType, dustScale);
                }
                lastPoint = point;
            }
        }
    }
}
