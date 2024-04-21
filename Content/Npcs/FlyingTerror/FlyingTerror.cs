using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using SupernovaMod.Common.ItemDropRules.DropConditions;
using SupernovaMod.Content.Items.Materials;
using SupernovaMod.Api;
using SupernovaMod.Common.Systems;
using SupernovaMod.Common;
using System.IO;

namespace SupernovaMod.Content.Npcs.FlyingTerror
{
    [AutoloadBossHead]
    public class FlyingTerror : ModNPC
    {
        private const int FRAME_DASH_DOWN = 29;

		private const int DAMAGE_PROJECILE = 20;
		private const int DAMAGE_PROJ_FIRE_BREATH = 23;

		private const float ExpertDamageMultiplier = .8f;
		private const float ProjectileExpertDamageMultiplier = .7f;

		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.CantTakeLunchMoney[Type] = true;
			NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.OnFire] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.ShadowFlame] = true;
			NPCID.Sets.TeleportationImmune[NPC.type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                // Influences how the NPC looks in the Bestiary
                PortraitScale = .75f,
                Scale = .75f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of NPC NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

				// Sets the description of NPC NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A scourge of the night, veraciously out for the hunt. Always looking for its next prey in its neverending pursuit to satiate its flesh craving. Its emissaries scour the underground searching for more victims to nourish their gluttonous master."),
			});
		}

		public override void SetDefaults()
        {
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            NPC.lifeMax = 4000; // The Max HP the boss has on Normal
            NPC.damage = 34; // The NPC damage value the boss has on Normal
            NPC.defense = 12; // The NPC defense on Normal
            NPC.knockBackResist = 0f; // No knockback
            NPC.width = 200;
            NPC.height = 100;
            NPC.value = 10000;
            NPC.npcSlots = 1f; // The higher the number, the more NPC slots NPC NPC takes.
            NPC.boss = true; // Is a boss
            NPC.lavaImmune = false; // Not hurt by lava
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Master mode loot
			//
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.FlyingTerrorRelic>()));

			// Expert mode loot
			//
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Consumables.BossBags.FlyingTerrorBossBag>()));
			npcLoot.Add(Common.GlobalNPCs.DropRules.GetDropRule<ExpertModeDropCondition>(conditionalRule =>
			{
				conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.TerrorInABottle>()));
			}));

			// Common loot
			//
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.FlyingTerrorTrophy>(), 10));
			npcLoot.Add(Common.GlobalNPCs.DropRules.GetDropRule<NormalModeDropCondition>(conditionalRule =>
			{
				conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TerrorTuft>(), 1, minimumDropped: 2, maximumDropped: 6));
			}));
		}

		protected Player target;
		protected float velocity;
		protected float acceleration;
		protected Vector2 targetOffset;
		protected float npcLifeRatio;

		public bool SecondPhase { get; private set; } = false;

		protected float _attackPointer2 = 0;

		public override void SendExtraAI(BinaryWriter writer)
		{
			//writer.Write(_attackPointer2);
			writer.Write(_playAnimation);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			//_attackPointer2 = reader.Read();
			_playAnimation = reader.ReadBoolean();
		}

		public override void AI()
        {
            // Set/Reset all required values
            //
            npcLifeRatio = NPC.life / (float)NPC.lifeMax;

			velocity = 12;
            acceleration = .05f;
			targetOffset = new Vector2(0, 250);

			ref float timer = ref NPC.ai[1];
            ref float attackPointer = ref NPC.ai[0];

			target = Main.player[NPC.target];

			drawMotionBlur = true;
			_trailColor = _glowDefault;
			_glowColor = _glowDefault;

			// Handle AI
			//
			if (DespawnAI())
			{
				return; // Don't run any other AI
			}
			timer++;

			if (npcLifeRatio > .92f)
			{
				if (attackPointer == 0 || attackPointer == 1)
				{
					AttackFlyAndShoot(ref timer, ref attackPointer);
				}
				else if (attackPointer == 2)
				{
					_trailColor = new Color(180, 50, 50, 245);
					AttackDive(ref timer, ref attackPointer);
				}
				else if (attackPointer == -1)
				{
					if (timer == 40)
					{
						SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center);
					}
					/*if (timer == 60)
					{
						acceleration = .4f;
						velocity = 25;
						drawMotionBlur = true;

						float gateValue = 100f;
						Vector2 distanceFromTarget = new Vector2(target.Center.X + (NPC.width * 4) * -NPC.direction, target.Center.Y) - NPC.Center;
						SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, velocity, acceleration, true);
					}*/
					if (timer > 40 && timer <= 120)
					{
						NPC.rotation += MathHelper.ToRadians(7.5f);

						if (timer % 20 == 0)
						{
							ShootToPlayer(ModContent.ProjectileType<Projectiles.TerrorFlame>(), DAMAGE_PROJECILE, .5f, 1 + Main.rand.NextFloat(-.1f, .1f));
						}
					}

					if (timer >= 140)
					{
						timer = 0;
						attackPointer++;
					}
				}
				else
				{
					attackPointer = 0;
				}
			}
			else if (npcLifeRatio > .5f)
			{
				if (attackPointer == 0 || attackPointer == 1 && (_attackPointer2 == 0 || _attackPointer2 == 1) || attackPointer == 3 || attackPointer == 4 && (_attackPointer2 == 0 || _attackPointer2 == 1))
				{
					AttackFlyAndShoot(ref timer, ref attackPointer);
				}
				else if (attackPointer == 1 || attackPointer == 4)
				{
					AttackFlyAndShoot2(ref timer, ref attackPointer);
				}
				else if (attackPointer == 2 && _attackPointer2 != 5)
				{
					_trailColor = new Color(180, 50, 50, 245);
					AttackDive(ref timer, ref attackPointer);
				}
				else if (attackPointer == 5 && (_attackPointer2 == 0 || _attackPointer2 == 2 || _attackPointer2 == 4))
				{
					_trailColor = new Color(180, 50, 50, 245);

					ref float direction = ref NPC.ai[2];
					if (timer == 1)
					{
						SoundEngine.PlaySound(SoundID.DD2_DrakinBreathIn);
						direction = (target.Center.X > NPC.Center.X) ? 1 : (-1);
					}

					if (timer == 170)
					{
						direction = (target.Center.X > NPC.Center.X) ? 1 : (-1);
						NPC.velocity = new Vector2((float)direction, 0f) * (Main.masterMode ? 22 : Main.expertMode ? 19 : 15);

						if (NPC.Center.X < target.Center.X - 2f)
						{
							NPC.direction = 1;
						}
						if (NPC.Center.X > target.Center.X + 2f)
						{
							NPC.direction = -1;
						}
						NPC.spriteDirection = NPC.direction;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ProjectileID.DD2BetsyFlameBreath, (int)(DAMAGE_PROJ_FIRE_BREATH * ProjectileExpertDamageMultiplier), 0f, Main.myPlayer, 0f, (float)NPC.whoAmI);
							Main.projectile[proj].tileCollide = false;
						}
						SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath, NPC.Center);

						if (Math.Abs(target.Center.X - NPC.Center.X) > 550f && Math.Abs(NPC.velocity.X) < 20f)
						{
							NPC.velocity.X = NPC.velocity.X + (float)Math.Sign(NPC.velocity.X) * 0.4f;
						}
					}

					if (timer >= 260)
					{
						timer = 0;
						direction = 0;
						attackPointer++;
					}

					if (timer < 160)
					{
						velocity = 15;
						acceleration = .2f;
						targetOffset = new Vector2(600 * direction, 150);
						MovementAI(target.Center - targetOffset, velocity, acceleration);
					}
					else
					{
						NPC.rotation = (NPC.rotation + (NPC.velocity.X * .25f) % 5) / 10f;
					}
				}
				else if (attackPointer == 5 && _attackPointer2 == 1 || attackPointer == 2)
				{
					AttackSpawnSpirits(ref timer, ref attackPointer);
				}
				else
				{
					attackPointer = 0;
					_attackPointer2 = Main.rand.Next(0, 5);
				}
			}
			else if (!SecondPhase)
			{
				NPC.localAI[0]++;

				if (NPC.localAI[0] == 1)
				{
					NPC.velocity = Vector2.Zero;
					NPC.dontTakeDamage = true;
					SoundEngine.PlaySound(SoundID.Roar);
				}

				if (NPC.localAI[0] > 20 && NPC.localAI[0] < 25)
				{
					for (int i = 0; i < 20; i++)
					{
						Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(-20, 20), 0).RotatedByRandom(MathHelper.ToRadians(360));
						Vector2 diff = NPC.Center - dustPos;
						diff.Normalize();

						Dust.NewDustPerfect(dustPos, DustID.Shadowflame, -diff * 15, Scale: Main.rand.Next(1, 4)).noGravity = true;
						Dust.NewDustPerfect(dustPos, DustID.Torch, -diff * 15, Scale: Main.rand.Next(1, 4)).noGravity = true;
					}
				}

				if (NPC.localAI[0] > 50)
				{
					NPC.rotation = 0;
					NPC.localAI[0] = 0;
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.ai[2] = 0;
					SecondPhase = true;
					NPC.dontTakeDamage = false;
					_attackPointer2 = Main.rand.Next(0, 2);
					_playAnimation = true;
				}
				velocity /= 2;
				MovementAI(target.Center - targetOffset, velocity, acceleration);
			}
			else
			{
				// Remove some attacks the more damage the boss has taken
				//
				if (npcLifeRatio < .35f && (attackPointer == 1 || attackPointer == 5))
				{
					attackPointer++;
				}
				if (npcLifeRatio < .2f && (attackPointer == 2 || attackPointer == 3))
				{
					attackPointer++;
				}
				// In expert mode or higher the boss only does special attacks at the end
				//
				if (npcLifeRatio < .05f && Main.expertMode && (attackPointer == 0 || attackPointer == 4))
				{
					attackPointer++;
				}
				// Handle attacks
				//
				if (attackPointer == 0 || attackPointer == 1 || attackPointer == 4 || attackPointer == 5)
				{
					AttackFlyAndShoot(ref timer, ref attackPointer);
				}
				else if (_attackPointer2 < 3 && (attackPointer == 2 || attackPointer == 3))
				{
					_trailColor = new Color(180, 50, 50, 245);

					ref float direction = ref NPC.ai[2];
					if (timer == 1)
					{
						SoundEngine.PlaySound(SoundID.DD2_DrakinBreathIn);
						direction = (target.Center.X > NPC.Center.X) ? 1 : (-1);
					}

					if (timer == 120)
					{
						direction = (target.Center.X > NPC.Center.X) ? 1 : (-1);
						NPC.velocity = new Vector2((float)direction, 0f) * (Main.masterMode ? 22 : Main.expertMode ? 19 : 15);

						if (NPC.Center.X < target.Center.X - 2f)
						{
							NPC.direction = 1;
						}
						if (NPC.Center.X > target.Center.X + 2f)
						{
							NPC.direction = -1;
						}
						NPC.spriteDirection = NPC.direction;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ProjectileID.DD2BetsyFlameBreath, (int)(DAMAGE_PROJ_FIRE_BREATH * ProjectileExpertDamageMultiplier), 0f, Main.myPlayer, 0f, (float)NPC.whoAmI);
							Main.projectile[proj].tileCollide = false;
						}
						SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath, NPC.Center);

						if (Math.Abs(target.Center.X - NPC.Center.X) > 550f && Math.Abs(NPC.velocity.X) < 20f)
						{
							NPC.velocity.X = NPC.velocity.X + (float)Math.Sign(NPC.velocity.X) * 0.5f;
						}
					}

					if (timer >= 240)
					{
						timer = 0;
						direction = 0;
						attackPointer++;
					}

					if (timer < 90 || timer > 190)
					{
						velocity = 20;
						acceleration = .25f;
						targetOffset = new Vector2(500 * direction, 150);
						MovementAI(target.Center - targetOffset, velocity, acceleration);
					}
					else
					{
						NPC.rotation = (NPC.rotation + (NPC.velocity.X * .25f) % 5) / 10f;
					}
				}
				else if (_attackPointer2 > 2 && (attackPointer == 2))
				{
					AttackDive(ref timer, ref attackPointer);
				}
				else if (_attackPointer2 > 2 && (attackPointer == 3))
				{
					attackPointer++;
				}
				else if (attackPointer == 6)
				{
					ref float isAtTarget = ref NPC.localAI[0];
					ref float angle = ref NPC.localAI[1];

					if (isAtTarget == 1)
					{
						NPC.velocity = Vector2.Zero;
						NPC.rotation = 0;

						// Play Charge effect
						//
						if (timer < 120)
						{
							if (timer == 20)
							{
								SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
							}
							Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(115, 125), 0).RotatedByRandom(MathHelper.ToRadians(360));
							Vector2 diff = NPC.Center - dustPos;
							diff.Normalize();

							Dust.NewDustPerfect(dustPos, ModContent.DustType<Dusts.TerrorDust>(), diff * 10, Scale: 2).noGravity = true;
							Dust.NewDustPerfect(dustPos, DustID.Shadowflame, diff * 10, Scale: 2).noGravity = true;
							return;
						}

						// Use a random charge attack
						//
						if (_attackPointer2 == 0 || _attackPointer2 == 3)
						{
							if (timer < 360 && timer % 50 == 0)
							{
								// Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
								// This means we also have to sync it after we spawned and set up the minion
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Vector2 randPos = target.Center + Main.rand.NextVector2CircularEdge(250, 250);

									for (int i = 0; i < 8; i++)
									{
										Vector2 dustPos = randPos + new Vector2(Main.rand.Next(115, 125), 0).RotatedByRandom(MathHelper.ToRadians(360));
										Vector2 diff = randPos - dustPos;
										diff.Normalize();

										Dust.NewDustPerfect(dustPos, ModContent.DustType<Dusts.TerrorDust>(), diff * 5, Scale: 1.5f).noGravity = true;
										Dust.NewDustPerfect(dustPos, DustID.Shadowflame, diff * 5, Scale: 1.5f).noGravity = true;
									}

									NPC minionNPC = NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)randPos.X, (int)randPos.Y, ModContent.NPCType<TerrorSpirit>());

									// Finally, syncing, only sync on server and if the NPC actually exists (Main.maxNPCs is the index of a dummy NPC, there is no point syncing it)
									//
									if (Main.netMode == NetmodeID.Server)
									{
										NetMessage.SendData(MessageID.SyncNPC, number: minionNPC.whoAmI);
									}
								}
							}
							if (timer >= 420)
							{
								timer = 0;
								isAtTarget = 0;
								attackPointer++;
							}
						}
						else if (_attackPointer2 == 1 || _attackPointer2 == 4)
						{
							if (timer % 25 == 0)
							{
								SoundEngine.PlaySound(SoundID.Item8, NPC.Center);

								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Vector2 position = target.Center + new Vector2(Main.rand.Next(-750, 750), -1000);
									Projectile.NewProjectile(NPC.GetSource_FromAI(), position, Vector2.UnitY * 5.7f, ProjectileID.CultistBossFireBallClone, (int)(DAMAGE_PROJECILE * ProjectileExpertDamageMultiplier), 6, Main.myPlayer);
								}
							}
							if (timer >= 310)
							{
								timer = 0;
								isAtTarget = 0;
								attackPointer++;
							}
						}
						else if (_attackPointer2 == 2 || _attackPointer2 == 5)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient && timer % 40 == 0)
							{
								int num220 = 24;
								for (int num221 = 0; num221 < num220; num221++)
								{
									// Create velocity for angle
									Vector2 value17 = -Vector2
										.Normalize(Vector2.One)
										// Rotate by angle
										.RotatedBy(MathHelper.ToRadians((360 / num220 * (num221 - 2)) + angle))
										// Make the velocity 3
										* 2;

									// Create a projectile for velocity
									SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
									int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, value17.X, value17.Y, ModContent.ProjectileType<Projectiles.TerrorFlame>(), (int)(DAMAGE_PROJECILE * ProjectileExpertDamageMultiplier), 1f, Main.myPlayer, 1, Main.rand.Next(-45, 1));
									Main.projectile[proj].timeLeft *= 2;
								}
								angle += 25;
							}
							if (timer >= 310)
							{
								timer = 0;
								angle = 0;
								isAtTarget = 0;
								attackPointer++;
							}
						}
					}
					else
					{
						if (Vector2.Distance(NPC.Center, target.Center - new Vector2(0, 400)) <= 100)
						{
							isAtTarget = 1;
						}
						timer = 0;
						MovementAI(target.Center - new Vector2(0, 400), velocity * 1.5f, .2f);
					}
				}
				else
                {
                    attackPointer = 0;
					_attackPointer2++;
					if (_attackPointer2 > 5)
					{
						_attackPointer2 = 0;
					}
				}
			}
        }

		#region AI Methods
		private bool DespawnAI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}

			Player player = Main.player[NPC.target];
			if (Main.dayTime || NPC.ai[0] != 3f && (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 3000))
			{
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
				if (Main.dayTime || player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 3000)
				{
					if (NPC.timeLeft > 180)
					{
						NPC.timeLeft = 180;
					}
					NPC.ai[0] = 0;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.netUpdate = true;

					NPC.velocity = new Vector2(7, -5);
					return true;
				}
			}
			else if (NPC.timeLeft < 1800)
			{
				NPC.timeLeft = 1800;
			}
			return false;
		}
		private void MovementAI(Vector2 targetPosition, float velocity, float acceleration)
		{
			// Set direction
			//
			if (NPC.Center.X < target.Center.X - 2f)
			{
				NPC.direction = 1;
			}
			if (NPC.Center.X > target.Center.X + 2f)
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;
			NPC.rotation = (NPC.rotation + (NPC.velocity.X * .25f) % 20) / 10f;

			NPC.rotation = MathHelper.ToRadians(NPC.velocity.X % 15);

			float gateValue = 100f;
			Vector2 distanceFromTarget = targetPosition - NPC.Center;
			SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, velocity, acceleration, true);
		}
		#endregion

		#region Attack Methods
		private void AttackFlyAndShoot(ref float timer, ref float attackPointer)
		{
			if (NPC.Center.X < target.Center.X - 2f)
			{
				NPC.direction = 1;
			}
			if (NPC.Center.X > target.Center.X + 2f)
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;
			NPC.rotation = (NPC.rotation + (NPC.velocity.X * .5f) % 15) / 10f;
			Vector2 value53 = target.Center - NPC.Center;
			value53.Y -= 200f;
			if (value53.Length() > 2800f)
			{
				NPC.TargetClosest(true);
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
				NPC.ai[2] = 0;
			}
			else if (value53.Length() > 240f)
			{
				float scaleFactor17 = 4.5f;
				if (SecondPhase)
				{
					scaleFactor17 = 6;
				}
				float num1309 = 30;
				value53.Normalize();
				value53 *= scaleFactor17;
				NPC.velocity = (NPC.velocity * (num1309 - 1f) + value53) / num1309;
			}
			else if (NPC.velocity.Length() > 2f)
			{
				NPC.velocity *= 0.95f;
			}
			else if (NPC.velocity.Length() < 1f)
			{
				NPC.velocity *= 1.05f;
			}

			if (SecondPhase)
			{
				if (timer > 300)
				{
					timer = 0;
					attackPointer++;
				}
				else if (timer == 80)
				{
					SoundEngine.PlaySound(SoundID.DD2_DrakinBreathIn);
				}
				else if (timer == 180)
				{
					SoundEngine.PlaySound(SoundID.DD2_DrakinShot);

					for (int i = 0; i < 3; i++)
					{
						ShootToPlayer(ModContent.ProjectileType<Content.Projectiles.Boss.TerrorBlast>(), DAMAGE_PROJECILE, Main.rand.NextFloat(.7f, .9f), Main.rand.NextFloat(.99f, 1.1f));
					}
				}
			}
			else
			{
				if (timer > 365)
				{
					timer = 0;
					attackPointer++;
				}
				else if (timer >= 80)
				{
					int timeBtwnShots = 180;
					if (timer % timeBtwnShots == 0)
					{
						SoundEngine.PlaySound(SoundID.DD2_DrakinShot);

						ShootToPlayer(ModContent.ProjectileType<Content.Projectiles.Boss.TerrorBlast>(), DAMAGE_PROJECILE, .85f);
					}
					else if (timer % (timeBtwnShots / 2) == 0)
					{
						SoundEngine.PlaySound(SoundID.DD2_DrakinBreathIn);
					}
				}
			}
		}
		private void AttackFlyAndShoot2(ref float timer, ref float attackPointer)
		{
			if (NPC.Center.X < target.Center.X - 2f)
			{
				NPC.direction = 1;
			}
			if (NPC.Center.X > target.Center.X + 2f)
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;
			NPC.rotation = (NPC.rotation + (NPC.velocity.X * .5f) % 15) / 10f;
			Vector2 value53 = target.Center - NPC.Center;
			value53.Y -= 200f;
			if (value53.Length() > 2800f)
			{
				NPC.TargetClosest(true);
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
				NPC.ai[2] = 0;
			}
			else if (value53.Length() > 240f)
			{
				float scaleFactor17 = 4.5f;
				if (SecondPhase)
				{
					scaleFactor17 = 6;
				}
				float num1309 = 30;
				value53.Normalize();
				value53 *= scaleFactor17;
				NPC.velocity = (NPC.velocity * (num1309 - 1f) + value53) / num1309;
			}
			else if (NPC.velocity.Length() > 2f)
			{
				NPC.velocity *= 0.95f;
			}
			else if (NPC.velocity.Length() < 1f)
			{
				NPC.velocity *= 1.05f;
			}

			if (timer > 280)
			{
				timer = 0;
				attackPointer++;
			}
			else if (timer == 80)
			{
				SoundEngine.PlaySound(SoundID.ForceRoar);
			}
			else if (timer == 140)
			{
				float velMulti = target.velocity.X * 5;
				for (int i = 0; i < Main.rand.Next(3, 6); i++)
				{
					Vector2 position = target.Center - (Vector2.UnitY * 600);
					position.X += Main.rand.Next(-500, 500) + velMulti;
					float rotation = (float)Math.Atan2(position.Y - (target.position.Y + target.height * 0.2f), position.X - (target.position.X + target.width * 0.15f));
					rotation *= 1 + Main.rand.NextFloat(-.15f, .15f);

					Vector2 velocity = new Vector2((float)-(Math.Cos(rotation) * 18) * .75f, (float)-(Math.Sin(rotation) * 18) * .75f) * 1.2f;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<Content.Projectiles.Boss.TerrorBlast>(), (int)(DAMAGE_PROJ_FIRE_BREATH * ProjectileExpertDamageMultiplier), 0f, 0, 1, target.whoAmI);
				}
			}
		}
		private void AttackDive(ref float timer, ref float attackPointer)
		{
			bool atTargetPositionY = (NPC.position.Y + NPC.height) <= target.position.Y;
			bool atTargetPositionX = (NPC.Center.X < (target.Center.X + target.width)) && (NPC.Center.X > (target.Center.X - target.width));

			velocity = 18;
			acceleration = .15f;

			Vector2 targetPos = new Vector2(0, 440);

			// Telegraphing before the dash
			//
			if (timer == 100)
			{
				SoundEngine.PlaySound(SoundID.DD2_DrakinBreathIn);
			}

			if (NPC.ai[2] == 0 && timer >= 120)
			{
				// Check if the boss is above the player,
				// if so start the dash.
				//
				NPC.ai[2] = (atTargetPositionY && atTargetPositionX) ? 1 : 0;
				if (NPC.ai[2] == 1)
				{
					SoundEngine.PlaySound(SoundID.ForceRoarPitched, NPC.Center);
					timer = 0;
					_playAnimation = false;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.velocity /= 3;
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
						}
					}
				}
			}
			// Handle the dash
			//
			else if (NPC.ai[2] == 1)
			{
				acceleration = .4f;
				velocity = 21;
				drawMotionBlur = true;

				float gateValue = 100f;
				Vector2 distanceFromTarget = new Vector2(target.Center.X, target.Center.Y + (NPC.height * 6)) - NPC.Center;
				SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, velocity, acceleration, true);

				if (Main.netMode != NetmodeID.MultiplayerClient && (NPC.position.Y + NPC.height) >= target.position.Y + (NPC.height * 6))
				{
					NPC.velocity = Vector2.Zero;
					NPC.ai[2] = 2;
					_playAnimation = true;
				}
				return;
			}
			else if (NPC.ai[2] == 2)
			{
				acceleration = .05f;
				targetPos.X = NPC.width * NPC.direction;
				if ((NPC.position.Y + NPC.height) <= target.position.Y)
				{
					timer = 0;
					NPC.ai[2] = 0;
					attackPointer++;
				}
			}
			MovementAI(target.position - targetPos, velocity, acceleration);
		}
		private void AttackSpawnSpirits(ref float timer, ref float attackPointer)
		{
			ref float isAtTarget = ref NPC.localAI[0];

			if (isAtTarget == 1)
			{
				NPC.velocity = Vector2.Zero;
				NPC.rotation = 0;

				if (timer < 60)
				{
					if (timer == 20)
					{
						SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
					}
					Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(115, 125), 0).RotatedByRandom(MathHelper.ToRadians(360));
					Vector2 diff = NPC.Center - dustPos;
					diff.Normalize();

					Dust.NewDustPerfect(dustPos, ModContent.DustType<Dusts.TerrorDust>(), diff * 10, Scale: 2).noGravity = true;
					Dust.NewDustPerfect(dustPos, DustID.Shadowflame, diff * 10, Scale: 2).noGravity = true;
					return;
				}

				if (timer % 40 == 0)
				{
					Vector2 randPos = target.Center + Main.rand.NextVector2CircularEdge(250, 250);

					for (int i = 0; i < 8; i++)
					{
						Vector2 dustPos = randPos + new Vector2(Main.rand.Next(115, 125), 0).RotatedByRandom(MathHelper.ToRadians(360));
						Vector2 diff = randPos - dustPos;
						diff.Normalize();

						Dust.NewDustPerfect(dustPos, ModContent.DustType<Dusts.TerrorDust>(), diff * 5, Scale: 1.5f).noGravity = true;
						Dust.NewDustPerfect(dustPos, DustID.Shadowflame, diff * 5, Scale: 1.5f).noGravity = true;
					}

					NPC.NewNPC(NPC.GetSource_FromAI(), (int)randPos.X, (int)randPos.Y, ModContent.NPCType<TerrorSpirit>());
				}
				if (timer >= (npcLifeRatio < .75 ? 170 : 120))
				{
					timer = 0;
					isAtTarget = 0;
					attackPointer++;
				}
			}
			else
			{
				if (Vector2.Distance(NPC.Center, target.Center - new Vector2(0, 400)) <= 100)
				{
					isAtTarget = 1;
				}
				timer = 0;
				MovementAI(target.Center - new Vector2(0, 400), velocity * 1.5f, .2f);
			}
		}
		#endregion

		#region Helper Methods
		private Vector2 _desiredDestination;
		private Vector2 GetRandFlyDestination()
		{
			if (target.position == Vector2.Zero)
			{
				return target.position;
			}

			if (_desiredDestination == Vector2.Zero ||
				Vector2.Distance(target.Center, _desiredDestination) >= 400 ||    // Make sure the boss will not get to far from the player
				Vector2.Distance(NPC.Center, _desiredDestination) >= 10			 //
			)
			{
				_desiredDestination = target.position + target.velocity;
				_desiredDestination.X += 350 * -NPC.direction;
				_desiredDestination.Y -= 100;
			}
			return _desiredDestination;
		}
		private void ShootToPlayer(int type, int damage, float velocityMulti = 1, float rotationMulti = 1)
        {
            //int type = ModContent.ProjectileType<TerrorProj>();
            SoundEngine.PlaySound(SoundID.Item20, NPC.Center);

			/*if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}*/

            Vector2 position = new Vector2(NPC.Center.X + (NPC.width / 2 + 25) * NPC.direction, NPC.Center.Y + NPC.height - 50);
			float rotation = (float)Math.Atan2(position.Y - (target.position.Y + target.height * 0.2f), position.X - (target.position.X + target.width * 0.15f));
			rotation *= rotationMulti;

            Vector2 velocity = new Vector2((float)-(Math.Cos(rotation) * 18) * .75f, (float)-(Math.Sin(rotation) * 18) * .75f) * velocityMulti;
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, (int)(damage * ProjectileExpertDamageMultiplier), 0,0,0,0);

            for (int x = 0; x < 5; x++)
            {
                int dust = Dust.NewDust(position, 50, 50, ModContent.DustType<Dusts.TerrorDust>(), velocity.X, velocity.Y, 80, default, Main.rand.NextFloat(.9f, 1.6f));
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= Main.rand.NextFloat(.3f, .5f);
            }
        }
		#endregion

		private readonly Color _glowDefault = new Color(180, 180, 180, 245);
		private readonly Color _glowDefault2 = new Color(180, 180, 180, 200);
		private Color _glowColor;
		private Color _trailColor;
		private bool drawMotionBlur;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;

			Texture2D glowMask = ModContent.Request<Texture2D>("SupernovaMod/Content/Npcs/FlyingTerror/FlyingTerrorGlowMask").Value;

			// Draw motion blur
			//
			if (drawMotionBlur)
			{
				SpriteEffects spriteEffects = 0;
				if (NPC.spriteDirection == 1)
				{
					spriteEffects = SpriteEffects.FlipHorizontally;
				}
				Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
				Color color = NPC.GetAlpha(drawColor);
				Color color2 = NPC.GetAlpha(drawColor);
				float amount9 = 0f;
				int num153 = 10;
				int num154 = 2;
				for (int num155 = 1; num155 < num153; num155 += num154)
				{
					Color color3 = color;
					color3 = Color.Lerp(color3, color2, amount9);
					color3 = NPC.GetAlpha(color3);
					color3 *= (float)(num153 - num155) / 15f;
					Vector2 position = base.NPC.oldPos[num155] + new Vector2((float)base.NPC.width, (float)base.NPC.height) / 2f - screenPos;
					position -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[base.NPC.type])) * base.NPC.scale / 2f;
					position += origin * base.NPC.scale + new Vector2(0f, base.NPC.gfxOffY);
					spriteBatch.Draw(texture, position, new Rectangle?(base.NPC.frame), color3, base.NPC.rotation, origin, base.NPC.scale, spriteEffects, 0f);

					color3 = _glowColor;
					color3 = Color.Lerp(color3, color2, amount9);
					color3 = NPC.GetAlpha(color3);
					color3 *= (float)(num153 - num155) / 15f;
					spriteBatch.Draw(glowMask, position, new Rectangle?(base.NPC.frame), color3, base.NPC.rotation, origin, base.NPC.scale, spriteEffects, 0f);
				}
			}

			Vector2 drawOrigin = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[base.NPC.type] / 2));
			Vector2 drawPosition = NPC.Center - screenPos;
			drawPosition -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
			drawPosition += drawOrigin * NPC.scale + new Vector2(0f, NPC.gfxOffY);

			SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			// Draw NPC
			Color color37 = Color.Lerp(Color.White, Color.Yellow, 0.25f);
			spriteBatch.Draw(texture, drawPosition, new Rectangle?(NPC.frame), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, NPC.scale, effects, 0f);

			// Draw Glowmask
			spriteBatch.Draw(glowMask, drawPosition, NPC.frame, _glowColor, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			return false;
        }

        private bool _playAnimation = true;
        public override void FindFrame(int frameHeight)
        {
            if (_playAnimation)
            {
				NPC.frameCounter++;
                NPC.frameCounter %= 30;

                int frame = (int)(NPC.frameCounter / 6.7);
                if (frame >= Main.npcFrameCount[NPC.type] - 1) frame = 0;
                NPC.frame.Y = frame * frameHeight;
				/*NPC.frameCounter++;

				if (NPC.frameCounter > 14)
				{
					NPC.frameCounter = 0;
					NPC npc = NPC;
					npc.frame.Y = npc.frame.Y + frameHeight;

					// Check if the frame exceeds the height of our sprite sheet.
					// If so we reset to 0.
					//
					if (NPC.frame.Y >= frameHeight * (Main.npcFrameCount[NPC.type]-2))
					{
						NPC.frame.Y = 0;
					}
				}*/
			}
            else
            {
                int frame = (int)(FRAME_DASH_DOWN / 6.7);
                NPC.frame.Y = frame * frameHeight;
            }
        }

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.TerrorDust>(), hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}

			int i = 0;
			while ((double)i < hit.Damage / (double)NPC.lifeMax * 100.0)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, hit.HitDirection, -1f, 0, default(Color), 1.2f);
				i++;
			}
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)((float)NPC.lifeMax * 0.8f * balance);
			NPC.damage = (int)((double)NPC.damage * ExpertDamageMultiplier);
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
		public override void OnKill()
		{
			if (!DownedSystem.downedFlyingTerror)
			{
				string key = "Mods.SupernovaMod.Status.Progression.StartDroppingRime";
				SupernovaUtils.NewLocalizedText(key, Color.LightSkyBlue);
			}
			DownedSystem.downedFlyingTerror = true;
			SupernovaNetworking.SyncWorldData();
		}
	}
}
