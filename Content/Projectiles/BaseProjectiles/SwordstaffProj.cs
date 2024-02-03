using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.BaseProjectiles
{
	public abstract class SwordstaffProj : ModProjectile
	{
		/// <summary>
		/// The time it takes to complete a swing cycle (in ticks)
		/// </summary>
		public float SwingCycleTime { get; protected set; } = 50;

		public override void SetStaticDefaults()
		{
			// If a Jellyfish is zapping and we attack it with this projectile, it will deal damage to us.
			// This set has the projectiles for the Night's Edge, Excalibur, Terra Blade (close range), and The Horseman's Blade (close range).
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 100;		//Set the hitbox width
			Projectile.height = 100;    //Set the hitbox height
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 90000;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			//Projectile.alpha = 255;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 12;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
		}

		public override void AI()
		{
			UpdateSwingSound();
			SwingAI();
			UpdateEffects();
			ExtraAI(ref Projectile.ai[0]);
		}
		private void UpdateSwingSound()
		{
			Projectile.soundDelay--;
			if (Projectile.soundDelay <= 0)//this is the proper sound delay for this type of weapon
			{
				SoundEngine.PlaySound(SoundID.DD2_SkyDragonsFurySwing, Projectile.Center);    //this is the sound when the weapon is used
				Projectile.soundDelay = 45;    //this is the proper sound delay for this type of weapon
			}
		}
		private void SwingAI()
		{
			Player player = Main.player[base.Projectile.owner];
			float spinCycleTime = SwingCycleTime;
			if (player.dead || !player.channel)
			{
				Projectile.Kill();
				player.reuseDelay = 2;
				return;
			}

			int direction = Math.Sign(Projectile.velocity.X);
			Projectile.velocity = new Vector2((float)direction, 0f);
			if (Projectile.ai[0] == 0f)
			{
				Projectile.rotation = Utils.ToRotation(new Vector2((float)direction, -player.gravDir)) + MathHelper.ToRadians(135f);
				if (Projectile.velocity.X < 0f)
				{
					Projectile.rotation -= 1.5707964f;
				}
			}
			Projectile.ai[0] += 1f;
			Projectile.rotation += 12.566371f / spinCycleTime * (float)direction;
			int expectedDirection = Utils.ToDirectionInt(SafeDirectionTo(player, Main.MouseWorld, default(Vector2?)).X > 0f);
			if (Projectile.ai[0] % spinCycleTime > spinCycleTime * 0.5f && (float)expectedDirection != Projectile.velocity.X)
			{
				player.ChangeDir(expectedDirection);
				Projectile.velocity = Vector2.UnitX * (float)expectedDirection;
				Projectile.rotation -= 3.1415927f;
				Projectile.netUpdate = true;
			}

			Vector2 plrCtr = player.RotatedRelativePoint(player.MountedCenter, true, true);
			Vector2 offset = Vector2.Zero;
			Projectile.Center = plrCtr + offset;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.timeLeft = 2;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.itemTime = (player.itemAnimation = 2);
			player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
		}
		/// <summary>
		/// Update any dust, light, and other effects
		/// </summary>
		protected virtual void UpdateEffects()
		{

		}
		/// <summary>
		/// Add extra AI. (example: used for shooting projectiles)
		/// </summary>
		protected virtual void ExtraAI(ref float swingCycleTime)
		{
		}

		/*public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			// Calculate the hit direction (from front or behind).
			//
			Vector2 direction = (Projectile.Center - target.position) * Projectile.spriteDirection;
			if (direction.X < 0)
			{
				hitDirection = 1;
			}
			else
			{
				hitDirection = -1;
			}

			base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}*/

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projHitbox.Intersects(targetHitbox))
			{
				return new bool?(true);
			}
			/*float f = Projectile.rotation - 0.7853982f * (float)Math.Sign(Projectile.velocity.X);
			float num2 = 0f;
			float num3 = Projectile.height / 2;//float num3 = 110f;
			if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center + Utils.ToRotationVector2(f) * -num3, Projectile.Center + Utils.ToRotationVector2(f) * num3, 23f * Projectile.scale, ref num2))
			{
				return new bool?(true);
			}*/
			return new bool?(false);
		}

		/*public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.AsyncLoad).Value;
			Vector2 drawPos = base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY);
			Rectangle rectangle = new (0, 0, tex.Width, tex.Height);
			Vector2 origin = Utils.Size(tex) / 2f;
			SpriteEffects spriteEffects = 0;
			if (Projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Main.EntitySpriteDraw(tex, drawPos, new Rectangle?(rectangle), lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}*/
		public override bool PreDraw(ref Color lightColor)  //this make the projectile sprite rotate perfectaly around the player 
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 position = Projectile.Center - Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.spriteBatch.Draw(texture, position, null, Color.White, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0f);

			// DEBUG: this line for a visual representation of the projectile's size.
			//
			if (Supernova.DebugMode)
			{
				Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, position, texture.Bounds, Color.Orange * 0.75f, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0f);
			}

			return false;
		}

		private Vector2 SafeDirectionTo(Entity entity, Vector2 destination, Vector2? fallback = null)
		{
			if (fallback == null)
			{
				fallback = new Vector2?(Vector2.Zero);
			}
			return Utils.SafeNormalize(destination - entity.Center, fallback.Value);
		}
	}
}
