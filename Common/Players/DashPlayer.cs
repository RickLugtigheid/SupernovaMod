using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.Audio;

namespace SupernovaMod.Common.Players
{
	public enum SupernovaDashType
	{
		None,
		Meteor
	}

	public class DashPlayer : ModPlayer
	{
		protected const byte DashDirDown = 0;
		protected const byte DashDirUp = 1;

		/// <summary>
		/// The time in ticks a dash should last.
		/// </summary>
		public virtual int DashTimeMax { get; protected set; }
		/// <summary>
		/// The delay in ticks it should take before you may dash again.
		/// </summary>
		public virtual int DashDelayMax { get; protected set; }
		/// <summary>
		/// 
		/// </summary>
		public virtual int DashSpeed { get; protected set; }
		public bool DashActive { get; protected set; } = false;

		public SupernovaDashType dashType = SupernovaDashType.None;
		public int dashTime;
		public int dashDelay;

		private int dashDirection = -1;

		public void UpdateDashAccessory(Item item, bool hideVisual = false)
		{
			// Don't update when not dashing
			//
			if (!DashActive || dashType == SupernovaDashType.None)
			{
				return;
			}

			// This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
			// Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
			// Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect
			Player.eocDash = dashTime;
			Player.armorEffectDrawShadowEOCShield = true;

			// If the dash has just started, apply the dash velocity in whatever direction we wanted to dash towards
			//
			if (dashTime == DashTimeMax)
			{
				Vector2 newVelocity = Player.velocity;

				//Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                if ((dashDirection == DashDirDown && Player.velocity.Y < DashSpeed))
				{
					// Y-velocity is set here
					// If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
					// This adjustment is roughly 1.3x the intended dash velocity
					//
					float direction = dashDirection == DashDirDown ? 1.5f : -1.3f;
					newVelocity.Y = direction * DashSpeed;
				}
				Player.velocity = newVelocity;
			}

			if (dashTime > 0)
			{
				if (dashType == SupernovaDashType.Meteor)
				{
					HandleDashCollision(item, out bool hit);

					if (Player.eocHit < 0)
					{
						Dust dust54 = Main.dust[Dust.NewDust(Player.Center, Player.width, Player.height, 6, 0f, 0f, 0, default(Color), 1f)];
						dust54.position = Player.Center;
						dust54.velocity = Player.velocity.RotatedBy(1.5707963705062866, default(Vector2)) * 0.33f + Player.velocity / 4f;
						dust54.position += Player.velocity.RotatedBy(1.5707963705062866, default(Vector2));
						dust54.fadeIn = 0.5f;
						dust54.noGravity = true;
						Dust dust55 = Main.dust[Dust.NewDust(Player.Center, Player.width, Player.height, 6, 0f, 0f, 0, default(Color), 1f)];
						dust55.position = Player.Center;
						dust55.velocity = Player.velocity.RotatedBy(-1.5707963705062866, default(Vector2)) * 0.33f + Player.velocity / 4f;
						dust55.position += Player.velocity.RotatedBy(-1.5707963705062866, default(Vector2));
						dust55.fadeIn = 0.5f;
						dust55.noGravity = true;
						for (int num208 = 0; num208 < 1; num208++)
						{
							int num209 = Dust.NewDust(new Vector2(Player.Center.X, Player.Center.Y), Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default(Color), 1f);
							Main.dust[num209].velocity *= 0.5f;
							Main.dust[num209].scale *= 1.3f;
							Main.dust[num209].fadeIn = 1f;
							Main.dust[num209].noGravity = true;
						}
					}
				}
			}

			//Decrement the timers
			dashTime--;
			dashDelay--;

			if (dashDelay <= 0)
            {
				//The dash has ended.  Reset the fields
				dashDelay = DashDelayMax;
				dashTime = DashTimeMax;
                DashActive = false;
            }
		}
		protected void HandleDashCollision(Item item, out bool hit)
		{
			hit = false;
			if (Player.eocHit < 0)
			{
				Rectangle rectangle = new Rectangle((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
				for (int i = 0; i < 200; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active && !npc.dontTakeDamage && !npc.friendly && (npc.aiStyle != 112 || npc.ai[2] <= 1f) && Player.CanNPCBeHitByPlayerOrPlayerProjectile(npc, null))
					{
						Rectangle rect = npc.getRect();
						if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit(npc)))
						{
							float num = 30f;
							float num2 = 9f;
							bool crit = false;
							if (Player.kbGlove)
							{
								num2 *= 2f;
							}
							if (Player.kbBuff)
							{
								num2 *= 1.5f;
							}
							if (Main.rand.NextFloat(0, 1) < Player.GetCritChance(item.DamageType))
							{
								crit = true;
							}
							int num3 = Player.direction;
							if (Player.velocity.X < 0f)
							{
								num3 = -1;
							}
							if (Player.velocity.X > 0f)
							{
								num3 = 1;
							}
							if (Player.whoAmI == Main.myPlayer)
							{
								if (dashType == SupernovaDashType.Meteor)
								{
									Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromAI(), npc.position, Vector2.Zero, ProjectileID.Meteor1, item.damage, 6, Player.whoAmI, .5f, .5f, .5f);
									proj.timeLeft = 4;
								}
								else
								{
									Player.ApplyDamageToNPC(npc, (int)num, num2, num3, crit, item.DamageType);
								}
							}
							Player.eocDash = 10;
							dashDelay = DashDelayMax;
							Player.velocity.X = (-(float)num3 * 5); // Knockback Left/Right
							Player.velocity.Y = -14; // Knockback Up/Down
							Player.GiveImmuneTimeForCollisionAttack(4);
							Player.eocHit = i;
							hit = true;
						}
					}
				}
			}
		}


		protected virtual void SetDefaults()
		{
			switch (dashType)
			{
				case SupernovaDashType.Meteor:
					DashTimeMax = 38;
					DashDelayMax = 58;
					DashSpeed = 64;
					Player.eocHit = -1;
					break;
			}
		}
		/// <summary>
		/// A method that runs when a dash starts.
		/// <para>Here you'd be able to set an effect that happens when the dash first activates</para>
		/// </summary>
		protected virtual void OnDashStart()
		{
			// Here you'd be able to set an effect that happens when the dash first activates
			// Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi

			switch (dashType)
			{
				case SupernovaDashType.Meteor:
					SoundEngine.PlaySound(SoundID.Item88, Player.Center);
					break;
			}
		}

		/*public override void PreUpdate()
		{
			// Reset
			dashType = SupernovaDashType.None;
		}*/
		public override void ResetEffects()
		{
			// ResetEffects() is called not long after player.doubleTapCardinalTimer's values have been set


			// If we don't have the ExampleDashAccessory equipped or the player has the Solor armor set equipped, return immediately
			// Also return if the player is currently on a mount, since dashes on a mount look weird, or if the dash was already activated
			if (dashType == SupernovaDashType.None || Player.setSolar || Player.mount.Active || DashActive)
			{
				return;
			}

			// TODO: Make for any direction
			//
			// When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
			// If the timers are set to 15, then this is the first press just processed by the vanilla logic. Otherwise, it's a double-tap
			if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDirDown] < 15)
			{
				dashDirection = DashDirDown;
			}	
			else
			{
				return;  //No dash was activated, return
			}

			SetDefaults();
			dashDelay = DashDelayMax;
			dashTime = DashTimeMax;
			DashActive = true;

			OnDashStart();
		}
	}
}
