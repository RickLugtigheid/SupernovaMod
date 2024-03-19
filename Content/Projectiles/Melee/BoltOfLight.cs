using Microsoft.Xna.Framework;
using SupernovaMod.Api.Effects;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Melee
{
	public class BoltOfLight : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NebulaArcanumExplosionShot}";

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.NebulaArcanumExplosionShot);
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.penetrate = 5;
			Projectile.ArmorPenetration = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 24;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 12;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			//if (Projectile.localAI[0] % 8 == 0)
			{
				//DrawDust.Electricity(_startPosition.Value, Projectile.position, DustID.Electric, .65f, 60, default, .6f);
				DrawDust.RingScaleOutwards(Projectile.Center, Vector2.One * 3, Vector2.One * 5, DustID.AncientLight, dustScale: .8f);
			}

			int num3;
			int num240 = (int)Projectile.ai[0];
			for (int num241 = 0; num241 < 3; num241 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.AncientLight, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 1.2f);
				Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center) / 2;
				Main.dust[num242].noGravity = true;
				Dust dust2 = Main.dust[num242];
				dust2.velocity *= 0.5f;
				num3 = num241;
			}
			for (int num243 = 0; num243 < 2; num243 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.AncientLight, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 0.4f);
				if (num243 == 0)
				{
					Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center * 5f) / 6f;
				}
				else if (num243 == 1)
				{
					Main.dust[num242].position = (Main.dust[num242].position + (Projectile.Center + Projectile.velocity / 2f) * 5f) / 6f;
				}
				Dust dust2 = Main.dust[num242];
				dust2.velocity *= 0.1f;
				Main.dust[num242].noGravity = true;
				Main.dust[num242].fadeIn = 1f;
				num3 = num243;
			}
		}
	}
}
