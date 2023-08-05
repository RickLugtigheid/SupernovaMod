using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.FlyingTerror.Projectiles
{
	public class TerrorFlame : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.DesertDjinnCurse}";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Terror Spirit");
			Main.projFrames[Projectile.type] = 4;
			//TextureAssets.Projectile[ProjectileID.DesertDjinnCurse].Value
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DesertDjinnCurse);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 310;
		}

		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 30f)
			{
				if (Projectile.ai[0] < 300)
				{
					Projectile.velocity *= 1.025f;
				}

				if (Projectile.ai[0] == 40 && Projectile.ai[1] > 0 && Main.player.Length <= Projectile.ai[1])
				{
					Player target = Main.player[(int)Projectile.ai[1]];
					float rotation = (float)Math.Atan2(Projectile.position.Y - (target.position.Y + target.height * 0.2f), Projectile.position.X - (target.position.X + target.width * 0.15f));
					rotation *= 1 + Main.rand.NextFloat(-.05f, .05f);
				}
			}
			Lighting.AddLight(Projectile.Center, 0.5f, 0.1f, 0.3f);

			if (Projectile.timeLeft > 30 && Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
			}
			if (Projectile.timeLeft > 30 && Projectile.alpha < 128 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			{
				Projectile.alpha = 128;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			int num3 = Projectile.frameCounter + 1;
			Projectile.frameCounter = num3;
			if (num3 > 4)
			{
				Projectile.frameCounter = 0;
				num3 = Projectile.frame + 1;
				Projectile.frame = num3;
				if (num3 >= 4)
				{
					Projectile.frame = 0;
				}
			}
			if (Main.rand.NextBool(3))
			{
				return;
			}	
			Dust dust22 = Main.dust[Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, -2f, 0, default(Color), 1f)];
			//dust22.position = Projectile.Center + Vector2.UnitY.RotatedBy((double)(Projectile.rotation), default(Vector2)) * 10f;
			dust22.noGravity = true;
			//dust22.velocity = Projectile.DirectionFrom(dust22.position);
			dust22.fadeIn = 0.5f;
			dust22.alpha = 200;
		}
	}
}
