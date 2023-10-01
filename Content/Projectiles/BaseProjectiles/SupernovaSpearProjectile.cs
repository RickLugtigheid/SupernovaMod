using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.BaseProjectiles
{
	public enum SpearAiStyle
	{
		BasicSpear,
		GhastlyGlaiveSpear
	}
	public abstract class SupernovaSpearProjectile : ModProjectile
	{
		protected virtual SpearAiStyle SpearAiStyle { get; set; } = SpearAiStyle.BasicSpear;

		protected virtual float StartSpeed => 3;
		protected virtual float ReelbackSpeed => 1;
		protected virtual float ForwardSpeed => .75f;

		protected virtual float TravelSpeed => 22;

		public override void AI()
		{
			// Check what AI we should use
			//
			switch (SpearAiStyle)
			{
				case SpearAiStyle.GhastlyGlaiveSpear:
					AI_GhastlyGlaiveSpear();
					break;
				default:
					AI_BasicSpear();
					break;
			}
		}

		protected virtual void AI_BasicSpear()
		{
			ref float speed = ref Projectile.ai[0];
			// Get the player and set this projectile,
			// as the players held projectile
			//
			Player player = Main.player[Projectile.owner];
			player.heldProj = Projectile.whoAmI;
			player.direction = Projectile.direction;
			player.itemTime = player.itemAnimation;

			// Set the projectile position,
			// and add the current velocity to the position
			//
			Projectile.position = player.Center - Projectile.Size / 2;
			Projectile.position += Projectile.velocity * speed;

			if (speed == 0)
			{
				speed = StartSpeed;
				Projectile.netUpdate = true;
			}

			// Check if the spear reached it's max extension size,
			// and if so we should reelback the spear.
			//
			if (player.itemAnimation < player.itemAnimationMax / 3)
			{
				speed -= ReelbackSpeed;

				/*if (Projectile.localAI[0] == 0f && this.EffectBeforeReelback != null && Main.myPlayer == Projectile.owner)
				{
					Projectile.localAI[0] = 1f;
					this.EffectBeforeReelback.Invoke(Projectile);
				}*/
			}
			// Else the spear should go forward
			//
			else
			{
				Projectile.ai[0] += ForwardSpeed;
			}

			// If the projectile is not moving, we sould destroy it
			//
			if (player.itemAnimation == 0)
			{
				Projectile.Kill();
			}

			Projectile.rotation = Utils.ToRotation(Projectile.velocity) + 1.5707964f + 0.7853982f;
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= 1.5707964f;
				return;
			}
		}
		protected virtual void AI_GhastlyGlaiveSpear()
		{
			Player player = Main.player[Projectile.owner];
			player.heldProj = Projectile.whoAmI;
			Projectile.direction = player.direction;

			Vector2 playerRelativePoint = player.RotatedRelativePoint(player.MountedCenter, true, true);
			Projectile.Center = playerRelativePoint;

			if (player.dead)
			{
				Projectile.Kill();
				return;
			}

            if (!player.frozen)
            {
				/*if (Main.player[Projectile.owner].itemAnimation < Main.player[Projectile.owner].itemAnimationMax / 3 && Projectile.localAI[0] == 0f && this.EffectBeforeReelback != null && Main.myPlayer == Projectile.owner)
				{
					Projectile.localAI[0] = 1f;
					this.EffectBeforeReelback.Invoke(Projectile);
				}*/

				Projectile.spriteDirection = (Projectile.direction = player.direction);
				if (Projectile.alpha > 0)
				{
					Projectile.alpha -= 127;
					if (Projectile.alpha < 0)
					{
						Projectile.alpha = 0;
					}
				}
				if (Projectile.localAI[0] > 0f)
				{
					Projectile.localAI[0] -= 1f;
				}
				float inverseAnimationCompletion = 1f - (float)player.itemAnimation / (float)player.itemAnimationMax;
				float originalVelocityDirection = Utils.ToRotation(Projectile.velocity);
				float originalVelocitySpeed = Projectile.velocity.Length();
				Vector2 flatVelocity = Utils.RotatedBy(Vector2.UnitX, (double)(3.1415927f + inverseAnimationCompletion * 6.2831855f), default(Vector2)) * new Vector2(originalVelocitySpeed, Projectile.ai[0]);
				Projectile.position += Utils.RotatedBy(flatVelocity, (double)originalVelocityDirection, default(Vector2)) + Utils.RotatedBy(new Vector2(originalVelocitySpeed + TravelSpeed, 0f), (double)originalVelocityDirection, default(Vector2));
				Vector2 destination = playerRelativePoint + Utils.RotatedBy(flatVelocity, (double)originalVelocityDirection, default(Vector2)) + Utils.ToRotationVector2(originalVelocityDirection) * (originalVelocitySpeed + TravelSpeed + 40f);
				Projectile.rotation = player.AngleTo(destination) + 0.7853982f * (float)player.direction;
				if (Projectile.spriteDirection == -1)
				{
					Projectile.rotation += 3.1415927f;
				}
			}
			if (player.itemAnimation == 2)
			{
				Projectile.Kill();
				player.reuseDelay = 2;
			}
		}
	}
}
