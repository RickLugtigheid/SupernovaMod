using System;
using Terraria.WorldBuilding;
using Terraria;
using SupernovaMod.Api.Helpers;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Api.DataStructures
{
	public static class CustomShapes
	{
		// Token: 0x020019D7 RID: 6615
		public class DistortedCircle : GenShape
		{
			// Token: 0x0600B593 RID: 46483 RVA: 0x0060E218 File Offset: 0x0060C418
			public DistortedCircle(int radius, float distortionFactor)
			{
				this.baseRadius = radius;
				this.distortionFactor = distortionFactor;
			}

			// Token: 0x0600B594 RID: 46484 RVA: 0x0060E230 File Offset: 0x0060C430
			public override bool Perform(Point origin, GenAction action)
			{
				float offsetAngle = Utils.NextFloat(WorldGen.genRand, -10f, 10f);
				for (float angle = 0f; angle < 6.2831855f; angle += 0.026389377f)
				{
					float distortionQuantity = Mathf.AperiodicSin(angle, offsetAngle, 1.5707964f, 1.3591409f) * this.distortionFactor;
					int currentRadius = (int)((float)this.baseRadius - distortionQuantity * (float)this.baseRadius);
					if (currentRadius > 0)
					{
						int horizontalOffset = (int)(Math.Cos((double)angle) * (double)currentRadius);
						int verticalOffset = (int)(Math.Sin((double)angle) * (double)currentRadius);
						for (int dx = 0; dx != horizontalOffset; dx += Math.Sign(horizontalOffset))
						{
							for (int dy = 0; dy != verticalOffset; dy += Math.Sign(verticalOffset))
							{
								base.UnitApply(action, origin, origin.X + dx, origin.Y + dy, new object[0]);
							}
						}
					}
				}
				return true;
			}

			// Token: 0x04001FD2 RID: 8146
			private readonly int baseRadius;

			// Token: 0x04001FD3 RID: 8147
			private readonly float distortionFactor;
		}
	}
}
