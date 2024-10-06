using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Api;
using SupernovaMod.Api.Effects;
using SupernovaMod.Api.Helpers;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Npcs.Fallen
{
	[AutoloadBossHead]
	public class Fallen : ModNPC
	{
		public enum AIState
		{
			Initial,
			Phase1,
			Phase2,
		}

		protected float npcLifeRatio;
		public Player target;

		public AIState State { get; private set; }

		private const float ExpertDamageMultiplier = .7f;

		public override void SetStaticDefaults()
		{
			NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
			NPCID.Sets.CantTakeLunchMoney[Type] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.ShadowFlame] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.OnFire] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.OnFire3] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Frostburn2] = true;
			NPCID.Sets.TeleportationImmune[NPC.type] = true;
			NPCID.Sets.TrailingMode[NPC.type] = 1;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				// Influences how the NPC looks in the Bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),
            });
        }

        protected float Timer
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		protected float AtkPtr
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		protected int AtkTimer { get; set; }

		public override void SetDefaults()
		{
			NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
			NPC.lifeMax = 40000; // The Max HP the boss has on Normal
			NPC.damage = 42; // The base damage value the boss has on Normal
			NPC.defense = 16; // The base defense on Normal
			NPC.knockBackResist = 0f; // No knockback
			NPC.width = 74;
			NPC.height = 112;
			NPC.value = 10000;
			NPC.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
			NPC.boss = true; // Is a boss
			NPC.lavaImmune = true; // Not hurt by lava
			NPC.noGravity = true; // Not affected by gravity
			NPC.noTileCollide = true; // Will not collide with the tiles. 
			NPC.HitSound = SoundID.NPCHit52;
			NPC.DeathSound = SoundID.NPCDeath55;
		}

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)((float)NPC.lifeMax * 0.75f * balance);
            NPC.damage = (int)((double)NPC.damage * ExpertDamageMultiplier);
        }

        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.localAI[0]);
			writer.Write((int)State);
			writer.Write(AtkTimer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.localAI[0] = reader.ReadSingle();
			State = (AIState)reader.ReadInt32();
			AtkTimer = reader.ReadInt32();
		}

		public override void AI()
		{
			AI_Setup();
			if (AI_Despawn())
			{
				return;
			}
			switch (State)
			{
				case AIState.Phase1:
					AI_Stage1();
					break;
				case AIState.Phase2:
					AI_Stage2();
					break;
			}
        }

		private void AI_Setup()
		{
			npcLifeRatio = NPC.life / (float)NPC.lifeMax;
			target = Main.player[NPC.target];

			// Run this method once
			//
			if (State == AIState.Initial && Main.netMode != NetmodeID.MultiplayerClient)
			{
				// Set the phase
				State = AIState.Phase1;
			}

			// Enter the second phase when the npc life ratio is at 50%
			//
			if (State == AIState.Phase1 && npcLifeRatio <= .5f)
			{
                State = AIState.Phase2;

				// Reset
				Timer = 0;
				AtkPtr = 0;
				AtkTimer = 0;
            }
        }
		private bool AI_Despawn()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 10000)
			{
				NPC.TargetClosest(true);
			}

			Player player = Main.player[NPC.target];
			if (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 10000)
			{
				NPC.TargetClosest(true);
				player = Main.player[NPC.target];
				if (player.dead || !player.active || Vector2.Distance(NPC.Center, player.Center) > 10000)
				{
					NPC.ai[0] = 0;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.netUpdate = true;

					NPC.velocity = Vector2.Zero;
					NPC.ai[3]++;

					if (NPC.alpha < 255)
					{
						NPC.alpha++;
					}
					if (NPC.ai[3] < 180)
					{
						// Charge dust effect
						//
						SoundEngine.PlaySound(SoundID.Item15);
						Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(110, 130), 0).RotatedByRandom(MathHelper.ToRadians(360));
						Vector2 diff = NPC.Center - dustPos;
						diff.Normalize();

						Dust.NewDustPerfect(dustPos, DustID.Ash, diff * 10, Scale: 2).noGravity = true;
						Dust.NewDustPerfect(dustPos, DustID.Demonite, diff * 10, Scale: 2).noGravity = true;
					}
					else
					{
						NPC.timeLeft = 0;
						NPC.active = false;
					}

					return true;
				}
			}
			else if (NPC.timeLeft < 1800)
			{
				NPC.timeLeft = 1800;
			}
			return false;
		}

		private void AI_Stage1()
		{
			float baseVelocity = 12;
			float baseAcceleration = .12f;		
			if (Main.expertMode)
			{
				baseAcceleration += .03f;
			}

            ref float attackPosition = ref NPC.ai[2];

			if (Timer < 4) // Left or Right attacks
			{
				// Setup the attackPosition
				//
				if (attackPosition == 0)
				{
					attackPosition = 1;
				}
				switch (AtkPtr)
				{
					case 1:
						if (HandleAttackLaser(-attackPosition))
						{
							AtkTimer = 0;
							AtkPtr = Main.rand.Next(2, 4);
							Timer++;
						}
						else if (AtkTimer > 75 && AtkTimer <= 280)
						{
							// Dont move our Y position while shooting our laser
							NPC.velocity = Vector2.Zero;
							return; 
						}
						break;
					case 2:
						if (HandleAttackShootStars())
						{
							AtkTimer = 0;
							AtkPtr = Main.rand.Next(1, 4);
							Timer++;
						}
						break;
					case 3:
						if (HandleAttackAshBall())
						{
							AtkTimer = 0;
							AtkPtr = Main.rand.Next(1, 4);
							Timer++;
						}
						break;
					default:
						AtkTimer = -140; // Wait 140 ticks before the first attack
						AtkPtr = Main.rand.Next(1, 4);
						break;
				}
			}
			// Save the current target center position
			//
			else if (Timer == 3)
			{
				NPC.ai[1] = target.position.X;// - (target.velocity.X * 2);
				NPC.ai[3] = target.position.Y;// - (target.velocity.Y * 2);
                Timer++;
            }
            else if (NPC.localAI[0] == 0 && Timer <= 360) // Center attack: Lasers
			{
				Timer++;
				if (Timer == 70)
				{
					SoundEngine.PlaySound(SoundID.Item119, NPC.Center);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient && Timer > 80)
				{
					NPC.velocity = Vector2.Zero;

					if (Timer <= 244 && Timer % 80 == 0)
					{
						SoundEngine.PlaySound(SoundID.Zombie104, NPC.Center);
						int type = ModContent.ProjectileType<Projectiles.FallenBeamStart>();
						int damage = (int)(42 * ExpertDamageMultiplier);
						int[] proj;
						if (AtkTimer % 2 == 0)
						{
							proj = ProjectileHelper.ShootPlusPattern(NPC.GetSource_FromAI(), NPC.Center, 4, 6, type, damage, 1, Main.myPlayer, .017f, NPC.whoAmI, 0);
						}
						else
						{
							proj = ProjectileHelper.ShootCrossPattern(NPC.GetSource_FromAI(), NPC.Center, 4, 6, type, damage, 1, Main.myPlayer, .017f, NPC.whoAmI, 0);
						}
						foreach (int i in proj)
						{
							Main.projectile[i].timeLeft = 90;
							(Main.projectile[i].ModProjectile as Projectiles.FallenBeamStart).Lifetime = 90;
						}
						AtkTimer++;
					}
				}
				else if (Timer <= 160)
				{
					int dustType = DustID.LifeDrain;
					Vector2 dustPosition = NPC.Center + NPC.velocity * .2f;
					for (int i = 0; i < 2; i++)
					{
						Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(NPC.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * Utils.NextFloat(Main.rand, 2f, 4f);
						Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1.75f);
						dust.noGravity = true;
					}
				}

				// Move to last target position
				MovementAI(new Vector2(NPC.ai[1], NPC.ai[3]), baseVelocity, baseAcceleration);
				return;
			}
			else if (NPC.localAI[0] == 1 && Timer <= 380)
			{
				Timer++;
				if (Timer == 60)
				{
					SoundEngine.PlaySound(SoundID.Item119, NPC.Center);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient && Timer > 60 && Timer <= 320)
				{
					NPC.velocity = Vector2.Zero;

					if (Timer % 20 == 0)
					{
						SoundEngine.PlaySound(SoundID.NPCDeath55, NPC.Center);
						int type = ModContent.ProjectileType<Projectiles.BlackStar>();
						int damage = (int)(36 * ExpertDamageMultiplier);
						int[] proj;
						
						if (AtkTimer % 2 == 0)
						{
							proj = ProjectileHelper.ShootPlusPattern(NPC.GetSource_FromAI(), NPC.Center, 16, 6, type, damage, 1, Main.myPlayer, 1, 2.5f);
                        }
						else
						{
							proj = ProjectileHelper.ShootPlusPattern(NPC.GetSource_FromAI(), NPC.Center, 13, 6, type, damage, 1, Main.myPlayer, 1, 2.5f);
                        }

						foreach (int projID in proj)
						{
							Main.projectile[projID].velocity = Main.projectile[projID].velocity.RotatedBy(MathHelper.ToRadians(AtkTimer));
						}
                        AtkTimer += 12 % 360;

                        /*if (AtkTimer % 4 == 0)
						{
							proj = ProjectileHelper.ShootPlusPattern(NPC.GetSource_FromAI(), NPC.Center, 18, 8, type, damage, 1, Main.myPlayer, 1, 2.5f);
						}
						else if (AtkTimer % 2 == 0)
						{
							proj = ProjectileHelper.ShootCrossPattern(NPC.GetSource_FromAI(), NPC.Center, 16, 8, type, damage, 1, Main.myPlayer, 1, 2.5f);
						}
						else
						{
							proj = ProjectileHelper.ShootPlusPattern(NPC.GetSource_FromAI(), NPC.Center, 12, 8, type, damage, 1, Main.myPlayer, 1, 2.5f);
						}*/
                        //AtkTimer++;
                    }
                }
				//else if (Timer <= 160)
				{
					int dustType = DustID.Shadowflame;
					Vector2 dustPosition = NPC.Center + NPC.velocity * .2f;
					for (int i = 0; i < 2; i++)
					{
						Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(NPC.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * Utils.NextFloat(Main.rand, 2f, 4f);
						Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1.75f);
						dust.noGravity = true;
					}
				}

				// Move to last target position
				MovementAI(new Vector2(NPC.ai[1], NPC.ai[3]), 12, .2f);
				return;
			}
			else if (NPC.localAI[0] == 2)
			{
				NPC.localAI[0] = 0;
			}
			else
			{
				attackPosition = -attackPosition;
				NPC.localAI[0]++;
				NPC.ai[0] = 0;
				NPC.ai[1] = 0;
				NPC.ai[3] = 0;
				AtkTimer = 0;
			}

			MovementAI(target.Center + new Vector2(550 * attackPosition, 0), 12, .2f);
		}
        private void AI_Stage2()
        {
            float baseVelocity = 8;
            float baseAcceleration = .1f;
            if (Main.expertMode)
            {
                baseAcceleration += .04f;
            }

            ref float attackPosition = ref NPC.ai[2];

			//if (Timer < 3)
			{
                switch (AtkPtr)
                {
                    case 1:
                        if (HandleAttackDash())
                        {
                            AtkTimer = 0;
                            AtkPtr = Main.rand.Next(2, 5);
                            Timer++;
                        }
                        else if (AtkTimer < 80)
                        {
                            NPC.velocity = Vector2.Zero;
                        }
                        return;
                    case 2:
                        if (HandleAttackUnholyTridents())
                        {
                            AtkTimer = 0;
                            AtkPtr = Main.rand.Next(1, 5);
                            Timer++;
                        }
						break;
					case 3:
						//if (HandleAttackShootStars2())
						if (HandleAttackShootStars())
						{
							AtkTimer = 0;
							AtkPtr = Main.rand.Next(1, 5);
							//if (AtkPtr == 3) AtkPtr = Main.rand.Next(1, 2);
							Timer++;
						}
						/*else if (AtkTimer < 86)
						{
							NPC.velocity = Vector2.Zero;
						}*/
						break;
					case 4:
						// When there are more than 4 ash ball projectiles already
						// active try another attack to avoid ash ball spam.
						//
						if (GetActiveNPCCount(ModContent.NPCType<Projectiles.AshBall>()) >= 4)
						{
							goto default;
						}
                        if (HandleAttackAshBallBarrage())
						{
                            AtkTimer = 0;
                            AtkPtr = Main.rand.Next(1, 4);
                            Timer++;
                        }
						break;
                    default:
                        AtkTimer = -120; // Wait 120 ticks before the first attack
						AtkPtr = Main.rand.Next(1, 5);
                        break;
                }
            }
            MovementAI(target.Center - new Vector2(0, 265), baseVelocity, baseAcceleration);
        }

        private void MovementAI(Vector2 destination, float velocity, float acceleration)
		{
			float movementDistanceGateValue = 100f;
			Vector2 distanceFromTarget = destination - NPC.Center;
            SupernovaUtils.MoveNPCSmooth(NPC, movementDistanceGateValue, distanceFromTarget, velocity, acceleration, true);

			// Set direction
			//
			if (NPC.velocity.X < 0f)
			{
				NPC.direction = -1;
			}
			else
			{
				NPC.direction = 1;
			}
		}

		#region Attacks

		private bool HandleAttackLaser(float direction)
		{
			AtkTimer++;
            // Lazer telegraph
            //
			if (AtkTimer > 0 && AtkTimer < 120)
			{
				int dustType = DustID.LifeDrain;
				Vector2 dustPosition = NPC.Center + NPC.velocity * .2f;
				for (int i = 0; i < 2; i++)
				{
					Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(NPC.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * Utils.NextFloat(Main.rand, 2f, 4f);
					Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1.75f);
					dust.noGravity = true;
				}
			}
            // After the telegraph spawn the lazer and give it
			// a random starting velocity with one making the 
			// lazer go from the top to the bottom, and the 
			// other from the bottom to the top.
            //
			else if (Main.netMode != NetmodeID.MultiplayerClient && AtkTimer == 120)
			{
				SoundEngine.PlaySound(SoundID.Zombie104, NPC.Center);
				int type2 = ModContent.ProjectileType<Projectiles.FallenBeamStart>();
				int damage = (int)(42 * ExpertDamageMultiplier);
				Vector2 velocity = Main.rand.NextBool() ? Vector2.UnitY : -Vector2.UnitY;
				Projectiles.FallenBeamStart proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, velocity, type2, damage, 0f, Main.myPlayer, (.015f * direction) * -velocity.Y, NPC.whoAmI, direction).ModProjectile as Projectiles.FallenBeamStart;
				proj.useMoveAI = true;
			}
			return AtkTimer >= 380;
        }
		private bool HandleAttackShootStars()
		{
			AtkTimer++;

            if (Main.netMode != NetmodeID.MultiplayerClient && AtkTimer > 0 && AtkTimer <= 90 && AtkTimer % 30 == 0)
            {
				// Dust
				//
				int dustType = DustID.Shadowflame;
				Vector2 dustPosition = NPC.Center + NPC.velocity * .5f;
				DrawDust.MakeExplosion(dustPosition, 4, dustType, 21, 3f, 8f, 50, 120, 1f, 2f, true);

				// Shoot
				//
				//int type = ProjectileID.AncientDoomProjectile;
				int type = ModContent.ProjectileType<Projectiles.BlackStar>();
				int damage = (int)(37 * ExpertDamageMultiplier);

				SoundEngine.PlaySound(SoundID.NPCDeath55, NPC.Center);
				Vector2 velocity = Mathf.VelocityFPTP(NPC.Center, new Vector2(target.Center.X, target.Center.Y), 9);
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, velocity.RotatedBy(.2f), type, damage, 3, ai0: NPC.target);
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, velocity, type, damage, 3, ai0: NPC.target);
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, velocity.RotatedBy(-.2f), type, damage, 3, ai0: NPC.target);
			}
			return AtkTimer > 140;
		}
		private bool HandleAttackAshBall()
		{
			AtkTimer++;
			if (Main.netMode != NetmodeID.MultiplayerClient && AtkTimer == 30)
			{
				// Dust
				//
				int dustType = DustID.Ash;
				Vector2 dustPosition = NPC.Center + NPC.velocity * .5f;
				for (int i = 0; i < 10; i++)
				{
					Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(NPC.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * Utils.NextFloat(Main.rand, 2f, 4f);
					Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1.75f);
					dust.noGravity = true;
				}

				// Shoot
				//
				//int type = ProjectileID.AncientDoomProjectile;
				SoundEngine.PlaySound(SoundID.NPCDeath55, NPC.Center);
				Vector2 Velocity = Mathf.VelocityFPTP(NPC.Center, new Vector2(target.Center.X, target.Center.Y), 8);
				int npc = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Projectiles.AshBall>(), Target: NPC.target);
				Main.npc[npc].velocity = Velocity;
			}

			return AtkTimer > 120;
		}
		private bool HandleAttackShootStars2()
		{
            AtkTimer++;
			if ((AtkTimer >= 90 && AtkTimer <= 135 && AtkTimer % 5 == 0) && Main.netMode != NetmodeID.MultiplayerClient)
			{
				// Shoot
				//
                int type = ModContent.ProjectileType<Projectiles.BlackStar>();
                int damage = (int)(37 * ExpertDamageMultiplier);
                Vector2 velocity = Mathf.VelocityFPTP(NPC.Center, new Vector2(target.Center.X, target.Center.Y), 8);
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, velocity, type, damage, 3, Main.myPlayer, NPC.target, 1, 3f)
					.tileCollide = true; // Make the projectile bounce between tiles
            }
			return AtkTimer > 160;
        }
        private bool HandleAttackUnholyTridents()
		{
            AtkTimer++;
			if ((AtkTimer > 30 && AtkTimer < 230) && AtkTimer % 80 == 0)
			{
                DrawDust.PentacleElectric(target.Center, Vector2.Zero, new Vector2(500, 500), DustID.Shadowflame, 50);

                // Shoot projectiles
                //
                if (Main.netMode != NetmodeID.MultiplayerClient)
				{
                    int damage = (int)(37 * ExpertDamageMultiplier);
                    int maxTridents = Main.rand.Next(4, 6);

                    for (int i = 0; i < maxTridents; i++)
                    {
                        Vector2 position = target.Center + Main.rand.NextVector2CircularEdge(500, 500);
                        Vector2 velocity = Mathf.VelocityFPTP(position, target.Center, .5f);

                        SoundEngine.PlaySound(SoundID.NPCDeath55, NPC.Center);
                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), position, velocity, ProjectileID.UnholyTridentHostile, damage, 3, ai0: NPC.target);
                    }
                }
            }
            return AtkTimer > 260;
        }
		private bool HandleAttackAshBallBarrage()
		{
            AtkTimer++;
            if (Main.netMode != NetmodeID.MultiplayerClient && AtkTimer == 30)
            {
                // Dust
                //
                int dustType = DustID.Ash;
                Vector2 dustPosition = NPC.Center + NPC.velocity * .5f;
                for (int i = 0; i < 10; i++)
                {
                    Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(NPC.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * Utils.NextFloat(Main.rand, 2f, 4f);
                    Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1.75f);
                    dust.noGravity = true;
                }

				// Shoot
				//
				for (int i = 0; i < 4; i++)
				{
                    (float sin, float cos) = MathF.SinCos(MathHelper.ToRadians(i * 360 / 4));

                    SoundEngine.PlaySound(SoundID.NPCDeath55, NPC.Center);
					Vector2 Velocity = Mathf.VelocityFPTP(NPC.Center, NPC.Center + new Vector2(sin, cos).RotatedBy(MathHelper.ToRadians(45)), 14);
					int npc = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Projectiles.AshBall>(), Target: NPC.target);
					Main.npc[npc].velocity = Velocity;
				}
            }

            return AtkTimer > 160;
        }
		private bool HandleAttackDash()
		{
			AtkTimer++;
            // Dash telegraph
			//
			if (AtkTimer > 0 && AtkTimer < 80)
            {
                int dustType = DustID.Shadowflame;
                Vector2 dustPosition = NPC.Center + NPC.velocity * .5f;
                for (int i = 0; i < 2; i++)
                {
                    Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(NPC.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * Utils.NextFloat(Main.rand, 2f, 4f);
                    Dust dust = Dust.NewDustDirect(dustPosition, NPC.width / 4, NPC.height / 4, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1.75f);
                    dust.noGravity = true;
                }
            }
            // After telegraphing preform the dash
			//
            else if (AtkTimer == 80)
			{
				const float dashSpeed = 24;

				Vector2 direction = NPC.DirectionTo(target.Center);
				Vector2 velocity = Vector2.Normalize(direction) * dashSpeed;

				NPC.velocity = velocity;
				NPC.ai[3] = SupernovaUtils.InScreenBounds(NPC, (-Vector2.One) * 30) ? 1 : 0;
			}
			// After starting the dash wait until we "collided"
			// with the screen bounds, or terrain, or until a
			// max dash time has been reached.
			//
			// QUESTION: How to check if the screen bounds are reached
			// in multiplayer? Since there will be multiple screen bounds.
			else if (Main.netMode != NetmodeID.MultiplayerClient)
			{
                // Draw dash dust
                //
                int dustType = DustID.Ash;
				Vector2 dustPosition = NPC.Center;
                for (int i = 0; i < 6; i++)
                {
					Vector2 dustVelocity = Utils.ToRotationVector2(Utils.ToRotation(NPC.velocity) + (float)Utils.ToDirectionInt(Utils.NextBool(Main.rand)) * 1.5707964f) * 8;
                    Dust dust = Dust.NewDustDirect(dustPosition, NPC.width / 6, NPC.height / 6, dustType, dustVelocity.X, dustVelocity.Y, 0, default(Color), 1.75f);
                    dust.noGravity = true;
                }

				// Detect if the NPC was off screen but has now entered the screen
				//
				if (NPC.ai[3] == 1 && !SupernovaUtils.InScreenBounds(NPC))
				{
					NPC.ai[3] = 0;
                }

				//
				if (// Check the max dash time
					(AtkTimer >= 300)
                    // Detect collision with the screen
                    || (NPC.ai[3] == 0 && !SupernovaUtils.InScreenBounds(NPC))
				)
                {
					NPC.velocity = Vector2.Zero;
					bool stoppedBecauseCollision = AtkTimer < 300;
					if (stoppedBecauseCollision)
					{
						Main.LocalPlayer.GetModPlayer<Common.Players.EffectsPlayer>().ScreenShakePower = 6;
                        SoundEngine.PlaySound(SoundID.Item70, NPC.position);
                    }

                    SoundEngine.PlaySound(SoundID.NPCDeath55, NPC.position);

                    int type = ModContent.ProjectileType<Projectiles.BlackStar>();
                    int damage = (int)(36 * ExpertDamageMultiplier);
                    ProjectileHelper.ShootPlusPattern(NPC.GetSource_FromAI(), NPC.Center, 16, 8, type, damage, 1, Main.myPlayer);
                    return true;
				}
            }
            return false;
        }

        #endregion

		private int GetActiveNPCCount(int type)
		{
			int npcCount = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].type != type) continue;
				if (!Main.npc[i].active) continue;
				npcCount++;
			}
			return npcCount;
		}

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			target.AddBuff(BuffID.Darkness, 180);
			target.AddBuff(BuffID.ShadowFlame, 240);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, DustID.Shadowflame, (float)hit.HitDirection, -1f, 0, default(Color), .75f);
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, DustID.Ash, (float)hit.HitDirection, -1f, 0, default(Color), 1f);
			}

			if (base.NPC.life <= 0)
			{
				// TODO: Death effect
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }

            SpriteEffects spriteEffects = 0;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Npc[base.NPC.type].Value;
            Vector2 origin = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[base.NPC.type] / 2));
            Vector2 npcOffset = base.NPC.Center - screenPos;
            npcOffset -= new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[base.NPC.type])) * base.NPC.scale / 2f;
            npcOffset += origin * base.NPC.scale + new Vector2(0f, base.NPC.gfxOffY);

			// TODO: Add "Get fixed boi" seed intergration
			// 
			if (Main.zenithWorld)
			{
				// TODO
			}
            spriteBatch.Draw(texture, npcOffset, new Rectangle?(base.NPC.frame), base.NPC.GetAlpha(drawColor), base.NPC.rotation, origin, base.NPC.scale, spriteEffects, 0f);
            return false;
        }
    }
}
