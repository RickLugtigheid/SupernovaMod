using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Ranged
{
	public class CosmolashGun : ModProjectile
	{
		public override string Texture => "SupernovaMod/Content/Items/Weapons/Ranged/Cosmolash";

		public override void SetDefaults()
		{
			Projectile.width = 76;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.ignoreWater = true;
			Projectile.scale = .85f;
		}

		public override void AI()
		{
			Player player = Main.player[base.Projectile.owner];
			Projectile.ai[0]++;
			ref float incrementAmt = ref Projectile.localAI[1];
			float spreadMult = 0.1f;

			if (Projectile.ai[0] < 360)
			{
				if (Projectile.ai[0] % 30 == 0)
				{
					incrementAmt++;
					Projectile.ai[1] += 0.02f;
					spreadMult = spreadMult + Projectile.ai[1];
				}
			}
			int shootDelayBase = 40;
			int incrementMult = 3;
			base.Projectile.ai[1] -= 1f;
			bool willShoot = false;
			if (base.Projectile.ai[1] <= 0f)
			{
				base.Projectile.ai[1] = (float)(shootDelayBase - incrementMult * incrementAmt);
				willShoot = true;
			}
			bool canShoot = player.channel && player.HasAmmo(player.GetActiveItem()) && !player.noItems && !player.CCed;
			if (base.Projectile.localAI[0] > 0f)
			{
				base.Projectile.localAI[0] -= 1f;
			}
			if (base.Projectile.soundDelay <= 0 && canShoot)
			{
				base.Projectile.soundDelay = shootDelayBase - incrementMult * (int)incrementAmt;
				if (base.Projectile.ai[0] != 1f)
				{
					SoundEngine.PlaySound(SoundID.Item38, new Vector2?(base.Projectile.position), null);
				}
				base.Projectile.localAI[0] = 12f;
			}
			Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true, true);
			if (willShoot && Main.myPlayer == base.Projectile.owner)
			{
				int projType = 14;
				float speedMult = 14f;
				int damage = player.GetWeaponDamage(player.GetActiveItem(), false);
				float kback = player.GetActiveItem().knockBack;
				if (canShoot)
				{
					int num45;
					player.PickAmmo(player.GetActiveItem(), out projType, out speedMult, out damage, out kback, out num45, false);
					kback = player.GetWeaponKnockback(player.GetActiveItem(), kback);
					float speed = player.GetActiveItem().shootSpeed * base.Projectile.scale;
					Vector2 targetPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - source;
					if (player.gravDir == -1f)
					{
						targetPos.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - source.Y;
					}
					Vector2 velMult = Vector2.Normalize(targetPos);
					if (float.IsNaN(velMult.X) || float.IsNaN(velMult.Y))
					{
						velMult = -Vector2.UnitY;
					}
					velMult *= speed;
					if (velMult.X != base.Projectile.velocity.X || velMult.Y != base.Projectile.velocity.Y)
					{
						base.Projectile.netUpdate = true;
					}
					base.Projectile.velocity = velMult * 0.55f;
					int randomBulletCount = 1;
					for (int projIndex = 0; projIndex < randomBulletCount; projIndex++)
					{
						Vector2 bulletVel = Vector2.Normalize(base.Projectile.velocity) * speedMult * (0.6f + Utils.NextFloat(Main.rand) * spreadMult);
						if (float.IsNaN(bulletVel.X) || float.IsNaN(bulletVel.Y))
						{
							bulletVel = -Vector2.UnitY;
						}
						source += Utils.RandomVector2(Main.rand, -5f, 5f);
						bulletVel.X += (float)Main.rand.Next(-3, 4) * spreadMult;
						bulletVel.Y += (float)Main.rand.Next(-3, 4) * spreadMult;
						int num44 = Projectile.NewProjectile(base.Projectile.GetSource_FromThis(null), source, bulletVel, projType, damage, kback, base.Projectile.owner, 0f, 0f, 0f);
						Main.projectile[num44].noDropItem = true;
						Main.projectile[num44].extraUpdates += (int)incrementAmt / 2;
					}
				}
				else
				{
					base.Projectile.Kill();
				}
			}
			base.Projectile.position = player.RotatedRelativePoint(player.MountedCenter, true, true) - base.Projectile.Size / 2f;
			float rotationAmt = 0f;
			if (base.Projectile.spriteDirection == -1)
			{
				rotationAmt = 3.1415927f;
			}
			base.Projectile.rotation = Utils.ToRotation(base.Projectile.velocity) + rotationAmt;
			base.Projectile.spriteDirection = base.Projectile.direction;
			base.Projectile.timeLeft = 2;
			player.ChangeDir(base.Projectile.direction);
			player.heldProj = base.Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2((double)(base.Projectile.velocity.Y * (float)base.Projectile.direction), (double)(base.Projectile.velocity.X * (float)base.Projectile.direction));
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x000C2790 File Offset: 0x000C0990
		public override bool? CanDamage()
		{
			return new bool?(false);
		}
	}
}
