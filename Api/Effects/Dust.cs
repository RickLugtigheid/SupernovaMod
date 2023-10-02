using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace SupernovaMod.Api.Effects
{
	public static class DrawDust
	{
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
		public static void Electricity(Vector2 point1, Vector2 point2, int dustType, float scale = 1, int armLength = 30, Color color = default, float density = 0.05f)
		{
			int nodeCount = (int)Vector2.Distance(point1, point2) / armLength;
			Vector2[] nodes = new Vector2[nodeCount + 1];

			nodes[nodeCount] = point2; //adds the end as the last point

			for (int k = 1; k < nodes.Count(); k++)
			{
				//Sets all intermediate nodes to their appropriate randomized dot product positions
				nodes[k] = Vector2.Lerp(point1, point2, k / (float)nodeCount) +
					(k == nodes.Count() - 1 ? Vector2.Zero : Vector2.Normalize(point1 - point2).RotatedBy(1.58f) * Main.rand.NextFloat(-armLength / 2, armLength / 2));

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
		public static void Ring(Vector2 position, Vector2 velocity, Vector2 size, int dustType = DustID.BlueFlare, int dustCount = 30)
		{
			for (int i = 0; i < dustCount; i++)
			{
				(float sin, float cos) = MathF.SinCos(MathHelper.ToRadians(i * 360 / dustCount));

				float amplitudeX = cos * size.X / 2f;
				float amplitudeY = sin * size.Y; // 5

				Dust dust = Dust.NewDustPerfect(position + new Vector2(amplitudeX, amplitudeY), dustType, -velocity, Scale: 1f);
				dust.noGravity = true;
			}
		}
	}
}
