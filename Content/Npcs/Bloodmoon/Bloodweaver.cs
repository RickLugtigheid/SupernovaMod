using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Common;
using SupernovaMod.Common.ItemDropRules.DropConditions;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Bloodmoon
{
	[AutoloadBossHead]
	public class Bloodweaver : ModNPC
	{
		private enum BloodweaverSpell { Shoot, SummonGazers }
		private const int SPRITE_SHEET_HEIGHT = 410;
		private const int SPRITE_SHEET_WIDTH = 370;

		private Player Target
		{
			get
			{
				if (NPC.HasValidTarget)
				{
					return Main.player[NPC.target];
				}
				return null;
			}
		}
		private float CastCooldown { get => NPC.localAI[1]; set => NPC.localAI[1] = value; }
		private float Timer { get => NPC.ai[0]; set => NPC.ai[0] = value; }

		private int[] _gazersOwned = new int[4];
		private BloodweaverSpell _currentSpell;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodweaver");
			Main.npcFrameCount[NPC.type] = 5;

			NPCID.Sets.BossBestiaryPriority.Add(NPC.type);

			NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0);
			npcbestiaryDrawModifiers.PortraitPositionYOverride = new float?((float)-5);
			NPCID.Sets.NPCBestiaryDrawModifiers value = npcbestiaryDrawModifiers;
			value.Position.Y = value.Position.Y + 20f;
			NPCID.Sets.NPCBestiaryDrawOffset[NPC.type] = value;
		}
		public override void SetDefaults()
		{
			NPC.width = 74;
			NPC.height = 82;
			NPC.damage = 24;
			NPC.defense = 7;
			NPC.rarity = 5;
			NPC.lifeMax = 600;
			NPC.HitSound = SoundID.NPCHit21;
			NPC.DeathSound = SoundID.NPCDeath24;
			NPC.value = (float)Item.buyPrice(0, 0, 30, 0);
			NPC.knockBackResist = 0.15f;
			NPC.scale = 1.25f;
			NPC.aiStyle = NPCAIStyleID.HoveringFighter;
			NPC.noGravity = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			//AIType = 3;
		}

		// Draw a health bar even tho NPC.boss == false.
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.25f;
			return default(bool?);
		}

		public override void FindFrame(int frameHeight)
		{
			/*if (this.isCasting || base.NPC.IsABestiaryIconDummy)
			{
				this.counter = 1;
			}
			else
			{
				base.NPC.frameCounter += 1.0;
				if (base.NPC.frameCounter > 8.0)
				{
					this.counter++;
					if (this.counter >= Main.npcFrameCount[NPC.type])
					{
						this.counter = 0;
					}
					base.NPC.frameCounter = 0.0;
				}
			}
			NPC.frame.Y = this.counter * frameHeight;*/

			int width = 74;
			int height = 82;
			NPC.frame.Width = width;
			NPC.frame.Height = height;

			if (isTaunting)
			{
				NPC.frameCounter++;

				// Select the row in our sprite sheet.
				int rowHeight = height * 4;
				NPC.frame.X = width * 2;

				if (NPC.frame.Y == 0)
				{
					SoundEngine.PlaySound(GetRandomTaunt(), NPC.position);
				}

				if (NPC.frameCounter > 15)
				{
					NPC.frameCounter = 0;
					NPC npc = NPC;
					npc.frame.Y = npc.frame.Y + height;

					// Check if the frame exceeds the height of our sprite sheet.
					// If so we reset to 0.
					//
					if (NPC.frame.Y >= rowHeight)
					{
						NPC.frame.Y = 0;
						isTaunting = false; // Stop taunting after the taunt animation
					}
				}
			}
			else if (isCasting)
			{
				NPC.frameCounter++;

				// Select the row in our sprite sheet.
				int rowHeight = height * 4;
				NPC.frame.X = isFocused ? width : (width * 3);

				if (NPC.frameCounter > 7)
				{
					NPC.frameCounter = 0;
					NPC npc = NPC;
					npc.frame.Y = npc.frame.Y + height;

					// Check if the frame exceeds the height of our sprite sheet.
					// If so we reset to 0.
					//
					if (NPC.frame.Y >= rowHeight)
					{
						NPC.frame.Y = 0;
					}
				}
			}
			else
			{
				NPC.frameCounter++;

				// Select the row in our sprite sheet.
				//NPC.frame.X = width;
				int rowHeight = height * 4;
				NPC.frame.X = 0;

				if (NPC.frameCounter > 8)
				{
					NPC.frameCounter = 0;
					NPC npc = NPC;
					npc.frame.Y = npc.frame.Y + height;

					// Check if the frame exceeds the height of our sprite sheet.
					// If so we reset to 0.
					//
					if (NPC.frame.Y >= rowHeight)
					{
						NPC.frame.Y = 0;
					}
				}
			}

			/*if (NPC.velocity.Y != 0f)
			{
				NPC.frame.X = width;
				NPC.frame.Y = height * 4;
			}*/
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneOverworldHeight || !Main.bloodMoon || NPC.AnyNPCs(ModContent.NPCType<Bloodweaver>()))
			{
				return 0f;
			}
			return 0.2f;
		}

		public override void AI()
		{
			// After losing half health set to focus
			//
			isFocused = (NPC.life / (float)NPC.lifeMax) <= .5f;


			if (!NPC.HasValidTarget || isIdle)
			{
				NPC.TargetClosest(false);

				// Check if we have found a valid target we can hit
				//
				if (!NPC.HasValidTarget || !Collision.CanHit(NPC.position + Vector2.UnitY * 6, 15, 15, Target.position, Target.width, Target.height))
				{
					isIdle = true;
				}
				else
				{
					SoundEngine.PlaySound(SoundID.NPCHit21, NPC.Center);
					isIdle = false;
					isTaunting = true;

					// Set the initial cast cooldown so we don't start casting at the start of the agro
					CastCooldown = 120;
				}
			}
			if (isTaunting)
			{
				return; // The taunt animation will play, and then isTaunting will be set to false
			}
			else if (isIdle)
			{
				if (!IdleAI())
				{
					return;
				}
			}
			else if (isCasting)
			{
				UpdateCasting();
				return;
			}
			else
			{
				TargetAI();
			}

			NPC.spriteDirection = -NPC.direction;
		}

		private bool IdleAI()
		{
			return false;
		}

		private void TargetAI()
		{
			// Use walking AI
			NPC.aiStyle = NPCAIStyleID.HoveringFighter;

			// Wait for the cast cooldown to be less than 1 before casting a new spell
			//
			if (CastCooldown > 1)
			{
				CastCooldown--;
				return;
			}

			// TODO: Spawn trail dust

			// Select the spell the NPC should cast
			BloodweaverSpell? spell = SelectSpell();

			// Check if the spell can be cast
			//
			if (spell.HasValue && CanCast(spell.Value))
			{
				// Stand still
				NPC.aiStyle = -1;
				NPC.velocity = Vector2.Zero;

				StartCasting(spell.Value);
			}
		}
		private SoundStyle GetRandomTaunt()
		{
			switch (Main.rand.Next(1))
			{
				case 0:
					return SoundID.Zombie90;
				default:
					return SoundID.Zombie88;
			}
		}
		private BloodweaverSpell? SelectSpell()
		{
			float distance = Vector2.Distance(NPC.position, Target.position);

			if (distance < 1750 && Main.rand.NextBool(2) && GazersActive() < Main.rand.Next(2, 4))
			{
				return BloodweaverSpell.SummonGazers;
			}
			if (distance > 250)
			{
				return BloodweaverSpell.Shoot;
			}
			return null;
		}
		private bool CanCast(BloodweaverSpell spell)
		{
			switch (spell)
			{
				case BloodweaverSpell.Shoot:
					// Check if we can hit our target
					//
					bool canHitTarget = Collision.CanHit(NPC.position + Vector2.UnitY * 6, 15, 15, Target.position, Target.width, Target.height);
					return canHitTarget;
				default:
					return true;
			}
		}
		private void UpdateCasting()
		{
			Timer++;

			switch (_currentSpell)
			{
				case BloodweaverSpell.SummonGazers:
					UpdateSpell_SummonGazers();
					break;
				case BloodweaverSpell.Shoot:
					UpdateSpell_Shoot();
					break;
			}
		}
		private void StartCasting(BloodweaverSpell spellToCast)
		{
			Timer = 0;
			isCasting = true;
			_currentSpell = spellToCast;
		}

		private void UpdateSpell_SummonGazers()
		{
			ref float gazersToSummon = ref NPC.localAI[2];
			ref float rotation = ref NPC.localAI[3];
			if (Timer > 90)
			{
				CastCooldown = 210;
				isCasting = false; // Stop casting
				gazersToSummon = 0;
				rotation = 0;

				// Randomly taunt the player after casting.
				//
				if (!isFocused && Main.rand.NextBool(3))
				{
					isTaunting = true;
					NPC.frameCounter = 0;
					NPC.frame.Y = 0;
				}
			}
			else if (Timer > 60)
			{
				for (int i = 0; i < _gazersOwned.Length; i++)
				{
					// Check if we already summoned the amount we wanted to summon
					//
					if (gazersToSummon < 1)
					{
						return;
					}
					// Check if the gazer saved to position i in the array is still active
					//
					int index = _gazersOwned[i];
					if (Main.netMode != NetmodeID.MultiplayerClient && ( index == 0 || !Main.npc[index].active || Main.npc[index].timeLeft < 1))
					{
						Vector2 summonOffset = Main.rand.NextVector2Circular(140, 40);
						Vector2 summonPosition = new Vector2(NPC.Center.X - summonOffset.X, NPC.Center.Y - (summonOffset.Y + 60));

						for (int d = 0; d < 8; d++)
						{
							Vector2 dustPos = summonPosition + new Vector2(Main.rand.Next(90, 110), 0).RotatedByRandom(MathHelper.ToRadians(360));
							Vector2 diff = summonPosition - dustPos;
							diff.Normalize();

							Dust.NewDustPerfect(dustPos, ModContent.DustType<Dusts.BloodDust>(), diff * 5, Scale: 1.2f).noGravity = true;
						}

						// Replace the gazer with a new gazer
						_gazersOwned[i] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)summonPosition.X, (int)summonPosition.Y, ModContent.NPCType<Gazer>());

						gazersToSummon--;
					}
				}
			}
			else
			{
				// TODO: Play charge animation at the hand position
				//Dust.NewDust(GetHandPosition(), 1, 1, DustID.CrimsonTorch);

				// Spawn gem dust on the player
				//
				Vector2 handPosition = GetHandPosition();
				for (int i = 0; i < 2; i++)
				{
					rotation += MathHelper.ToRadians(67.5f); // 45
					rotation = rotation % MathHelper.ToRadians(360);

					Vector2 dustPos = handPosition + new Vector2(15, 0).RotatedBy(rotation);
					Vector2 diff = handPosition - dustPos;
					diff.Normalize();

					int dustType = DustID.CrimsonTorch;

					Dust.NewDustPerfect(dustPos, dustType, diff, Scale: 1.75f).noGravity = true;
				}
				rotation += MathHelper.ToRadians(1);

				gazersToSummon = isFocused ? Main.rand.Next(2, 3) : Main.rand.Next(1, 2);
			}
		}
		private void UpdateSpell_Shoot()
		{
			ref float rotation = ref NPC.localAI[3];

			if (Timer > 80)
			{
				isCasting = false; // Stop casting
				CastCooldown = 145;
				rotation = 0;

				// Randomly taunt the player after casting.
				//
				if (!isFocused && Main.rand.NextBool(7))
				{
					isTaunting = true;
					NPC.frameCounter = 0;
					NPC.frame.Y = 0;
				}
			}
			else if (Timer > 40)
			{
				if (isFocused)
				{
					if (Timer < 71 && Timer % 10 == 0)
					{
						ShootToPlayer(ModContent.ProjectileType<Projectiles.Hostile.BloodBoltHostile>(), 12, 28, .4f);
					}
				}
				else if (Timer % 15 == 0)
				{
					ShootToPlayer(ModContent.ProjectileType<Projectiles.Hostile.BloodBoltHostile>(), 10, 24, .4f);
				}
			}
			else
			{
				// TODO: Play charge animation at the hand position
				//Dust.NewDust(GetHandPosition(), 1, 1, DustID.DemonTorch);
				//
				//
				Vector2 handPosition = GetHandPosition();
				for (int i = 0; i < 2; i++)
				{
					rotation += MathHelper.ToRadians(67.5f); // 45
					rotation = rotation % MathHelper.ToRadians(360);

					Vector2 dustPos = handPosition + new Vector2(15, 0).RotatedBy(rotation);
					Vector2 diff = handPosition - dustPos;
					diff.Normalize();

					int dustType = DustID.DemonTorch;

					Dust.NewDustPerfect(dustPos, dustType, diff, Scale: 1.75f).noGravity = true;
				}
				rotation += MathHelper.ToRadians(1);
			}
		}

		private int GazersActive()
		{
			int result = 0;
			for (int i = 0; i < _gazersOwned.Length; i++)
			{
				int npc = _gazersOwned[i];

				if (npc > 0 && Main.npc[npc].active && Main.npc[npc].timeLeft > 1)
				{
					result++;
				}
			}
			return result;
		}

		private Vector2 GetHandPosition()
		{
			return NPC.Center - new Vector2(32 * NPC.spriteDirection, 8);
		}

		private void ShootToPlayer(int type, float speed, int damage, float SpreadMult = 0)
		{
			SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, GetHandPosition());
			Vector2 Velocity = Mathf.VelocityFPTP(NPC.Center, new Vector2(Target.Center.X, Target.Center.Y), speed);
			int Spread = 2;
			Velocity.X = Velocity.X + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
			Velocity.Y = Velocity.Y + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
			int i = Projectile.NewProjectile(NPC.GetSource_FromAI(), GetHandPosition(), Velocity, type, damage, 1.75f);
			Main.projectile[i].hostile = true;
			Main.projectile[i].friendly = true;
			Main.projectile[i].tileCollide = true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 6; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CrimsonTorch, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.OneFromOptions(3, new int[]
				{
					ModContent.ItemType<Items.Weapons.Summon.GazerStaff>(),
					ModContent.ItemType<Items.Rings.BloodMagicRing>()
				}
			));
			// Drops after the EoC is downed
			//
			npcLoot.Add(Common.GlobalNPCs.DropRules.GetDropRule<EoCDownedDropCondition>(conditionalRule =>
			{
				conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BoneFragment>(), 3, maximumDropped: 8));
			}));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 position = NPC.position - screenPos;
			SpriteEffects effect = (NPC.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, position, new Rectangle?(NPC.frame), drawColor, 0f, default(Vector2), NPC.scale, effect, 0f);
			return false;
		}

		public int counter;
		public bool isIdle = true;
		public bool isTaunting = false;
		public bool isCasting = false;
		public bool isFocused = false;
	}
}
