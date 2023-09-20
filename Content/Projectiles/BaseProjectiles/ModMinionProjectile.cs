using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.BaseProjectiles
{
    /// <credits>
    ///	Created based on the example mod minion code: https://github.com/tModLoader/tModLoader/blob/1.4/ExampleMod/Content/Projectiles/Minions/ExampleSimpleMinion.cs
    /// </credits>
    public abstract class SupernovaMinionProjectile : ModProjectile
    {
        protected int frameSpeed = 5;

        /// <summary>
        /// The buff that the player should have for this minion
        /// </summary>
        protected abstract int BuffType { get; }

        // The AI of this minion is split into multiple methods to avoid bloat. This method just passes values between calls actual parts of the AI.
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!CheckActive(owner))
            {
                return;
            }

            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter, out NPC target);

            UpdateMovement(foundTarget, distanceFromTarget, targetCenter, target, distanceToIdlePosition, vectorToIdlePosition);
            UpdateVisuals();

			Projectile.spriteDirection = Projectile.direction;
		}

		// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		protected virtual bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(BuffType);
            }

            if (owner.HasBuff(BuffType))
            {
                Projectile.timeLeft = 2;
                return true;
            }

            return false;
        }

        protected virtual void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
			Vector2 idlePosition = owner.Center;
            ModifyIdlePosition(ref idlePosition);

            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is projectile.minionPos
            float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
            idlePosition.X += minionPositionOffsetX; // Go behind the player

            // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

            // Teleport to player if distance is too big
            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.04f;

            // Fix overlap with other minions
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];

                if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < other.position.X)
                    {
                        Projectile.velocity.X -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.X += overlapVelocity;
                    }

                    if (Projectile.position.Y < other.position.Y)
                    {
                        Projectile.velocity.Y -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }

        protected virtual void ModifyIdlePosition(ref Vector2 idlePosition)
        {
			idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)
		}

		protected virtual void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter, out NPC target)
        {
            // Starting search distance
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;
            target = null;

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f)
                {
                    distanceFromTarget = between;
					targetCenter = npc.Center;
                    foundTarget = true;
					target = npc;
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
                        bool closeThroughWall = between < 100f;

                        if ((closest && inRange || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
							target = npc;
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

        protected virtual void UpdateMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, NPC target, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            if (foundTarget)
            {
				// Set direction
				//
				if (Projectile.Center.X < targetCenter.X - 2f)
				{
					Projectile.direction = 1;
				}
				if (Projectile.Center.X > targetCenter.X + 2f)
				{
					Projectile.direction = -1;
				}
				UpdateAttackMovement(foundTarget, distanceFromTarget, targetCenter, target, distanceToIdlePosition, vectorToIdlePosition);
            }
            else
            {
                UpdateIdleMovement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
				// Set direction
				//
				if (Projectile.Center.X < vectorToIdlePosition.X - 2f)
				{
					Projectile.direction = -1;
				}
				if (Projectile.Center.X > vectorToIdlePosition.X + 2f)
				{
					Projectile.direction = 1;
				}
			}
        }

        // Default movement parameters (here for attacking)
        protected float speedMax = 8;
		protected float speed = 8f;
        protected float attackSpeedMuli = 1.5f;
		protected float inertia = 10;
		protected virtual void UpdateAttackMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, NPC target, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            speed = speedMax;
			// Minion has a target: attack (here, fly towards the enemy)
            //
			if (distanceFromTarget > target.width)
			{
                float speedMulti = distanceFromTarget / 100;
                speedMulti = Utils.Clamp(speedMulti, .4f, 1); // Cap the multiplier
				SupernovaUtils.MoveProjectileSmooth(Projectile, 100, targetCenter - Projectile.Center, speed * speedMulti, .15f);
			}

			Vector2 vectorToTarget = targetCenter - Projectile.Center;
			vectorToTarget = Utils.SafeNormalize(vectorToTarget, Vector2.Zero) * (speed * attackSpeedMuli);
			
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, vectorToTarget, 0.024390243f);

            // Cap the velocity at the max speed
            //
            if (Projectile.velocity.X > speedMax)
            {
				Projectile.velocity.X = speedMax;
			}
			else if (Projectile.velocity.X < -speedMax)
			{
				Projectile.velocity.X = -speedMax;
			}
			if (Projectile.velocity.Y > speedMax)
			{
				Projectile.velocity.Y = speedMax;
			}
			if (Projectile.velocity.Y < -speedMax)
			{
				Projectile.velocity.Y = -speedMax;
			}
		}
        protected virtual void UpdateIdleMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
			// Minion doesn't have a target: return to player and idle
			if (distanceToIdlePosition > 600f)
			{
				// Speed up the minion if it's away from the player
				speed = 12f;
				inertia = 60f;
			}
			else
			{
				// Slow down the minion if closer to the player
				speed = 4f;
				inertia = 80f;
			}

			if (distanceToIdlePosition > 20f)
			{
				// The immediate range around the player (when it passively floats about)

				// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
				vectorToIdlePosition.Normalize();
				vectorToIdlePosition *= speed;
				Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
			}
			else if (Projectile.velocity == Vector2.Zero)
			{
				// If there is a case where it's not moving at all, give it a little "poke"
				Projectile.velocity.X = -0.15f;
				Projectile.velocity.Y = -0.05f;
			}
		}

        protected virtual void UpdateVisuals()
        {
            // So it will lean slightly towards the direction it's moving
            Projectile.rotation = Projectile.velocity.X * 0.05f;

            // Check if our projectile has multiple frames
            //
            if (Main.projFrames[Projectile.type] > 0)
            {
                // This is a simple "loop through all frames from top to bottom" animation
                Projectile.frameCounter++;

                if (Projectile.frameCounter >= frameSpeed)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;

                    if (Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        Projectile.frame = 0;
                    }
                }
            }
        }
    }
}