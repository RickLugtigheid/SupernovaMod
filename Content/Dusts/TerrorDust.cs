using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Dusts
{
    public class TerrorDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
			//dust.velocity *= 0.3f; // Mulitiply the Velocity on both X and Y by a float (Example 0.6f)
			dust.velocity.Y = (float)Main.rand.Next(-10, 6) * 0.1f;
			dust.velocity.X = dust.velocity.X * 0.3f;
			dust.noGravity = true; // Dust doesn't fall to the ground
            dust.noLight = false; // Dust doesn't emit light
        }

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y = dust.velocity.Y + 0.05f;
			}
			if (!dust.noLight)
			{
				float strength = dust.scale * 1.4f;
				if (strength > 1f)
				{
					strength = 1f;
				}
				Lighting.AddLight(dust.position, 0.1f * strength, 0.025f * strength, 0.025f * strength);
			}
			return true;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color?(new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25));
		}
	}
}