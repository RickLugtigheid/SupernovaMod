using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using static Terraria.ModLoader.PlayerDrawLayer;
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
									Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromAI(), npc.position, Vector2.Zero, ProjectileID.Meteor1, item.damage, 6);
									proj.timeLeft = 4;
									proj.Resize(12, 12);
								}
								else
								{
									Player.ApplyDamageToNPC(npc, (int)num, num2, num3, crit, item.DamageType);
								}
							}
							Player.eocDash = 10;
							dashDelay = DashDelayMax;
							Player.velocity.X = (-(float)num3 * 5); // Knockback Left/Right
							Player.velocity.Y = -11; // Knockback Up/Down
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
					DashTimeMax = 40;
					DashDelayMax = 80;
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

	public class OldDashPlayer : ModPlayer
	{
		public DamageClass damageClass = DamageClass.Generic;
		public SupernovaDashType dashType = SupernovaDashType.Meteor;
		protected int dashTime;
		protected int dashDelay;

		public override void PreUpdate()
		{
			damageClass = DamageClass.Generic;
			//dashType = SupernovaDashType.None;
		}

		public void UpdateDash(bool hideVisual = false)
		{

		}

		public void UpdateDashAccessory(bool hideVisual = false)
		{
			// Unset vanilla dash type.
			Player.dash = 0;

			//
			//
			if (dashType == SupernovaDashType.Meteor && Player.eocDash > 0)
			{
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
								float num = 30f * Player.GetDamage(damageClass).Base;
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
								if (Main.rand.Next(100) < Player.GetCritChance(damageClass))
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
									Player.ApplyDamageToNPC(npc, (int)num, num2, num3, crit);
								}
								Player.eocDash = 10;
								dashDelay = 30;
								Player.velocity.X = (float)(-(float)num3 * 9);
								Player.velocity.Y = -4f;
								Player.GiveImmuneTimeForCollisionAttack(4);
								Player.eocHit = i;
							}
						}
					}
				}
				else if ((!Player.controlLeft || Player.velocity.X >= 0f) && (!Player.controlRight || Player.velocity.X <= 0f))
				{
					Player.velocity.X = Player.velocity.X * 0.95f;
				}
			}

			if (dashDelay > 0)
			{
				if (Player.eocDash > 0)
				{
					Player.eocDash--;
				}
				if (Player.eocDash == 0)
				{
					Player.eocHit = -1;
				}
				dashDelay--;
				return;
			}
			if (dashDelay < 0)
			{
				Player.StopVanityActions(true);
				float num7 = 12f;
				float num8 = 0.992f;
				float num9 = Math.Max(Player.accRunSpeed, Player.maxRunSpeed);
				float num10 = 0.96f;
				int num11 = 20;
				if (dashType == SupernovaDashType.Meteor)
				{
					for (int l = 0; l < 0; l++)
					{
						int num13;
						if (Player.velocity.Y == 0f)
						{
							num13 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)Player.height - 4f), Player.width, 8, 31, 0f, 0f, 100, default(Color), 1.4f);
						}
						else
						{
							num13 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2) - 8f), Player.width, 16, 31, 0f, 0f, 100, default(Color), 1.4f);
						}
						Main.dust[num13].velocity *= 0.1f;
						Main.dust[num13].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
					}
					num8 = 0.985f;
					num10 = 0.94f;
					num11 = 30;
				}
				//if (Player.dash > 0)
				{
					Player.doorHelper.AllowOpeningDoorsByVelocityAloneForATime(num11 * 3);
					Player.vortexStealthActive = false;
					if (Player.velocity.X > num7 || Player.velocity.X < -num7)
					{
						Player.velocity.X = Player.velocity.X * num8;
						return;
					}
					if (Player.velocity.X > num9 || Player.velocity.X < -num9)
					{
						Player.velocity.X = Player.velocity.X * num10;
						return;
					}
					dashDelay = num11;
					if (Player.velocity.X < 0f)
					{
						Player.velocity.X = -num9;
						return;
					}
					if (Player.velocity.X > 0f)
					{
						Player.velocity.X = num9;
						return;
					}
				}
			}
			else if (dashType > 0 && !Player.mount.Active)
			{
				if (dashType == SupernovaDashType.Meteor)
				{
					//int num22;
					//bool flag2;
					//Player.DoCommonDashHandle(out num22, out flag2, null);
					/*if (flag2)
					{
						Player.velocity.X = 14.5f * (float)num22;
						Point point3 = (Player.Center + new Vector2((float)(num22 * Player.width / 2 + 2), Player.gravDir * (float)(-(float)Player.height) / 2f + Player.gravDir * 2f)).ToTileCoordinates();
						Point point4 = (Player.Center + new Vector2((float)(num22 * Player.width / 2 + 2), 0f)).ToTileCoordinates();
						if (WorldGen.SolidOrSlopedTile(point3.X, point3.Y) || WorldGen.SolidOrSlopedTile(point4.X, point4.Y))
						{
							Player.velocity.X = Player.velocity.X / 2f;
						}
						dashDelay = -1;
						Player.eocDash = 15;
						for (int num23 = 0; num23 < 0; num23++)
						{
							int num24 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default(Color), 2f);
							Dust dust3 = Main.dust[num24];
							dust3.position.X = dust3.position.X + (float)Main.rand.Next(-5, 6);
							Dust dust4 = Main.dust[num24];
							dust4.position.Y = dust4.position.Y + (float)Main.rand.Next(-5, 6);
							Main.dust[num24].velocity *= 0.2f;
							Main.dust[num24].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
						}
					}*/
				}
			}
		}

		/*
		public void DashMovement()
		{
			if (Player.dashDelay == 0)
			{
				Player.dash = Player.dashType;
			}
			if (Player.dash == 0)
			{
				Player.dashTime = 0;
				Player.dashDelay = 0;
			}
			if (Player.dash == 2 && Player.eocDash > 0)
			{
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
								float num = 30f * Player.meleeDamage;
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
								if (Main.rand.Next(100) < Player.meleeCrit)
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
									Player.ApplyDamageToNPC(npc, (int)num, num2, num3, crit);
								}
								Player.eocDash = 10;
								Player.dashDelay = 30;
								Player.velocity.X = (float)(-(float)num3 * 9);
								Player.velocity.Y = -4f;
								Player.GiveImmuneTimeForCollisionAttack(4);
								Player.eocHit = i;
							}
						}
					}
				}
				else if ((!Player.controlLeft || Player.velocity.X >= 0f) && (!Player.controlRight || Player.velocity.X <= 0f))
				{
					Player.velocity.X = Player.velocity.X * 0.95f;
				}
			}
			if (Player.dash == 3 && Player.dashDelay < 0 && Player.whoAmI == Main.myPlayer)
			{
				Rectangle rectangle2 = new Rectangle((int)((double)Player.position.X + (double)Player.velocity.X * 0.5 - 4.0), (int)((double)Player.position.Y + (double)Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
				for (int j = 0; j < 200; j++)
				{
					NPC npc2 = Main.npc[j];
					if (npc2.active && !npc2.dontTakeDamage && !npc2.friendly && npc2.immune[Player.whoAmI] <= 0 && (npc2.aiStyle != 112 || npc2.ai[2] <= 1f) && Player.CanNPCBeHitByPlayerOrPlayerProjectile(npc2, null))
					{
						Rectangle rect2 = npc2.getRect();
						if (rectangle2.Intersects(rect2) && (npc2.noTileCollide || Player.CanHit(npc2)))
						{
							if (!Player.solarDashConsumedFlare)
							{
								Player.solarDashConsumedFlare = true;
								Player.ConsumeSolarFlare();
							}
							float num4 = 150f * Player.meleeDamage;
							float num5 = 9f;
							bool crit2 = false;
							if (Player.kbGlove)
							{
								num5 *= 2f;
							}
							if (Player.kbBuff)
							{
								num5 *= 1.5f;
							}
							if (Main.rand.Next(100) < Player.meleeCrit)
							{
								crit2 = true;
							}
							int direction = Player.direction;
							if (Player.velocity.X < 0f)
							{
								direction = -1;
							}
							if (Player.velocity.X > 0f)
							{
								direction = 1;
							}
							if (Player.whoAmI == Main.myPlayer)
							{
								Player.ApplyDamageToNPC(npc2, (int)num4, num5, direction, crit2);
								int num6 = Projectile.NewProjectile(Player.GetProjectileSource_OnHit(npc2, 2), Player.Center.X, Player.Center.Y, 0f, 0f, 608, (int)num4, 15f, Main.myPlayer, 0f, 0f, 0f);
								Main.projectile[num6].Kill();
							}
							npc2.immune[Player.whoAmI] = 6;
							Player.GiveImmuneTimeForCollisionAttack(4);
						}
					}
				}
			}
			if (Player.dashDelay > 0)
			{
				if (Player.eocDash > 0)
				{
					Player.eocDash--;
				}
				if (Player.eocDash == 0)
				{
					Player.eocHit = -1;
				}
				Player.dashDelay--;
				return;
			}
			if (Player.dashDelay < 0)
			{
				Player.StopVanityActions(true);
				float num7 = 12f;
				float num8 = 0.992f;
				float num9 = Math.Max(Player.accRunSpeed, Player.maxRunSpeed);
				float num10 = 0.96f;
				int num11 = 20;
				if (Player.dash == 1)
				{
					for (int k = 0; k < 2; k++)
					{
						int num12;
						if (Player.velocity.Y == 0f)
						{
							num12 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)Player.height - 4f), Player.width, 8, 31, 0f, 0f, 100, default(Color), 1.4f);
						}
						else
						{
							num12 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2) - 8f), Player.width, 16, 31, 0f, 0f, 100, default(Color), 1.4f);
						}
						Main.dust[num12].velocity *= 0.1f;
						Main.dust[num12].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
					}
				}
				else if (Player.dash == 2)
				{
					for (int l = 0; l < 0; l++)
					{
						int num13;
						if (Player.velocity.Y == 0f)
						{
							num13 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)Player.height - 4f), Player.width, 8, 31, 0f, 0f, 100, default(Color), 1.4f);
						}
						else
						{
							num13 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2) - 8f), Player.width, 16, 31, 0f, 0f, 100, default(Color), 1.4f);
						}
						Main.dust[num13].velocity *= 0.1f;
						Main.dust[num13].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
					}
					num8 = 0.985f;
					num10 = 0.94f;
					num11 = 30;
				}
				else if (Player.dash == 3)
				{
					for (int m = 0; m < 4; m++)
					{
						int num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 6, 0f, 0f, 100, default(Color), 1.7f);
						Main.dust[num14].velocity *= 0.1f;
						Main.dust[num14].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
						Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
						Main.dust[num14].noGravity = true;
						if (Main.rand.Next(2) == 0)
						{
							Main.dust[num14].fadeIn = 0.5f;
						}
					}
					num7 = 14f;
					num8 = 0.985f;
					num10 = 0.94f;
					num11 = 20;
				}
				else if (Player.dash == 4)
				{
					for (int n = 0; n < 2; n++)
					{
						int num15 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 229, 0f, 0f, 100, default(Color), 1.2f);
						Main.dust[num15].velocity *= 0.1f;
						Main.dust[num15].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
						Main.dust[num15].noGravity = true;
						if (Main.rand.Next(2) == 0)
						{
							Main.dust[num15].fadeIn = 0.3f;
						}
					}
					num8 = 0.985f;
					num10 = 0.94f;
					num11 = 20;
				}
				if (Player.dash == 5)
				{
					for (int num16 = 0; num16 < 2; num16++)
					{
						int type = (int)Main.rand.NextFromList(new short[]
						{
					68,
					69,
					70
						});
						int num17;
						if (Player.velocity.Y == 0f)
						{
							num17 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)Player.height - 4f), Player.width, 8, type, 0f, 0f, 100, default(Color), 1f);
						}
						else
						{
							num17 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)(Player.height / 2) - 8f), Player.width, 16, type, 0f, 0f, 100, default(Color), 1f);
						}
						Main.dust[num17].velocity *= 0.2f;
						Main.dust[num17].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
						Main.dust[num17].fadeIn = 0.5f + (float)Main.rand.Next(20) * 0.01f;
						Main.dust[num17].noGravity = true;
						Main.dust[num17].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
					}
				}
				if (Player.dash > 0)
				{
					Player.doorHelper.AllowOpeningDoorsByVelocityAloneForATime(num11 * 3);
					Player.vortexStealthActive = false;
					if (Player.velocity.X > num7 || Player.velocity.X < -num7)
					{
						Player.velocity.X = Player.velocity.X * num8;
						return;
					}
					if (Player.velocity.X > num9 || Player.velocity.X < -num9)
					{
						Player.velocity.X = Player.velocity.X * num10;
						return;
					}
					Player.dashDelay = num11;
					if (Player.velocity.X < 0f)
					{
						Player.velocity.X = -num9;
						return;
					}
					if (Player.velocity.X > 0f)
					{
						Player.velocity.X = num9;
						return;
					}
				}
			}
			else if (Player.dash > 0 && !Player.mount.Active)
			{
				if (Player.dash == 1)
				{
					int num18;
					bool flag;
					Player.DoCommonDashHandle(out num18, out flag, null);
					if (flag)
					{
						Player.velocity.X = 16.9f * (float)num18;
						Point point = (Player.Center + new Vector2((float)(num18 * Player.width / 2 + 2), Player.gravDir * (float)(-(float)Player.height) / 2f + Player.gravDir * 2f)).ToTileCoordinates();
						Point point2 = (Player.Center + new Vector2((float)(num18 * Player.width / 2 + 2), 0f)).ToTileCoordinates();
						if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
						{
							Player.velocity.X = Player.velocity.X / 2f;
						}
						Player.dashDelay = -1;
						for (int num19 = 0; num19 < 20; num19++)
						{
							int num20 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default(Color), 2f);
							Dust dust = Main.dust[num20];
							dust.position.X = dust.position.X + (float)Main.rand.Next(-5, 6);
							Dust dust2 = Main.dust[num20];
							dust2.position.Y = dust2.position.Y + (float)Main.rand.Next(-5, 6);
							Main.dust[num20].velocity *= 0.2f;
							Main.dust[num20].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
						}
						int num21 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 34f), default(Vector2), Main.rand.Next(61, 64), 1f);
						Main.gore[num21].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
						Main.gore[num21].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
						Main.gore[num21].velocity *= 0.4f;
						num21 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 14f), default(Vector2), Main.rand.Next(61, 64), 1f);
						Main.gore[num21].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
						Main.gore[num21].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
						Main.gore[num21].velocity *= 0.4f;
					}
				}
				else if (Player.dash == 2)
				{
					int num22;
					bool flag2;
					Player.DoCommonDashHandle(out num22, out flag2, null);
					if (flag2)
					{
						Player.velocity.X = 14.5f * (float)num22;
						Point point3 = (Player.Center + new Vector2((float)(num22 * Player.width / 2 + 2), Player.gravDir * (float)(-(float)Player.height) / 2f + Player.gravDir * 2f)).ToTileCoordinates();
						Point point4 = (Player.Center + new Vector2((float)(num22 * Player.width / 2 + 2), 0f)).ToTileCoordinates();
						if (WorldGen.SolidOrSlopedTile(point3.X, point3.Y) || WorldGen.SolidOrSlopedTile(point4.X, point4.Y))
						{
							Player.velocity.X = Player.velocity.X / 2f;
						}
						Player.dashDelay = -1;
						Player.eocDash = 15;
						for (int num23 = 0; num23 < 0; num23++)
						{
							int num24 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default(Color), 2f);
							Dust dust3 = Main.dust[num24];
							dust3.position.X = dust3.position.X + (float)Main.rand.Next(-5, 6);
							Dust dust4 = Main.dust[num24];
							dust4.position.Y = dust4.position.Y + (float)Main.rand.Next(-5, 6);
							Main.dust[num24].velocity *= 0.2f;
							Main.dust[num24].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
						}
					}
				}
				else if (Player.dash == 3)
				{
					int num25;
					bool flag3;
					Player.DoCommonDashHandle(out num25, out flag3, new Player.DashStartAction(Player.SolarDashStart));
					if (flag3)
					{
						Player.velocity.X = 21.9f * (float)num25;
						Point point5 = (Player.Center + new Vector2((float)(num25 * Player.width / 2 + 2), Player.gravDir * (float)(-(float)Player.height) / 2f + Player.gravDir * 2f)).ToTileCoordinates();
						Point point6 = (Player.Center + new Vector2((float)(num25 * Player.width / 2 + 2), 0f)).ToTileCoordinates();
						if (WorldGen.SolidOrSlopedTile(point5.X, point5.Y) || WorldGen.SolidOrSlopedTile(point6.X, point6.Y))
						{
							Player.velocity.X = Player.velocity.X / 2f;
						}
						Player.dashDelay = -1;
						for (int num26 = 0; num26 < 20; num26++)
						{
							int num27 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 6, 0f, 0f, 100, default(Color), 2f);
							Dust dust5 = Main.dust[num27];
							dust5.position.X = dust5.position.X + (float)Main.rand.Next(-5, 6);
							Dust dust6 = Main.dust[num27];
							dust6.position.Y = dust6.position.Y + (float)Main.rand.Next(-5, 6);
							Main.dust[num27].velocity *= 0.2f;
							Main.dust[num27].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
							Main.dust[num27].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
							Main.dust[num27].noGravity = true;
							Main.dust[num27].fadeIn = 0.5f;
						}
					}
				}
				if (Player.dash == 5)
				{
					int num28;
					bool flag4;
					Player.DoCommonDashHandle(out num28, out flag4, null);
					if (flag4)
					{
						Player.velocity.X = 16.9f * (float)num28;
						Point point7 = (Player.Center + new Vector2((float)(num28 * Player.width / 2 + 2), Player.gravDir * (float)(-(float)Player.height) / 2f + Player.gravDir * 2f)).ToTileCoordinates();
						Point point8 = (Player.Center + new Vector2((float)(num28 * Player.width / 2 + 2), 0f)).ToTileCoordinates();
						if (WorldGen.SolidOrSlopedTile(point7.X, point7.Y) || WorldGen.SolidOrSlopedTile(point8.X, point8.Y))
						{
							Player.velocity.X = Player.velocity.X / 2f;
						}
						Player.dashDelay = -1;
						for (int num29 = 0; num29 < 20; num29++)
						{
							int type2 = (int)Main.rand.NextFromList(new short[]
							{
								68,
								69,
								70
							});
							int num30 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, type2, 0f, 0f, 100, default(Color), 1.5f);
							Dust dust7 = Main.dust[num30];
							dust7.position.X = dust7.position.X + (float)Main.rand.Next(-5, 6);
							Dust dust8 = Main.dust[num30];
							dust8.position.Y = dust8.position.Y + (float)Main.rand.Next(-5, 6);
							Main.dust[num30].velocity = Player.DirectionTo(Main.dust[num30].position) * 2f;
							Main.dust[num30].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
							Main.dust[num30].fadeIn = 0.5f + (float)Main.rand.Next(20) * 0.01f;
							Main.dust[num30].noGravity = true;
							Main.dust[num30].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
						}
					}
				}
			}
		}
		*/
	}
}
