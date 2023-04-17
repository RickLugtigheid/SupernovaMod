using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Hostile
{
	public class LightningHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning");
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 8;
			Projectile.hostile = true;
			Projectile.hide = true;
			Projectile.timeLeft = 10;
			Projectile.tileCollide = false;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(4))
			{
				target.AddBuff(BuffID.Electrified, Main.rand.Next(1, 3) * 60, true);
			}
		}
	}
}
