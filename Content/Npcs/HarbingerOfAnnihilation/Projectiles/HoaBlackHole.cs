using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
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

			UpdatePortalEffects();
		}
		/// <summary>
		/// Effects based on <see cref="ProjectileID.VortexVortexPortal"/>
		/// </summary>
		private void UpdatePortalEffects()
		{
			/*int num3;
			for (int num850 = 0; num850 < 25; num850 = num3 + 1)
			{
				int num851 = Utils.SelectRandom<int>(Main.rand, new int[]
				{
						229,
						229,
						161
				});
				Dust dust23 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, num851, 0f, 0f, 0, default(Color), 1f)];
				dust23.noGravity = true;
				dust23.scale = 1.75f + Main.rand.NextFloat() * 1.25f;
				dust23.fadeIn = 0.25f;
				Dust dust2 = dust23;
				dust2.velocity *= 3.5f + Main.rand.NextFloat() * 0.5f;
				dust23.noLight = true;
				num3 = num850;
			}*/
			//Projectile.scale = (Projectile.timeLeft - 50) / 40f;

			Projectile.alpha = 255 - (int)(255f * Projectile.scale);
			Projectile.rotation -= 0.15707964f;
			if (Main.rand.NextBool(2))
			{
				Vector2 vector104 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
				Dust dust29 = Main.dust[Dust.NewDust(Projectile.Center - vector104 * 30f, 0, 0, DustID.Demonite, 0f, 0f, 0, default(Color), .9f)];
				dust29.noGravity = true;
				dust29.position = Projectile.Center - vector104 * (float)Main.rand.Next(10, 21);
				dust29.velocity = vector104.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
				dust29.scale = 0.5f + Main.rand.NextFloat();
				dust29.fadeIn = 0.5f;
				dust29.customData = Projectile.Center;
			}
			if (Main.rand.NextBool(2))
			{
				Vector2 vector105 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
				Dust dust30 = Main.dust[Dust.NewDust(Projectile.Center - vector105 * 30f, 0, 0, DustID.Granite, 0f, 0f, 0, default(Color), 1f)];
				dust30.noGravity = true;
				dust30.position = Projectile.Center - vector105 * 30f;
				dust30.velocity = vector105.RotatedBy(-1.5707963705062866, default(Vector2)) * 3f;
				dust30.scale = 0.5f + Main.rand.NextFloat();
				dust30.fadeIn = 0.5f;
				dust30.customData = Projectile.Center;
			}
			return;
			ref float ptr = ref Projectile.localAI[2];
			ref float ptr61 = ref ptr;
			float num19 = ptr;
			ptr61 = num19 + 1f;
			Main.NewText(ptr);
			if (ptr <= 50f)
			{
				if (Main.rand.NextBool(4))
				{
					Vector2 vector97 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					Dust dust24 = Main.dust[Dust.NewDust(Projectile.Center - vector97 * 30f, 0, 0, DustID.Vortex, 0f, 0f, 0, default(Color), 1f)];
					dust24.noGravity = true;
					dust24.position = Projectile.Center - vector97 * (float)Main.rand.Next(10, 21);
					dust24.velocity = vector97.RotatedBy(1.5707963705062866, default(Vector2)) * 4f;
					dust24.scale = 0.5f + Main.rand.NextFloat();
					dust24.fadeIn = 0.5f;
				}
				if (Main.rand.NextBool(4))
				{
					Vector2 vector98 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					Dust dust25 = Main.dust[Dust.NewDust(Projectile.Center - vector98 * 30f, 0, 0, DustID.Granite, 0f, 0f, 0, default(Color), 1f)];
					dust25.noGravity = true;
					dust25.position = Projectile.Center - vector98 * 30f;
					dust25.velocity = vector98.RotatedBy(-1.5707963705062866, default(Vector2)) * 2f;
					dust25.scale = 0.5f + Main.rand.NextFloat();
					dust25.fadeIn = 0.5f;
				}
			}
			else if (ptr <= 90f)
			{
				if (ptr == 90f)
				{
					//if (flag56)
					{
						SoundEngine.PlaySound(SoundID.Item113, Projectile.position);
					}
					//else
					//{
					//	SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
					//}
				}
				Projectile.scale = (ptr - 50f) / 40f;
				Projectile.alpha = 255 - (int)(255f * Projectile.scale);
				Projectile.rotation -= 0.15707964f;
				if (Main.rand.NextBool(2))
				{
					Vector2 vector104 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					Dust dust29 = Main.dust[Dust.NewDust(Projectile.Center - vector104 * 30f, 0, 0, DustID.Vortex, 0f, 0f, 0, default(Color), 1f)];
					dust29.noGravity = true;
					dust29.position = Projectile.Center - vector104 * (float)Main.rand.Next(10, 21);
					dust29.velocity = vector104.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
					dust29.scale = 0.5f + Main.rand.NextFloat();
					dust29.fadeIn = 0.5f;
					dust29.customData = Projectile.Center;
				}
				if (Main.rand.NextBool(2))
				{
					Vector2 vector105 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					Dust dust30 = Main.dust[Dust.NewDust(Projectile.Center - vector105 * 30f, 0, 0, DustID.Granite, 0f, 0f, 0, default(Color), 1f)];
					dust30.noGravity = true;
					dust30.position = Projectile.Center - vector105 * 30f;
					dust30.velocity = vector105.RotatedBy(-1.5707963705062866, default(Vector2)) * 3f;
					dust30.scale = 0.5f + Main.rand.NextFloat();
					dust30.fadeIn = 0.5f;
					dust30.customData = Projectile.Center;
				}
			}
			else if (ptr <= 120f)
			{
				Projectile.scale = 1f;
				Projectile.alpha = 0;
				Projectile.rotation -= 0.05235988f;
				if (Projectile.type == 813)
				{
					if (Main.rand.Next(2) == 0)
					{
						Vector2 vector111 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
						Dust dust34 = Main.dust[Dust.NewDust(Projectile.Center - vector111 * 30f, 0, 0, 5, 0f, 0f, 0, default(Color), 1f)];
						dust34.noGravity = true;
						dust34.position = Projectile.Center - vector111 * (float)Main.rand.Next(10, 21);
						dust34.velocity = vector111.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
						dust34.scale = 0.5f + Main.rand.NextFloat();
						dust34.fadeIn = 0.5f;
						dust34.customData = Projectile.Center;
					}
					else
					{
						Vector2 vector112 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
						Dust dust35 = Main.dust[Dust.NewDust(Projectile.Center - vector112 * 30f, 0, 0, 240, 0f, 0f, 0, default(Color), 1f)];
						dust35.noGravity = true;
						dust35.position = Projectile.Center - vector112 * 30f;
						dust35.velocity = vector112.RotatedBy(-1.5707963705062866, default(Vector2)) * 3f;
						dust35.scale = 0.5f + Main.rand.NextFloat();
						dust35.fadeIn = 0.5f;
						dust35.customData = Projectile.Center;
					}
				}
				else if (Main.rand.NextBool(2))
				{
					Vector2 vector113 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					Dust dust36 = Main.dust[Dust.NewDust(Projectile.Center - vector113 * 30f, 0, 0, 229, 0f, 0f, 0, default(Color), 1f)];
					dust36.noGravity = true;
					dust36.position = Projectile.Center - vector113 * (float)Main.rand.Next(10, 21);
					dust36.velocity = vector113.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
					dust36.scale = 0.5f + Main.rand.NextFloat();
					dust36.fadeIn = 0.5f;
					dust36.customData = Projectile.Center;
				}
				else
				{
					Vector2 vector114 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					Dust dust37 = Main.dust[Dust.NewDust(Projectile.Center - vector114 * 30f, 0, 0, 240, 0f, 0f, 0, default(Color), 1f)];
					dust37.noGravity = true;
					dust37.position = Projectile.Center - vector114 * 30f;
					dust37.velocity = vector114.RotatedBy(-1.5707963705062866, default(Vector2)) * 3f;
					dust37.scale = 0.5f + Main.rand.NextFloat();
					dust37.fadeIn = 0.5f;
					dust37.customData = Projectile.Center;
				}
			}
			else
			{
				Projectile.scale = 1f - (ptr - 120f) / 60f;
				Projectile.alpha = 255 - (int)(255f * Projectile.scale);
				Projectile.rotation -= 0.10471976f;
				if (Projectile.alpha >= 255)
				{
					Projectile.Kill();
				}
				if (Projectile.type == 813)
				{
					int num3;
					for (int num858 = 0; num858 < 2; num858 = num3 + 1)
					{
						int num859 = Main.rand.Next(3);
						if (num859 == 0)
						{
							Vector2 vector115 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Projectile.scale;
							Dust dust38 = Main.dust[Dust.NewDust(Projectile.Center - vector115 * 30f, 0, 0, 5, 0f, 0f, 0, default(Color), 1f)];
							dust38.noGravity = true;
							dust38.position = Projectile.Center - vector115 * (float)Main.rand.Next(10, 21);
							dust38.velocity = vector115.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
							dust38.scale = 0.5f + Main.rand.NextFloat();
							dust38.fadeIn = 0.5f;
							dust38.customData = Projectile.Center;
						}
						else if (num859 == 1)
						{
							Vector2 vector116 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Projectile.scale;
							Dust dust39 = Main.dust[Dust.NewDust(Projectile.Center - vector116 * 30f, 0, 0, 240, 0f, 0f, 0, default(Color), 1f)];
							dust39.noGravity = true;
							dust39.position = Projectile.Center - vector116 * 30f;
							dust39.velocity = vector116.RotatedBy(-1.5707963705062866, default(Vector2)) * 3f;
							dust39.scale = 0.5f + Main.rand.NextFloat();
							dust39.fadeIn = 0.5f;
							dust39.customData = Projectile.Center;
						}
						num3 = num858;
					}
				}
				else
				{
					int num3;
					for (int num860 = 0; num860 < 2; num860 = num3 + 1)
					{
						int num861 = Main.rand.Next(3);
						if (num861 == 0)
						{
							Vector2 vector117 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Projectile.scale;
							Dust dust40 = Main.dust[Dust.NewDust(Projectile.Center - vector117 * 30f, 0, 0, 229, 0f, 0f, 0, default(Color), 1f)];
							dust40.noGravity = true;
							dust40.position = Projectile.Center - vector117 * (float)Main.rand.Next(10, 21);
							dust40.velocity = vector117.RotatedBy(1.5707963705062866, default(Vector2)) * 6f;
							dust40.scale = 0.5f + Main.rand.NextFloat();
							dust40.fadeIn = 0.5f;
							dust40.customData = Projectile.Center;
						}
						else if (num861 == 1)
						{
							Vector2 vector118 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Projectile.scale;
							Dust dust41 = Main.dust[Dust.NewDust(Projectile.Center - vector118 * 30f, 0, 0, DustID.Granite, 0f, 0f, 0, default(Color), 1f)];
							dust41.noGravity = true;
							dust41.position = Projectile.Center - vector118 * 30f;
							dust41.velocity = vector118.RotatedBy(-1.5707963705062866, default(Vector2)) * 3f;
							dust41.scale = 0.5f + Main.rand.NextFloat();
							dust41.fadeIn = 0.5f;
							dust41.customData = Projectile.Center;
						}
						num3 = num860;
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
				// Get the distance from Projectile black hole to the player
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

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.localAI[0] = Projectile.type;
			Projectile.type = ProjectileID.MoonlordTurret;
			return base.PreDraw(ref lightColor);
		}

		public override void PostDraw(Color lightColor)
		{
			Projectile.type = (int)Projectile.localAI[0];
		}
	}
}
