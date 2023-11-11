using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Api.Effects;
using SupernovaMod.Common;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
	public class TerrorInABottle : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.value = BuyPrice.RarityGreen;
			Item.accessory = true;
			Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.GetJumpState<TerrorExtraJump>().Enable();
			// TODO: add ExtraDash class
			player.GetModPlayer<TerrorDashPlayer>().DashAccessoryEquipped = true;
		}

		public class TerrorDashPlayer : ModPlayer
		{
			private const int TERROR_STATE_DAMAGE = 50;
			private const int TERROR_STATE_DURATION = 80;

			// These indicate what direction is what in the timer arrays used
			public const int DashDown = 0;
			public const int DashUp = 1;
			public const int DashRight = 2;
			public const int DashLeft = 3;

			public const int DashCooldown = 42; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
			public const int DashDuration = 34; // Duration of the dash afterimage effect in frames

			// The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
			public const float DashVelocity = 12.5f;//10f;

			// The direction the player has double tapped.  Defaults to -1 for no dash double tap
			public int DashDir = -1;

			// The fields related to the dash accessory
			public bool DashAccessoryEquipped;
			public int DashDelay = 0; // frames remaining till we can dash again
			public int DashTimer = 0; // frames remaining in the dash

			public override void ResetEffects()
			{
				// Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
				DashAccessoryEquipped = false;

				// ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
				// When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
				// If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
				if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
				{
					DashDir = DashDown;
				}
				else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15)
				{
					DashDir = DashUp;
				}
				else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
				{
					DashDir = DashRight;
				}
				else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
				{
					DashDir = DashLeft;
				}
				else
				{
					DashDir = -1;
				}
			}

			// This is the perfect place to apply dash movement, it's after the vanilla movement code, and before the player's position is modified based on velocity.
			// If they double tapped this frame, they'll move fast this frame
			public override void PreUpdateMovement()
			{
				// if the player can use our dash, has double tapped in a direction, and our dash isn't currently on cooldown
				if (CanUseDash() && DashDir != -1 && DashDelay == 0)
				{
					Vector2 newVelocity = Player.velocity;

					switch (DashDir)
					{
						// Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
						case DashUp when Player.velocity.Y > -DashVelocity:
						case DashDown when Player.velocity.Y < DashVelocity:
							return;
							{
								// Y-velocity is set here
								// If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
								// This adjustment is roughly 1.3x the intended dash velocity
								float dashDirection = DashDir == DashDown ? 1 : -1.3f;
								newVelocity.Y = dashDirection * DashVelocity;
								break;
							}
						case DashLeft when Player.velocity.X > -DashVelocity:
						case DashRight when Player.velocity.X < DashVelocity:
							{
								// X-velocity is set here
								float dashDirection = DashDir == DashRight ? 1 : -1;
								newVelocity.X = dashDirection * DashVelocity;
								break;
							}
						default:
							return; // not moving fast enough, so don't start our dash
					}

					// start our dash
					DashDelay = DashCooldown;
					DashTimer = DashDuration;
					Player.velocity = newVelocity;

					// TODO: Create a custom Terror Dash cooldown
					//
					if (Player.HasBuff<Buffs.Cooldowns.TerrorState>())
					{
						if (Player.statLife <= TERROR_STATE_DAMAGE)
						{
							Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " was overwhelmed by fear"), TERROR_STATE_DAMAGE, 0, false, true, -1, false, 100, 100, 0);
						}
						else
						{
							Player.statLife -= TERROR_STATE_DAMAGE;
							SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost);
						}

						// TODO: Add blood paricle effect at the player to indicate loss of health
					}
					Player.AddBuff<Buffs.Cooldowns.TerrorState>(TERROR_STATE_DURATION);

					// Here you'd be able to set an effect that happens when the dash first activates
					// Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
				}

				if (DashDelay > 0)
					DashDelay--;
				
				if (DashTimer > 0)
				{ // dash is active
				  // This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
				  // Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
				  // Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect
					Player.eocDash = DashTimer;
					Player.armorEffectDrawShadowEOCShield = true;

					UpdateDashViusals();

					// count down frames remaining
					DashTimer--;
				}
			}

			private void UpdateDashViusals()
			{
				//set the dust of the trail
				int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 2), Player.width + 2, Player.height + 2, ModContent.DustType<Dusts.TerrorDust>(), Player.velocity.X, Player.velocity.Y, 15, default(Color), 1.35f);
				Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 2), Player.width, Player.height, DustID.Shadowflame, Player.velocity.X * .05f, Player.velocity.Y * .05f, 2, default(Color), 1.2f);
				
				Main.dust[dust].noGravity = true; //this make so the dust has no gravity
				Main.dust[dust].velocity *= 0f;
			}

			private bool CanUseDash()
			{
				return DashAccessoryEquipped
					&& Player.dashType == 0//DashID.None // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
					&& !Player.setSolar // player isn't wearing solar armor
					&& !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
			}
		}
	}


	public class TerrorExtraJump : ExtraJump
	{
		public override Position GetDefaultPosition() => new After(BlizzardInABottle);

		public override float GetDurationMultiplier(Player player)
		{
			return 1f;
		}

		public override void UpdateHorizontalSpeeds(Player player)
		{
			// Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			player.runAcceleration *= 1.25f;
			player.maxRunSpeed *= 2f;
			player.jumpSpeedBoost = 1.5f;
		}

		public override void OnStarted(Player player, ref bool playSound)
		{
			// Spawn rings of fire particles
			int offsetY = player.height;
			if (player.gravDir == -1f)
				offsetY = 0;

			offsetY -= 16;

			/*DrawDust.Ring(
				player.Top + new Vector2(0, offsetY),
				-player.velocity * 0.35f,
				new Vector2((player.width + 2), 5) * 1.5f,
				ModContent.DustType<Dusts.TerrorDust>(),
				50
			);*/

			Vector2 center = player.Top + new Vector2(0, offsetY);
			const int numDusts = 40;
			for (int i = 0; i < numDusts; i++)
			{
				(float sin, float cos) = MathF.SinCos(MathHelper.ToRadians(i * 360 / numDusts));

				float amplitudeX = cos * (player.width + 2) / 2f;
				float amplitudeY = sin * 5;

				Dust dust = Dust.NewDustPerfect(center + new Vector2(amplitudeX, amplitudeY), ModContent.DustType<Dusts.TerrorDust>() /*DustID.BlueFlare*/, -player.velocity * 0.4f, Scale: 1f);
			}
		}

		public override void ShowVisuals(Player player)
		{
			// Use this hook to trigger effects that should appear throughout the duration of the extra jump
			// This example mimics the logic for spawning the dust from the Blizzard in a Bottle
			int offsetY = player.height - 6;
			if (player.gravDir == -1f)
				offsetY = 6;

			/*DrawDust.Ring(
				player.Top + new Vector2(0, offsetY),
				-player.velocity * 0.35f,
				new Vector2((player.width + 2), 5)
			);*/
		}
	}
}
