using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace SupernovaMod.Content.Projectiles.Melee.Spears
{
    public class ConquerorsBladeStaffProj : SupernovaSpearProjectile
    {
        protected override SpearAiStyle SpearAiStyle => SpearAiStyle.GhastlyGlaiveSpear;

		private bool IsAltUse => Projectile.ai[1] == ItemAlternativeFunctionID.ActivatedAndUsed;

		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 90;
        }
        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.MonkStaffT2);
            Projectile.CloneDefaults(ProjectileID.Spear);

            Projectile.width = 66;
            Projectile.height = 66;
            Projectile.aiStyle = -1;

            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (IsAltUse)
			{
				// Get the player and set this projectile,
				// as the players held projectile
				//
				player.heldProj = Projectile.whoAmI;
				player.direction = Projectile.direction;
				player.itemTime = player.itemAnimation;

				// Set the projectile position,
				// and add the current velocity to the position
				//
				Projectile.position = player.Center - Projectile.Size / 2;

				Vector2 playerRelativePoint = player.RotatedRelativePoint(player.MountedCenter, true, true);
				Projectile.Center = playerRelativePoint;

				if (player.dead)
				{
					Projectile.Kill();
					return;
				}

				if (!player.frozen)
				{
					Projectile.spriteDirection = (Projectile.direction = player.direction);

					/*float inverseAnimationCompletion = 1f - (float)player.itemAnimation / (float)player.itemAnimationMax;
					float originalVelocityDirection = Utils.ToRotation(Projectile.velocity);
					float originalVelocitySpeed = Projectile.velocity.Length();
					Vector2 flatVelocity = Utils.RotatedBy(Vector2.UnitX, (double)(3.1415927f + inverseAnimationCompletion * 6.2831855f), default(Vector2)) * new Vector2(originalVelocitySpeed, Projectile.ai[0]);
					Projectile.position += Utils.RotatedBy(flatVelocity, (double)originalVelocityDirection, default(Vector2)) + Utils.RotatedBy(new Vector2(originalVelocitySpeed + TravelSpeed, 0f), (double)originalVelocityDirection, default(Vector2));
					Vector2 destination = playerRelativePoint + Utils.RotatedBy(flatVelocity, (double)originalVelocityDirection, default(Vector2)) + Utils.ToRotationVector2(originalVelocityDirection) * (originalVelocitySpeed + TravelSpeed + 40f);
					Projectile.rotation = player.AngleTo(destination) + 0.7853982f * (float)player.direction;
					if (Projectile.spriteDirection == -1)
					{
						Projectile.rotation += 3.1415927f;
					}*/

					// TODO: Charge and shoot
					//
					base.Projectile.localAI[0] += 1f;
					if (base.Projectile.localAI[0] >= 15f)
					{
						base.Projectile.localAI[0] = 0f;
						if (Main.myPlayer == base.Projectile.owner)
						{
							float velocityY = base.Projectile.velocity.Y * 1.25f;
							if (velocityY < 0.1f)
							{
								velocityY = 0.1f;
							}
							int proj = Projectile.NewProjectile(base.Projectile.GetSource_FromThis(null), base.Projectile.Center.X + base.Projectile.velocity.X, base.Projectile.Center.Y + base.Projectile.velocity.Y, base.Projectile.velocity.X * 1.25f, velocityY, ModContent.ProjectileType<Projectiles.Hostile.BloodBoltHostile>(), (int)((double)base.Projectile.damage * 0.5), 0f, base.Projectile.owner, 0f, 0f);
							/*if (proj.WithinBounds(1000))
							{
								Main.projectile[proj].DamageType = DamageClass.Melee;
							}*/
						}
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
					}
				}
			}
            else
            {
				base.AI();
            }
        }

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawPosition = base.Projectile.position + new Vector2((float)base.Projectile.width, (float)base.Projectile.height) / 2f + Vector2.UnitY * base.Projectile.gfxOffY - Main.screenPosition;
			Texture2D value = ModContent.Request<Texture2D>(this.Texture, (ReLogic.Content.AssetRequestMode)2).Value;
			Vector2 origin = Utils.Size(value) * 0.5f;
			Main.EntitySpriteDraw(value, drawPosition, default(Rectangle?), lightColor, base.Projectile.rotation, origin, base.Projectile.scale, (SpriteEffects)((Projectile.spriteDirection == -1) ? 1 : 0), 0);

			// DEBUG: this line for a visual representation of the projectile's size.
			//
			if (Supernova.DebugMode)
			{
				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawPosition, Projectile.getRect(), Color.Orange * 0.75f, 0f, origin, Projectile.scale, (SpriteEffects)((Projectile.spriteDirection == -1) ? 1 : 0));
			}
			return false;
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x002B0A2C File Offset: 0x002AEC2C
		/*public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (IsAltUse)
			{
				return false;
			}

			float angle = base.Projectile.rotation - 0.7853982f * (float)Utils.ToInt(base.Projectile.spriteDirection == -1);
			float _ = 0f;
			return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), base.Projectile.Center, base.Projectile.Center + Utils.ToRotationVector2(angle) * -105f, (float)base.Projectile.width, ref _));
		}
*/
		// Token: 0x0600507E RID: 20606 RVA: 0x002B0AB8 File Offset: 0x002AECB8
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[base.Projectile.owner];
			float animationRatio = (float)player.itemAnimation / (float)player.itemAnimationMax;
			DelegateMethods.f_1 = Utils.GetLerpValue(0f, 0.4f, animationRatio, true) * Utils.GetLerpValue(1f, 0.6f, animationRatio, true);
			DelegateMethods.c_1 = Color.White;
			List<Vector2> list = new List<Vector2>();
			list.Add(base.Projectile.position);
			List<Vector2> oldPositions = list;
			foreach (Vector2 oldPosition in base.Projectile.oldPos)
			{
				if (oldPosition != Vector2.Zero)
				{
					oldPositions.Add(oldPosition);
				}
			}
			for (int i = 0; i < oldPositions.Count - 2; i += 2)
			{
				Vector2 offset = base.Projectile.Size * 0.5f + Utils.RotatedBy(base.Projectile.Size, (double)(Utils.ToRotation(base.Projectile.velocity) - 0.7853982f), default(Vector2)) * 0.4f;
				Vector2 start = oldPositions[i] + offset - Main.screenPosition + Vector2.UnitY * base.Projectile.gfxOffY;
				Vector2 end = oldPositions[i + 2] + offset - Main.screenPosition + Vector2.UnitY * base.Projectile.gfxOffY;

				//Texture2D tex = ModContent.Request<Texture2D>(Supernova.GetTexturePath("LightningProj"), (ReLogic.Content.AssetRequestMode)2).Value;
				Texture2D tex = ModContent.Request<Texture2D>("SupernovaMod/Content/Dusts/BloodDust", (ReLogic.Content.AssetRequestMode)2).Value;

				//Utils.DrawLaser(Main.spriteBatch, tex, start, end, new Vector2(.75f), new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
			}
		}
	}
}
