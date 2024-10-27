using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Api.Helpers;
using SupernovaMod.Common.GlobalProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Summon
{
	public class RedDevil : ModProjectile
	{
		private const int FRAME_SPEED = 7;

		public override string Texture => $"Terraria/Images/NPC_{NPCID.RedDevil}";

		private int BuffType => ModContent.BuffType<Buffs.Summon.RedDevilBuff>();

		public override void SetStaticDefaults()
		{
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 5;

			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.

			// Flag this projectile as demonic minion
			SupernovaGlobalProjectile.DemonicMinions.Add(Projectile.type);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.FlyingImp);

			Projectile.width = 94;
			Projectile.height = 58;
			Projectile.minionSlots = 0;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];

			if (!CheckActive(owner))
			{
				return;
			}

			UpdateVisuals();
			SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter, out NPC? target);

			ref float timer = ref Projectile.localAI[0];
			if (foundTarget)
			{
				timer++;
				if (timer > 180)
				{
					// Cooldown
					//
					if (timer > 220)
					{
						timer = 0;
					}
					return;
				}
				// Shoot
				if (timer > 80 && (timer % 20) == 0)
				{
					Shoot(targetCenter);
				}
			}
			else
			{
				timer = 0;
			}
		}

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

		private void UpdateVisuals()
		{
			// Check if our projectile has multiple frames
			//
			if (Main.projFrames[Projectile.type] > 0)
			{
				// This is a simple "loop through all frames from top to bottom" animation
				Projectile.frameCounter++;

				if (Projectile.frameCounter >= FRAME_SPEED)
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

		private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter, out NPC? target)
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

		private void Shoot(Vector2 targetCenter)
		{
			SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
			Vector2 Velocity = Mathf.VelocityFPTP(Projectile.Center, targetCenter, 8f + Main.rand.NextFloat());
			float dist = Vector2.Distance(Projectile.Center, targetCenter);

            targetCenter = ProjectileHelper.CalculateBasicTargetPrediction(Projectile.Center, targetCenter, Velocity);
            Velocity = Mathf.VelocityFPTP(Projectile.Center, targetCenter, 8f + Main.rand.NextFloat());

            int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, Velocity.X, Velocity.Y, ProjectileID.UnholyTridentFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
			Main.projectile[proj].DamageType = DamageClass.Summon;
		}
	}
}
