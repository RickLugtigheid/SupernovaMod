using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
	public class EldrichTentacle : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NebulaArcanumExplosionShot}";

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ShadowFlame);
			Projectile.penetrate = 2;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			Vector2 center13 = Projectile.Center;
			Projectile.scale = 1f - Projectile.localAI[0];
			Projectile.width = (int)(20f * Projectile.scale);
			Projectile.height = Projectile.width;
			Projectile.position.X = center13.X - (float)(Projectile.width / 2);
			Projectile.position.Y = center13.Y - (float)(Projectile.height / 2);
			ref float ptr = ref Projectile.localAI[0];
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = (Projectile.velocity.RotatedByRandom(.1) * Main.rand.Next(-1, 1)).ToRotation();
			}
			if ((double)Projectile.localAI[0] < 0.1)
			{
				ptr = ref Projectile.localAI[0];
				ptr += 0.01f;
			}
			else
			{
				ptr = ref Projectile.localAI[0];
				ptr += 0.025f;
            }
            if (Projectile.localAI[0] >= 0.95f)
			{
				Projectile.Kill();
			}
			ptr = ref Projectile.velocity.X;
			ptr += Projectile.ai[0] * 1.5f;
			ptr = ref Projectile.velocity.Y;
			ptr += Projectile.ai[1] * 1.5f;
			if (Projectile.velocity.Length() > 16f)
			{
				Projectile.velocity.Normalize();
                Projectile.velocity *= 16f;
			}
			ptr = ref Projectile.ai[0];
			ptr *= 1.05f;
			ptr = ref Projectile.ai[1];
			ptr *= 1.05f;
			if (Projectile.scale < 1f)
			{
				Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[2] / 100);
				Projectile.ai[2] *= 1.065f;


                int num791 = 0;
				while ((float)num791 < Projectile.scale * 10f)
				{
					int num792 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CorruptionThorns, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), .5f);
					Main.dust[num792].position = (Main.dust[num792].position + Projectile.Center) / 2f;
					Main.dust[num792].noGravity = true;
					Dust dust2 = Main.dust[num792];
					dust2.velocity *= 0.1f;
					dust2 = Main.dust[num792];
					dust2.velocity -= Projectile.velocity * (1.3f - Projectile.scale);
					Main.dust[num792].fadeIn = (float)(100 + Projectile.owner);
					dust2 = Main.dust[num792];
					dust2.scale += Projectile.scale * 0.75f;
					int num3 = num791;
					num791 = num3 + 1;
				}
				return;
			}
		}
	}
}
