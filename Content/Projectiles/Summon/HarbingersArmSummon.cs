using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using SupernovaMod.Api.Helpers;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Projectiles.Summon
{
    public class HarbingersArmSummon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Harbingers Arm");
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;

			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;	// This is necessary for right-click targeting
			Main.projPet[Projectile.type] = true;								// Denotes that this projectile is a pet or minion
		}
		public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 46;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile  = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 24;
			Projectile.scale = .85f;
		}

		//private bool IsVanity { get => Main.item[(int)Projectile.ai[0]].; }
		private bool IsVanity => false; // TODO: vanity projectile
		public override bool? CanDamage() => !IsVanity;

		public override void AI()
        {
            CheckActive();

            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<HarbingersArmSummon>()] > 1)
            {
                Projectile.timeLeft = 0;
            }

			//Factors for calculations
			double deg = 90;
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 60;                   //Distance away from the player

			SearchForTargets(player, out bool foundTarget, out float distFromTarget, out Vector2 targetCenter);

			if (IsVanity || !AttackAI(player, foundTarget, targetCenter, distFromTarget))
			{
				Vector2 targetPosition = new Vector2(
					player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2,
					player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2
				);

				Vector2 distanceFromTarget = new Vector2(targetPosition.X, targetPosition.Y) - Projectile.position;
				SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, 10, .01f);
			}
		}

        public bool AttackAI(Player owner, bool targetFound, Vector2 targetCenter, float distanceFromTarget)
        {
			bool withinDistance = distanceFromTarget <= 500;

			ref float timer = ref Projectile.localAI[0];

			if (targetFound && withinDistance || timer > 80)
			{
				timer++;
			}
			else
			{
				timer = 0;				 // Reset timer
				//Projectile.rotation = 0; // Reset rotation
				Projectile.rotation = Mathf.LerpAngle(Projectile.rotation, 0, .1f);
				_drawMotionBlur = false;

				// Don't look at the target when not on screen
				if (distanceFromTarget > 1000)
				{
					return false;
				}
			}
			if (timer <= 80)
			{
				float damping = .1f;
				Projectile.rotation = Mathf.LerpAngle(Projectile.rotation, Projectile.GetTargetLookRotation(targetCenter), damping);
				return false;
			}
			else if (timer == 81)
			{
				_drawMotionBlur = true;
				SoundEngine.PlaySound(SoundID.Item117, Projectile.Center);
				Vector2 velocity = (targetCenter - Projectile.Center);
				velocity *= 15 / velocity.Magnitude();

				Projectile.velocity = velocity;
			}
			else if (timer >= 110 && ReturnToStartPosition())
			{
				timer = 0;
				_drawMotionBlur = false;
			}
			return true;
		}

		protected virtual void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
		{
			// Starting search distance
			distanceFromTarget = 2000;
			targetCenter = Projectile.position;
			foundTarget = false;

			// When in vanity mode we don't attack
			//
			if (IsVanity)
			{
				return;
			}

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				bool canTargetNpc = !(npc.friendly && npc.CountsAsACritter && npc.dontTakeDamage);
				float between = Vector2.Distance(Projectile.Center, npc.Center);

				// Reasonable distance away so it doesn't target across multiple screens
				if (canTargetNpc && between < 2000)
				{
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}

			if (!foundTarget)
			{
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy())
					{
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
						// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
						bool closeThroughWall = between < 125;

						if ((closest && inRange || !foundTarget) && (lineOfSight || closeThroughWall))
						{
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			Projectile.friendly = foundTarget;
		}

		/// <summary>
		/// Returns this arm projectile to the start position.
		/// </summary>
		/// <returns>If returned to that position</returns>
		private bool ReturnToStartPosition(float maxVelocity = 10)
		{
			// Factors for calculations
			double rad = 90 * (Math.PI / 180);    //Convert degrees to radians 
			double dist = 80;

			// Move to the target position
			//
			Player player = Main.player[Projectile.owner];
			Vector2 targetPosition = new Vector2(
				player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2,
				player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2
			);

			Vector2 distanceFromTarget = new Vector2(targetPosition.X, targetPosition.Y) - Projectile.position;
			SupernovaUtils.MoveProjectileSmooth(Projectile, 100, distanceFromTarget, maxVelocity, .1f);

			// Check if at the target postion
			//
			bool atPosX = Projectile.Center.X <= targetPosition.X + Projectile.width && Projectile.Center.X >= targetPosition.X - Projectile.width;
			bool atPosY = Projectile.Center.Y <= targetPosition.Y + Projectile.height && Projectile.Center.Y >= targetPosition.Y - Projectile.height;
			return atPosX && atPosY;
		}

		public void CheckActive()
        {
            Player player = Main.player[Projectile.owner];

            int buff = ModContent.BuffType<Buffs.Summon.HarbingersArmBuff>();

			if (player.dead)
            {
                player.ClearBuff(buff);
            }
            if (player.HasBuff(buff))
            {
				Projectile.timeLeft = 2;
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			target.AddBuff(BuffID.Frozen, 300);
			base.ModifyHitNPC(target, ref modifiers);
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i <= 7; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UndergroundHallowedEnemies, Projectile.velocity.X, Projectile.velocity.Y, Scale: 1.5f);
			}
			base.OnKill(timeLeft);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			if ((int)Projectile.ai[0] != 0)
			{
				Item item = Main.item[(int)Projectile.ai[0]];
				return item.GetAlpha(lightColor);
			}
			return base.GetAlpha(lightColor);
		}

		private bool _drawMotionBlur = false;
		public override bool PreDraw(ref Color lightColor)
		{
			if (!_drawMotionBlur)
				return true;

			SpriteEffects spriteEffects = 0;
			if (Projectile.spriteDirection == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color24 = Projectile.GetAlpha(lightColor);
			Color color25 = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
			Texture2D texture2D3 = TextureAssets.Projectile[Projectile.type].Value;
			int num156 = TextureAssets.Projectile[Projectile.type].Value.Height / 1;
			int y3 = num156 * (int)Projectile.frameCounter;
			Rectangle rectangle = new Rectangle(0, y3, texture2D3.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			int num157 = 8;
			int num158 = 2;
			int num159 = 1;
			float num160 = 0f;
			int num161 = num159;
			Main.spriteBatch.Draw(texture2D3, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, Projectile.width, Projectile.height), color24, Projectile.rotation, new Vector2(Projectile.width, Projectile.height) / 2f, Projectile.scale, spriteEffects, 0f);
			while (num158 > 0 && num161 < num157 || num158 < 0 && num161 > num157)
			{
				Color color26 = Projectile.GetAlpha(color25);
				float num162 = num157 - num161;
				if (num158 < 0)
				{
					num162 = num159 - num161;
				}
				color26 *= num162 / (ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f);
				Vector2 value4 = Projectile.oldPos[num161];
				float num163 = Projectile.rotation;
				Main.spriteBatch.Draw(texture2D3, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(rectangle), color26, num163 + Projectile.rotation * num160 * (num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, Projectile.scale, spriteEffects, 0f);
				num161 += num158;
			}
			return true;
		}
	}
}
