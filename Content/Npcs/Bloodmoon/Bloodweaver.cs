using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Api.Helpers;
using SupernovaMod.Common;
using SupernovaMod.Common.ItemDropRules.DropConditions;
using SupernovaMod.Common.Systems;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Bloodmoon
{
	[AutoloadBossHead]
	public class Bloodweaver : ModNPC
	{
		private enum BloodweaverSpell { Shoot, SummonGazers, Dash }
		//private const int SPRITE_SHEET_HEIGHT = 410;
		//private const int SPRITE_SHEET_WIDTH = 370;

		private const float ProjectileExpertDamageMultiplier = .62f;

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
			Main.npcFrameCount[NPC.type] = 5;

			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;		
			NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
			NPCID.Sets.TeleportationImmune[NPC.type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(NPC.type);
			NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers();
			npcbestiaryDrawModifiers.PortraitPositionYOverride = new float?((float)-5);
			npcbestiaryDrawModifiers.Scale = .75f;
		}
		public override void SetDefaults()
		{
			NPC.width = 74;
			NPC.height = 82;
			NPC.damage = 18;
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
			NPC.noTileCollide = true;
			//AIType = 3;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of NPC NPC that is listed in the bestiary.
				//BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon,

				// Sets the description of NPC that is listed in the bestiary.
				//new FlavorTextBestiaryInfoElement(""),
			});
		}

		// Draw a health bar even tho NPC.boss == false.
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.25f;
			return default(bool?);
		}

		public override void FindFrame(int frameHeight)
		{
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

				if (NPC.frame.Y == 0 || NPC.frameCounter == 15)
				{
					SoundEngine.PlaySound(SoundID.NPCHit48, NPC.position);
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
			else if (isDashing)
			{
				NPC.frameCounter++;

				// Select the row in our sprite sheet.
				int rowHeight = height * 2;
				NPC.frame.X = width * 4;

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
			if (!spawnInfo.Player.ZoneOverworldHeight || !Main.bloodMoon || NPC.AnyNPCs(ModContent.NPCType<Bloodweaver>()))
			{
				return 0f;
			}
			return 0.0475f;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((byte)_currentSpell);
			writer.Write(isIdle);
			writer.Write(isTaunting);
			writer.Write(isCasting);
			writer.Write(isDashing);
			writer.Write(isFocused);
			writer.Write(wasFocused);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			_currentSpell = (BloodweaverSpell)reader.ReadByte();
			isIdle = reader.ReadBoolean();
			isTaunting = reader.ReadBoolean();
			isCasting = reader.ReadBoolean();
			isDashing = reader.ReadBoolean();
			isFocused = reader.ReadBoolean();
			wasFocused = reader.ReadBoolean();
		}

		public override void AI()
		{
			if (DespawnAI())
			{
				return; // Don't run any other AI
			}

			// After losing half health set to focus
			//
			isFocused = (NPC.life / (float)NPC.lifeMax) <= .5f;

			//
			NPC.netUpdate = true;

			// When changing from not focused to focused,
			// we should reset the AI to idle.
			//
			if (isFocused && !wasFocused)
			{
				SoundEngine.PlaySound(SoundID.Zombie6, NPC.position);

				// Reset flags
				//
				isIdle = false;
				isCasting = false;
				isDashing = false;
				isTaunting = false;
				wasFocused = true;
			}

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
			else if (isDashing)
			{
				Timer++;
				Update_Dash();
				return;
			}
			else
			{
				TargetAI();
			}

			NPC.spriteDirection = -NPC.direction;
		}

		private bool DespawnAI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}

			Player player = Main.player[NPC.target];
			if (NPC.ai[0] != 3f && (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 2250))
			{
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
				if (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 2500)
				{
					if (NPC.timeLeft > 130)
					{
						NPC.timeLeft = 130;
					}
					NPC.ai[0] = 0;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.netUpdate = true;
					return true;
				}
			}
			else if (NPC.timeLeft < 2000)
			{
				NPC.timeLeft = 2000;
			}
			return false;
		}

		private bool IdleAI()
		{
			return false;
		}

		private void TargetAI()
		{
			// Use HoveringFighter AI
			NPC.aiStyle = NPCAIStyleID.HoveringFighter;

			// If we are lower than the player we should add some Y velocity
			//
			if ((Target.Center.Y - (Target.height / 2)) < (NPC.Center.Y + (NPC.height / 2)))
			{
				NPC.velocity.Y -= .09f;
			}

			// Wait for the cast cooldown to be less than 1 before casting a new spell
			//
			if (CastCooldown > 1)
			{
				CastCooldown--;
				return;
			}

			// Only run the SelectSpell code for the server or singleplayer client
			//
			/*if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}*/

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

			// Sync TargetAI changes
			//
			/*if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
			}*/
		}
		private BloodweaverSpell? SelectSpell()
		{
			float distance = Vector2.Distance(NPC.position, Target.position);

			if (distance < 2000 && Main.rand.NextBool(2) && GazersActive() < Main.rand.Next(2, 4))
			{
				return BloodweaverSpell.SummonGazers;
			}
			if (
				distance < 300 && Main.rand.NextBool()
				// Check if the player is not below or above the NPC
				&& (Target.Center.Y < (NPC.Center.Y + NPC.height)) && (Target.Center.Y > (NPC.Center.Y - NPC.height))
			)
			{
				return BloodweaverSpell.Dash;
			}
			if (distance > 100)
			{
				return BloodweaverSpell.Shoot;
			}
			return null;
		}
		private bool CanCast(BloodweaverSpell spell)
		{
			// If the distance from the target is to high
			// we should not cast a spell.
			//
			if (MathHelper.Distance(NPC.position.X, Target.position.X) > 3000)
			{
				return false;
			}

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
				case BloodweaverSpell.Dash:
					isDashing = true;
					isCasting = false;
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
				if (!isFocused && Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(3))
				{
					isTaunting = true;
					NPC.frameCounter = 0;
					NPC.frame.Y = 0;
				}
			}
			else if (Timer > 60)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					// Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
					// This means we also have to sync it after we spawned and set up the minion
					return;
				}

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
						ShootToPlayer(ModContent.ProjectileType<Projectiles.Hostile.BloodBoltHostile>(), 12, 18, .4f);
					}
				}
				else if (Timer % 15 == 0)
				{
					ShootToPlayer(ModContent.ProjectileType<Projectiles.Hostile.BloodBoltHostile>(), 10, 14, .4f);
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
		private void Update_Dash()
		{
			if (Timer == 2)
			{
				NPC.knockBackResist = 0;
				for (int i = 0; i < 8; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CrimsonTorch, Main.rand.NextFloatDirection() * 2.5f, Main.rand.NextFloatDirection() * 2.5f, 0, default(Color), 1.5f);
				}
			}
			if (Timer == 8)
			{
				SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.velocity = Vector2.UnitX * NPC.direction;
					NPC.velocity *= 14;

					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
					}
				}
			}
			NPC.velocity *= .98f;
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CrimsonTorch, -NPC.direction * 1.5f, Main.rand.NextFloatDirection(), 0, default(Color), 1.5f);

			if (Timer > 68)
			{
				NPC.knockBackResist = 0.15f;

				isDashing = false; // Stop dashing
				CastCooldown = 120;
				NPC.velocity = Vector2.Zero;
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

			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			Vector2 Velocity = Mathf.VelocityFPTP(NPC.Center, new Vector2(Target.Center.X, Target.Center.Y), speed);
			int Spread = 2;
			Velocity.X = Velocity.X + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
			Velocity.Y = Velocity.Y + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
			int i = Projectile.NewProjectile(NPC.GetSource_FromAI(), GetHandPosition(), Velocity, type, (int)(damage * ProjectileExpertDamageMultiplier), 3);
			Main.projectile[i].hostile = true;
			Main.projectile[i].friendly = false;
			Main.projectile[i].tileCollide = true;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 6; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CrimsonTorch, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Master mode loot
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.BloodweaverRelic>()));

			// Common loot
			//
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.BloodweaverTrophy>(), 10));
			npcLoot.Add(ItemDropRule.OneFromOptions(3, new int[]
				{
					ModContent.ItemType<Items.Weapons.Summon.GazerStaff>(),
					ModContent.ItemType<Items.Rings.BloodweaversRing>()
				}
			));

			// Drops after the EoC is downed
			//
			npcLoot.Add(Common.GlobalNPCs.DropRules.GetDropRule<EoCDownedDropCondition>(conditionalRule =>
			{
				conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Materials.BoneFragment>(), 1, maximumDropped: 8));
			}));
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)((float)NPC.lifeMax * 0.8f * balance);
			NPC.damage = (int)((double)NPC.damage * .8f);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 position = NPC.position - screenPos;
			SpriteEffects effect = (NPC.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, position, new Rectangle?(NPC.frame), drawColor, 0f, default(Vector2), NPC.scale, effect, 0f);
			return false;
		}

		public bool isIdle = true;
		public bool isTaunting = false;
		public bool isCasting = false;
		public bool isDashing = false;
		public bool isFocused = false;
		public bool wasFocused = false;

		public override void OnKill()
		{
			DownedSystem.downedBloodweaver = true;
			SupernovaNetworking.SyncWorldData();
		}
	}
}
