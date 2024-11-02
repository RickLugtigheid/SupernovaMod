using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class HarpyMothersFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Because of the homing effect
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 44;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
			Projectile.ignoreWater = false;
			Projectile.timeLeft = 36;
			Projectile.scale = .8f;
		}

		public bool IsModeHoming => Projectile.ai[0] == 1;

		public override void AI()
		{
			//this make that the projectile faces the right way
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;

			// Small homig feather
			//
			if (IsModeHoming)
			{
				if (Projectile.ai[1] == 0)
				{
					Projectile.scale = .4f;
					Projectile.timeLeft = 80;
					Projectile.ai[1]++;
				}

				// Homing
				//
				float speed = 12;
				float d = 128f;
				bool targetfound = false;
				Vector2 targetcenter = Projectile.position;
				for (int i = 0; i < 200; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy(null, false))
					{
						float dpt = Vector2.Distance(Projectile.Center, npc.Center);
						if ((dpt < d && !targetfound) || dpt < d)
						{
							d = dpt;
							targetfound = true;
							targetcenter = npc.Center;
						}
					}
				}
				if (targetfound)
				{
					Projectile.velocity = Vector2.Normalize(targetcenter - Projectile.Center) + Projectile.oldVelocity * 0.87f;
					float num = Math.Abs(Projectile.velocity.X);
					float vely = Math.Abs(Projectile.velocity.Y);
					if (num > speed)
					{
						float direction = Math.Abs(Projectile.velocity.X) / Projectile.velocity.X;
						Projectile.velocity.X = speed * direction;
					}
					if (vely > speed)
					{
						float direction2 = Math.Abs(Projectile.velocity.Y) / Projectile.velocity.Y;
						Projectile.velocity.Y = speed * direction2;
						return;
					}
				}
				else
				{
					//Projectile.velocity = new Vector2(speed, 0f).RotatedBy((double)Projectile.rotation, default(Vector2));
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			// Spawn dust
			//
			for (int i = 0; i <= Main.rand.Next(10, 18); i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Harpy, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 20, default, 1);
				dust.noGravity = true;
			}

			// Only trigger for default mode
			//
			if (!IsModeHoming)
			{
				Vector2 velocity = Vector2.Normalize(Projectile.velocity) * 12;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity.RotatedBy(.3), ModContent.ProjectileType<HarpyMothersFeather>(), (int)(Projectile.damage * .8f), Projectile.knockBack, Projectile.owner, 1);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<HarpyMothersFeather>(), (int)(Projectile.damage * .8f), Projectile.knockBack, Projectile.owner, 1);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity.RotatedBy(-.3), ModContent.ProjectileType<HarpyMothersFeather>(), (int)(Projectile.damage * .8f), Projectile.knockBack, Projectile.owner, 1);
				SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}
	}
}
