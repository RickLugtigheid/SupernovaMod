using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.HarbingerOfAnnihilation.Projectiles
{
	public class HoaBlackHole : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Black Hole");
		}

		public override void SetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 6000;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.light = 0.2f;
		}

		public override void AI()
		{
			UpdateEffects();

			// Suck the player in, with configurable distance and power
			Succ(Projectile.ai[0], Projectile.ai[1]);
		}
		protected virtual void UpdateEffects()
		{
			float finalPhaseDustRatio = .5f;

			int dustRings = 3;
			for (int h = 0; h < dustRings; h++)
			{
				float distanceDivisor = (float)h + 1f;
				float dustDistance = Projectile.ai[0] / distanceDivisor;
				int numDust = (int)(0.62831855f * dustDistance);
				float angleIncrement = 6.2831855f / (float)numDust;
				Vector2 dustOffset = new Vector2(dustDistance, 0f);
				dustOffset = Utils.RotatedByRandom(dustOffset, 6.2831854820251465);
				int var = (int)(dustDistance / finalPhaseDustRatio);
				float dustVelocity = 24f / distanceDivisor * finalPhaseDustRatio;
				for (int j = 0; j < numDust; j++)
				{
					if (Utils.NextBool(Main.rand, var))
					{
						dustOffset = Utils.RotatedBy(dustOffset, (double)angleIncrement, default(Vector2));
						int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Demonite, 0f, 0f, 0, default(Color), 1f);
						Main.dust[dust].position = Projectile.Center + dustOffset;
						Main.dust[dust].fadeIn = 1f;
						Main.dust[dust].velocity = Vector2.Normalize(Projectile.Center - Main.dust[dust].position) * dustVelocity;
						Main.dust[dust].scale = 3f - (float)h;
					}
				}
			}
		}

		public virtual void Succ(float maxSuccDistance, float succPower)
		{
			// Check for every player
			//
			for (int k = 0; k < Main.maxPlayers; k++)
			{
				// Get the distance from this black hole to the player
				float distanceFromPlayer = Vector2.Distance(Main.player[k].Center, Projectile.Center);

				// Check if the player can be sucked in.
				// Check if within maxSuccDistance, if not using a grappling hook, and if not behind blocks
				//
				if (distanceFromPlayer < maxSuccDistance && Main.player[k].grappling[0] == -1 && Collision.CanHit(Projectile.Center, 1, 1, Main.player[k].Center, 1, 1))
				{
					float distanceRatio = distanceFromPlayer / maxSuccDistance;
					float multiplier = Utils.Clamp(1f - distanceRatio, .5f, 1);
					if (Main.player[k].Center.X < Projectile.Center.X)
					{
						Player player = Main.player[k];
						player.velocity.X = player.velocity.X + succPower * multiplier;
					}
					else
					{
						Player player2 = Main.player[k];
						player2.velocity.X = player2.velocity.X - succPower * multiplier;
					}
				}
			}
		}
	}
}
