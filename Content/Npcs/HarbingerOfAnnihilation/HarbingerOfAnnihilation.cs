using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using SupernovaMod.Common;
using Terraria.GameContent;
using SupernovaMod.Content.Npcs.HarbingerOfAnnihilation.Projectiles;
using Filters = Terraria.Graphics.Effects.Filters;

namespace SupernovaMod.Content.Npcs.HarbingerOfAnnihilation
{
    [AutoloadBossHead]
    public class HarbingerOfAnnihilation : ModNPC
    {
        protected float npcLifeRatio;
        public Player target;

		private const int DAMAGE_PROJ_MISSILE = 18;
		private const int DAMAGE_PROJ_ORB = 31;

		private readonly int _projIdMissile = ModContent.ProjectileType<HarbingerMissile>();

        private readonly HarbingerOfAnnihilation_Arm[] _arms = new HarbingerOfAnnihilation_Arm[4];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbinger of Annihilation");
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("An enigmatic subsistence. This stellar entity with a seemingly inexplicable nature all the way back to its formation is tasked with one thing and one thing only - to destitute all planets of their lifeforms."),
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            NPC.lifeMax = 2600; // The Max HP the boss has on Normal
            NPC.damage = 25; // The base damage value the boss has on Normal
            NPC.defense = 8; // The base defense on Normal
            NPC.knockBackResist = 0f; // No knockback
            NPC.width = 68;
            NPC.height = 68;
            NPC.value = 10000;
            NPC.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            NPC.boss = true; // Is a boss
            NPC.lavaImmune = true; // Not hurt by lava
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicID.Boss1;

			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
        }

        protected float velocity;
        protected float acceleration;
        public bool SecondPhase { get; private set; } = false;

		private int _attackPointer2 = 0;

		public override void AI()
        {
            // Run this method once
            //
            if (_arms[0] == null && Main.netMode != NetmodeID.MultiplayerClient)
            {
				int deg = 360 / _arms.Length;
				for (int i = 0; i < _arms.Length; i++)
                {
					int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, 0, 0, ModContent.ProjectileType<HarbingerOfAnnihilation_Arm>(), 20, 5, Main.myPlayer, ai0: NPC.whoAmI, ai1: deg * i);
                    _arms[i] = Main.projectile[proj].ModProjectile as HarbingerOfAnnihilation_Arm;
				}
			}

			// Set/Reset all required values
			//
			npcLifeRatio = NPC.life / (float)NPC.lifeMax;

            ref float timer = ref NPC.ai[1];
            ref float attackPointer = ref NPC.ai[0];
            target = Main.player[NPC.target];

            // Handle AI
            //
            if (DespawnAI())
			{
				return; // Don't run any other AI
			}
			if (DeathAI())
			{
				return; // Don't run any other AI
			}

			timer++;

            // First phase
            //
			if (npcLifeRatio > .5)
			{
				velocity = Main.masterMode ? 7.5f : 5;
				acceleration = .04f;

				ref float index = ref NPC.ai[2];

				if (attackPointer == 0 || attackPointer == 3)
				{
					if (timer == 90 || timer == 100 || timer == 110 || timer == 120)
					{
						// Set a random arm to shoot to the player
						_arms[(int)index].Projectile.ai[0] = HoaArmAI.LaunchAtPlayer;
						index++;
					}
					if (timer >= 400 && WaitForAllArmsToReturn())
					{
						index = 0;
						timer = 0;
						attackPointer++;
					}
				}
				else if (attackPointer == 1)
				{
					ref float attackCount = ref NPC.ai[2];

					if (timer == 1)
					{
						attackCount = 1;

						if (npcLifeRatio < .80)
							attackCount++;
						if (npcLifeRatio < .60)
							attackCount++;

						if (Main.masterMode || Main.expertMode)
							attackCount++;
					}

					if (attackCount > 0 && timer % 80 == 0)
					{
						GetRandomArm().Projectile.ai[0] = HoaArmAI.SmashPlayer;
						attackCount--;
					}

					/*if (timer == 40 && (Main.expertMode || Main.masterMode))
                    {
						GetRandomArm().Projectile.ai[0] = 2;
					}
					if (timer == 120)
                    {
                        GetRandomArm().Projectile.ai[0] = 2;
					}*/

					if (timer >= 380 && WaitForAllArmsToReturn())
					{
						index = 0;
						timer = 0;
						attackPointer++;
					}
				}
				else if (attackPointer == 2)
				{
					if (timer == 1)
					{
						ForeachArm(arm => arm.Projectile.ai[0] = HoaArmAI.CirclePlayerAndShoot);
					}
					if (timer >= 420 && WaitForAllArmsToReturn())
					{
						index = 0;
						timer = 0;
						attackPointer++;
					}
				}
				else if (attackPointer == 4)
                {
                    AttackCastOrb(ref timer, ref attackPointer);
					return;
				}
				else
				{
					attackPointer = 0;
				}

				MovementAI(GetDesiredDestination(), velocity, acceleration);
			}
			else if (!SecondPhase)
			{
				NPC.localAI[0]++;

                if (NPC.localAI[0] == 1)
                {
					ForeachArm(arm => {
						arm.Projectile.alpha = 250;
						arm.Projectile.hostile = false;
						arm.AttackPointer = HoaArmAI.Reset;
					});
                    NPC.dontTakeDamage = true;
				}

                if (NPC.localAI[0] < 140)
                {
					NPC.velocity = Vector2.Zero;
					NPC.rotation += (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.ToRadians(10);

					// Charge dust effect
					//
					SoundEngine.PlaySound(SoundID.Item15);
					Vector2 dustPos = NPC.Center + new Vector2(120, 0).RotatedByRandom(MathHelper.ToRadians(360));
					Vector2 diff = NPC.Center - dustPos;
					diff.Normalize();

					Dust.NewDustPerfect(dustPos, DustID.UndergroundHallowedEnemies, diff * 10).noGravity = true;
					Dust.NewDustPerfect(dustPos, DustID.Vortex, diff * 10).noGravity = true;
				}
				else if (NPC.localAI[0] == 140)
				{
					NPC.rotation = 0;
				}

				// End animation
				//
				if (NPC.localAI[0] > 160 && WaitForAllArmsToReturn())
				{
                    NPC.localAI[0] = 0;
                    NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.ai[2] = 0;
					SecondPhase = true;
                    NPC.dontTakeDamage = false;
					ForeachArm(arm => {
						arm.Projectile.alpha = 0;
						arm.Projectile.hostile = true;
					});
				}
			}
			else
			{
                bool move = true;

				if (_attackPointer2 == 0 || _attackPointer2 == 1)
				{
					if (attackPointer == 0)
					{
						AttackCastOrb(ref timer, ref attackPointer, 1.25f);
						return;
					}
					else if (attackPointer == 1 && _attackPointer2 == 0)
					{
						ref float direction = ref NPC.ai[2];

						if ((NPC.position.X - target.position.X) > 0)
						{
							direction = 1;
						}
						else
						{
							direction = -1;
						}

						if (timer < 400)
						{
							velocity = 15;
							acceleration = .1f;

							NPC.rotation += MathHelper.ToRadians(15) * direction;
							NPC.rotation = NPC.rotation % 360;
						}
						else
						{
							NPC.velocity = Vector2.Zero;
							NPC.rotation = 0;

							if (timer > 520 && NPC.rotation == 0 && WaitForAllArmsToReturn())
							{
								timer = 0;
								direction = 0;
								attackPointer++;
							}
						}
						MovementAI(GetDesiredDestinationDirect(), velocity, acceleration);
					}
					else if (attackPointer == 1)
					{
						MovementAI(GetDesiredDestination(), velocity, acceleration);
						ref float index = ref NPC.ai[2];

						if (timer == 90 || timer == 100 || timer == 110 || timer == 120)
						{
							// Set a random arm to shoot to the player
							_arms[(int)index].Projectile.ai[0] = HoaArmAI.LaunchAtPlayer;
							index++;
						}
						if (timer >= 400 && WaitForAllArmsToReturn())
						{
							index = 0;
							timer = 0;
							attackPointer++;
						}
					}
					else if (attackPointer == 2 || attackPointer == 3)
					{
						if (_attackPointer2 == 0)
						{
							AttackLightningArms(ref timer, ref attackPointer);
						}
						else if (attackPointer == 2)
						{
							AttackBlackHole(ref timer, ref attackPointer);
						}
						else if (attackPointer == 3)
							attackPointer++;
					}
					else
					{
						timer = 0;
						attackPointer = 0;
						_attackPointer2 = Main.rand.Next(3);
					}
				}
				else
				{
					if (attackPointer == 0)
					{
						NPC.velocity = Vector2.Zero;
						ref float attackCount = ref NPC.ai[2];

						if (timer == 1)
						{
							attackCount = 3;
							if (Main.masterMode || Main.expertMode)
								attackCount++;
						}

						if (attackCount > 0 && timer % 80 == 0)
						{
							GetRandomArm().Projectile.ai[0] = HoaArmAI.SmashPlayer;
							attackCount--;
						}

						if (timer >= 380 && WaitForAllArmsToReturn())
						{
							timer = 0;
							attackPointer++;
						}
					}
					else if (attackPointer == 1)
					{
						MovementAI(GetDesiredDestination(), velocity, acceleration);

						if (timer > 120)
						{
							timer = 0;
							attackPointer++;
						}
					}
					else if (attackPointer == 2 || attackPointer == 3)
					{
						if (_attackPointer2 == 0)
						{
							AttackLightningArms(ref timer, ref attackPointer);
						}
						else if (attackPointer == 2)
						{
							AttackBlackHole(ref timer, ref attackPointer);
						}
						else if (attackPointer == 3)
							attackPointer++;
					}
					else
					{
						timer = 0;
						attackPointer = 0;
						_attackPointer2 = Main.rand.Next(3);
					}
				}
				//if (move) MovementAI(GetDesiredDestination(), velocity, acceleration);
				return;
				if (attackPointer == 0 || attackPointer == 1 || attackPointer == 3 || attackPointer == 4)
                {
                    if (timer >= 80)
                    {
						NPC.alpha += 2;
						ForeachArm(arm => arm.Projectile.alpha += 2);
						if (NPC.alpha >= 255)
						{
							SpawnPortal();
                            NPC.position = target.position + (50 * NPC.velocity);
                            NPC.alpha = 0;
							ForeachArm(arm => arm.Projectile.alpha = 0);

							timer = 0;
                            attackPointer++;
                        }
					}
                }
                else if (attackPointer == 2)
                {
					if (timer == 1)
					{
						ForeachArm(arm => arm.Projectile.ai[0] = 1);
					}
					if (timer >= 420)
					{
						timer = 0;
						attackPointer++;
					}
				}
				else if (attackPointer == 5)
				{
					if (timer == 1)
					{
						ForeachArm(arm => arm.Projectile.ai[0] = 3);
					}
					if (timer >= 420)
					{
						timer = 0;
						attackPointer++;
					}
				}
				else
                {
                    attackPointer = 0;
                }

                if (move) MovementAI(target.position, velocity, acceleration);
			}
        }

		#region AI Methods
		public override bool CheckDead()
		{
			if (NPC.ai[3] == 0f)
			{
				NPC.ai[3] = 1f;
				NPC.damage = 0;
				NPC.life = NPC.lifeMax;
				NPC.dontTakeDamage = true;
				NPC.netUpdate = true;
				return false;
			}
			return true;
		}

		private int rippleCount = 3;
		private int rippleSize = 5;
		private int rippleSpeed = 30;
		private float distortStrength = 200f;
		private bool DeathAI()
		{
			if (NPC.ai[3] == 1)
			{
				NPC.velocity = Vector2.Zero;
			}
			if (NPC.ai[3] > 0)
			{
				ForeachArm(arm => arm.Projectile.Kill());
				NPC.dontTakeDamage = true;
				NPC.ai[3]++; // increase our death timer.
								 //npc.velocity = Vector2.UnitY * npc.velocity.Length();
				/*NPC.velocity.X *= 0.95f; // lose inertia
				if (NPC.velocity.Y < 0.5f)
				{
					NPC.velocity.Y = NPC.velocity.Y + 0.02f;
				}
				if (NPC.velocity.Y > 0.5f)
				{
					NPC.velocity.Y = NPC.velocity.Y - 0.02f;
				}*/

				if (NPC.ai[3] > 150 && Main.netMode != NetmodeID.Server)
				{
					if (Filters.Scene["Shockwave"].IsActive())
					{
						float progress = (180 - NPC.ai[3]) / 40;
						Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
					}
					else
					{
						Filters.Scene.Activate("Shockwave", NPC.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(NPC.Center);
					}
				}

				if (NPC.ai[3] > 120)
				{
					NPC.Opacity = 1f - (NPC.ai[3] - 120f) / 60f;
				}
				if (NPC.ai[3] < 180)
				{
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
					NPC.velocity = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi).ToRotationVector2();

					// Charge dust effect
					//
					SoundEngine.PlaySound(SoundID.Item15);
					Vector2 dustPos = NPC.Center + new Vector2(120, 0).RotatedByRandom(MathHelper.ToRadians(360));
					Vector2 diff = NPC.Center - dustPos;
					diff.Normalize();

					Dust.NewDustPerfect(dustPos, DustID.UndergroundHallowedEnemies, diff * 10).noGravity = true;
					Dust.NewDustPerfect(dustPos, DustID.Vortex, diff * 10).noGravity = true;
				}

				if (NPC.ai[3] % 60f == 1f)
				{
					//SoundEngine.PlaySound(4, npc.Center, 22);
					SoundEngine.PlaySound(SoundID.NPCDeath22, NPC.Center); // every second while dying, play a sound
				}
				if (NPC.ai[3] >= 180)
				{
					if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
					{
						Filters.Scene["Shockwave"].Deactivate();
					}

					SoundEngine.PlaySound(SoundID.Item14);

					for (int num925 = 0; num925 < 4; num925++)
					{
						int num915 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Vortex, 0f, 0f, 100, default, 1.5f);
						Main.dust[num915].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
					}
					for (int num924 = 0; num924 < 30; num924++)
					{
						int num917 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.UndergroundHallowedEnemies, 0f, 0f, 200, default, 3.7f);
						Main.dust[num917].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
						Main.dust[num917].noGravity = true;
						Dust dust24 = Main.dust[num917];
						dust24.velocity *= 3f;
						num917 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Vortex, 0f, 0f, 100, default, 1.5f);
						Main.dust[num917].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
						dust24 = Main.dust[num917];
						dust24.velocity *= 2f;
						Main.dust[num917].noGravity = true;
						Main.dust[num917].fadeIn = 2.5f;
					}
					for (int i = 0; i < 25; i++)
					{
						Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(-40, 40), 0).RotatedByRandom(MathHelper.ToRadians(360));
						Vector2 diff = NPC.Center - dustPos;
						diff.Normalize();

						Dust.NewDustPerfect(dustPos, DustID.UndergroundHallowedEnemies, -diff * 20, Scale: Main.rand.Next(1, 4)).noGravity = true;
						Dust.NewDustPerfect(dustPos, DustID.Vortex, -diff * 20, Scale: Main.rand.Next(1, 4)).noGravity = true;
					}
					for (int num921 = 0; num921 < 2; num921++)
					{
						Vector2 position16 = NPC.position + new Vector2(NPC.width * Main.rand.Next(100) / 100f, NPC.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f;
						Vector2 vector33 = default;
						int num920 = Gore.NewGore(NPC.GetSource_Death(), position16, vector33, Main.rand.Next(61, 64), 1f);
						Main.gore[num920].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
						Gore gore2 = Main.gore[num920];
						gore2.velocity *= 0.3f;
						Gore expr_79A3_cp_0 = Main.gore[num920];
						expr_79A3_cp_0.velocity.X = expr_79A3_cp_0.velocity.X + Main.rand.Next(-10, 11) * 0.05f;
						Gore expr_79D3_cp_0 = Main.gore[num920];
						expr_79D3_cp_0.velocity.Y = expr_79D3_cp_0.velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
					}

					NPC.life = 0;
					NPC.HitEffect(0, 0);
					NPC.checkDead(); // This will trigger ModNPC.CheckDead the second time, causing the real death.
				}
				return true;
			}
			return false;
		}
        private bool DespawnAI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
            }

            Player player = Main.player[NPC.target];
            if (player.dead || !player.active)
            {
                NPC.TargetClosest(true);
                player = Main.player[NPC.target];
                if (player.dead || !player.active)
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
					if (NPC.ai[3] == 1)
					{
						ForeachArm(arm => arm.AttackPointer = HoaArmAI.CircleHoa);
					}
					else if (NPC.ai[3] < 180)
					{
						
						// Charge dust effect
						//
						SoundEngine.PlaySound(SoundID.Item15);
						Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(110, 130), 0).RotatedByRandom(MathHelper.ToRadians(360));
						Vector2 diff = NPC.Center - dustPos;
						diff.Normalize();

						Dust.NewDustPerfect(dustPos, DustID.UndergroundHallowedEnemies, diff * 10, Scale: 2).noGravity = true;
						Dust.NewDustPerfect(dustPos, DustID.Vortex, diff * 10, Scale: 2).noGravity = true;
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

        private void MovementAI(Vector2 destination, float velocity, float acceleration)
        {
            float gateValue = 100f;
            Vector2 distanceFromTarget = new Vector2(destination.X, destination.Y) - NPC.Center;
            SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, velocity, acceleration, true);

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

        private Vector2 _desiredProjectileDestination;
		private Vector2 _desiredDestination;
		private Vector2 GetDesiredDestinationDirect()
        {
			if (target.position == Vector2.Zero)
			{
				return target.position;
			}
			if (_desiredDestination == Vector2.Zero ||
				Vector2.Distance(target.position, _desiredDestination) >= 350 ||    // Make sure the boss will not get to far from the player
				Vector2.Distance(NPC.position, _desiredDestination) <= 50           // Pick a new target destination when in 25 blocks of the prev target
			)
			{
				_desiredDestination = target.position;

                Vector2 extra = target.position - NPC.position;
				extra.Normalize();
				_desiredDestination += extra * target.velocity;
			}
			return _desiredDestination;
		}
		private Vector2 GetDesiredDestination()
		{
            if (target.position == Vector2.Zero)
            {
                return target.position;
            }

			if (_desiredDestination == Vector2.Zero || 
                Vector2.Distance(target.position, _desiredDestination) >= 500 ||    // Make sure the boss will not get to far from the player
				Vector2.Distance(target.position, _desiredDestination) <= 80 ||     // Make sure the boss will not get to close to the player (this may limit player movement options)
				Vector2.Distance(NPC.position, _desiredDestination) <= 50           // Pick a new target destination when in 50 blocks of the prev target
			)
			{
                _desiredDestination = target.position;

				if (Main.rand.NextBool())
                {
                    _desiredDestination.X += Main.rand.Next(0, 250) * -NPC.direction;
				}
                else
                {
					_desiredDestination.Y += Main.rand.Next(0, 250) * -NPC.direction;
				}
			}
			return _desiredDestination;
		}
		#endregion

		#region Attack Methods

		void AttackLightningArms(ref float timer, ref float attackPointer)
		{
			if (attackPointer == 2)
			{
				ref float direction = ref NPC.localAI[0];
				if (timer < 300)
				{
					if (direction == 0)
					{
						direction = Main.rand.NextBool() ? 1 : -1;
					}
					if (timer <= 221)
					{
						_desiredProjectileDestination = target.Center - new Vector2(0, 500 * direction);
					}
					if (timer == 222)
					{
						_desiredProjectileDestination += new Vector2(0, 1000 * direction);
					}

					if (timer < 380)
					{
						HarbingerOfAnnihilation_Arm arm = _arms[0];
						arm.customTarget = _desiredProjectileDestination - new Vector2(200, 0) * direction;
						arm.customDuration = 380;
						arm.Projectile.ai[0] = HoaArmAI.LightningLink;
						arm.Projectile.ai[1] = _arms[1].Projectile.whoAmI;

						arm = _arms[1];
						arm.customTarget = _desiredProjectileDestination + new Vector2(200, 0) * direction;
						arm.customDuration = 380;
						arm.Projectile.ai[0] = HoaArmAI.LightningLink;
						arm.Projectile.ai[1] = _arms[0].Projectile.whoAmI;
					}
				}
				else
				{
					timer = 0;
					direction = 0;
					attackPointer++;
				}
			}
			else if (attackPointer == 3)
			{
				ref float direction = ref NPC.localAI[0];
				if (timer < 480)
				{
					if (direction == 0)
					{
						direction = Main.rand.NextBool() ? 1 : -1;
					}
					if (timer <= 221)
					{
						_desiredProjectileDestination = target.Center - new Vector2(500 * direction, 0);
					}
					if (timer == 222)
					{
						_desiredProjectileDestination += new Vector2(1000 * direction, 0);
					}

					if (timer < 380)
					{
						HarbingerOfAnnihilation_Arm arm = _arms[2];
						arm.customTarget = _desiredProjectileDestination - new Vector2(0, 200) * direction;
						arm.customDuration = 380;
						arm.Projectile.ai[0] = HoaArmAI.LightningLink;
						arm.Projectile.ai[1] = _arms[3].Projectile.whoAmI;

						arm = _arms[3];
						arm.customTarget = _desiredProjectileDestination + new Vector2(0, 200) * direction;
						arm.customDuration = 380;
						arm.Projectile.ai[0] = HoaArmAI.LightningLink;
						arm.Projectile.ai[1] = _arms[2].Projectile.whoAmI;
					}
				}
				else if (WaitForAllArmsToReturn())
				{
					timer = 0;
					direction = 0;
					attackPointer++;
				}
			}
		}
		void AttackBlackHole(ref float timer, ref float attackPointer)
		{
			ref float isAtTarget = ref NPC.localAI[0];
			ref float angle = ref NPC.localAI[1];

			if (isAtTarget == 1)
			{
				// Play Charge effect
				//
				if (timer < 90)
				{
					NPC.velocity = Vector2.Zero;
					NPC.rotation = 0;

					if (timer == 20)
					{
						SoundEngine.PlaySound(SoundID.ForceRoar, NPC.Center);
					}
					Vector2 dustPos = NPC.Center + new Vector2(Main.rand.Next(115, 125), 0).RotatedByRandom(MathHelper.ToRadians(360));
					Vector2 diff = NPC.Center - dustPos;
					diff.Normalize();

					Dust.NewDustPerfect(dustPos, DustID.Demonite, diff * 10, Scale: 2).noGravity = true;
					Dust.NewDustPerfect(dustPos, DustID.Corruption, diff * 10, Scale: 2).noGravity = true;
					return;
				}

				if (timer == 90)
				{
					int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.HoaBlackHole>(), 40, 6, Main.myPlayer, 1200, .25f);
					Main.projectile[proj].timeLeft = 440;
					ForeachArm(arm =>
					{
						arm.AttackPointer = HoaArmAI.CircleTarget;
						arm.customTarget = Main.projectile[proj].Center;
						arm.customDuration = 440;
					});
				}
				else if (timer > 120)
				{
					velocity = Main.masterMode ? 7.5f : 5;
					acceleration = .04f;
					MovementAI(GetDesiredDestination(), velocity, acceleration);
					if (timer % 75 == 0)
					{
						ShootToPlayer(_projIdMissile, DAMAGE_PROJ_MISSILE, 1.2f, Main.rand.NextFloat(.995f, 1.05f));
					}
				}

				if (timer >= 580)
				{
					timer = 0;
					isAtTarget = 0;
					attackPointer++;
				}
			}
			else
			{
				if (Vector2.Distance(NPC.Center, target.Center - new Vector2(0, 250)) <= 75)
				{
					isAtTarget = 1;
				}
				timer = 0;
				MovementAI(target.Center - new Vector2(0, 250), velocity * 1.5f, .2f);
			}
		}

		void AttackCastOrb(ref float timer, ref float attackPointer, float timeLeftMulti = 1)
        {
			NPC.velocity = Vector2.Zero;

			if ((NPC.Center.X - target.Center.X) > 0)
			{
				NPC.direction = -1;
			}
			else
			{
				NPC.direction = 1;
			}

			Vector2 lookTarget = NPC.Center + (new Vector2(100, 0) * NPC.direction);
			if (timer == 50)
			{
				HarbingerOfAnnihilation_Arm arm = _arms[0];
				arm.customTarget = NPC.Center + (new Vector2(100, 100) * NPC.direction);
				arm.customLookTarget = lookTarget;
				arm.Projectile.ai[0] = HoaArmAI.GotoTarget;

				arm = _arms[1];
				arm.customTarget = NPC.Center + (new Vector2(50, 50) * NPC.direction);
				arm.customLookTarget = lookTarget;
				arm.Projectile.ai[0] = HoaArmAI.GotoTarget;

				arm = _arms[2];
				arm.customTarget = NPC.Center + (new Vector2(50, -50) * NPC.direction);
				arm.customLookTarget = lookTarget;
				arm.Projectile.ai[0] = HoaArmAI.GotoTarget;

				arm = _arms[3];
				arm.customTarget = NPC.Center + (new Vector2(100, -100) * NPC.direction);
				arm.customLookTarget = lookTarget;
				arm.Projectile.ai[0] = HoaArmAI.GotoTarget;
			}

			if (timer == 50)
			{
				Projectile proj = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), lookTarget, Vector2.Zero, ModContent.ProjectileType<HarbingerOrb>(), DAMAGE_PROJ_ORB, 4, Main.myPlayer)];
				proj.timeLeft = (int)(proj.timeLeft * timeLeftMulti);
			}

			if (timer >= 220 && WaitForAllArmsToReturn())
			{
				timer = 0;
				attackPointer++;
			}
		}

		void AttackTripleStrike(ref float timer, ref float attackPointer)
        {
            ref float direction = ref NPC.ai[2];
            if (timer >= 220)
            {
				velocity = Main.masterMode ? 50 : Main.expertMode ? 40 : 30;

				acceleration = .3f;
            }
            if (timer >= 220 && timer <= 300)
            {
                if (direction == 0)
                {
                    direction = -target.direction;
                }

                SetNPCRotation(-45 * direction);
                //targetOffset.X = (200 + target.velocity.X) * direction;
                //targetOffset.X -= target.velocity.X * direction;

                if (timer == 240)
                {
					ShootPatternCross(22);
                }
            }
            else if (timer >= 340 && timer <= 420)
            {
                SetNPCRotation(0);

                if (timer == 360)
                {
					ShootPatternPlus(22);
                }
            }
            else if (timer >= 460 && timer <= 520)
            {
                SetNPCRotation(45 * direction);
                //targetOffset.X = (200 + target.velocity.X) * -direction;

                if (timer == 500)
                {
                    ShootPatternCross(22);
                }
            }
            else if (timer > 520)
            {
                SetNPCRotation(0);

                // Wait for the npc rotation to be 0 before ending this attack
                //
                if (NPC.rotation == 0)
                {
                    timer = 0;
                    direction = 0;
                    attackPointer++;
                }
            }
        }
		#endregion

		#region Helper Methods
		private void ShootToPlayer(int type, int damage, float velocityMulti = 1, float rotationMulti = 1)
		{
			//int type = ModContent.ProjectileType<TerrorProj>();
			SoundEngine.PlaySound(SoundID.Item20, NPC.Center);

			Vector2 position = NPC.Center;
			float rotation = (float)Math.Atan2(position.Y - (target.position.Y + target.height * 0.2f), position.X - (target.position.X + target.width * 0.15f));
			rotation *= rotationMulti;

			Vector2 velocity = new Vector2((float)-(Math.Cos(rotation) * 18) * .75f, (float)-(Math.Sin(rotation) * 18) * .75f) * velocityMulti;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, damage, 0f, 0);

			for (int x = 0; x < 5; x++)
			{
				int dust = Dust.NewDust(position, NPC.width, NPC.height, DustID.UndergroundHallowedEnemies, velocity.X / 2, velocity.Y / 2, 80, default, Main.rand.NextFloat(.9f, 1.6f));
				Main.dust[dust].noGravity = true;
			}
		}
		private void ForeachArm(Action<HarbingerOfAnnihilation_Arm> action)
        {
			for (int i = 0; i < _arms.Length; i++)
            {
                action(_arms[i]);
			}
		}
        private HarbingerOfAnnihilation_Arm GetRandomArm(bool getInactiveOnly = false)
        {
			HarbingerOfAnnihilation_Arm randArm = _arms[Main.rand.Next(0, _arms.Length)];
            if (!getInactiveOnly || randArm.Projectile.ai[0] == 0)
            {
                return randArm;
            }
            return GetRandomArm();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="timer"></param>
		/// <returns>If all arms have returned to thier start position</returns>
		private bool WaitForAllArmsToReturn()
		{
			for (int i = 0; i < _arms.Length; i++)
			{
				// Check if the arm has the idle AI
				//
				if (_arms[i].AttackPointer != HoaArmAI.CircleHoa)
				{
					return false;
				}
			}
			return true;
		}
        void SpawnPortal()
        {
            /*int portalId = ModContent.ProjectileType<HarbingerMeteorPortal>();

			if (Main.rand.NextBool())
            {
                portalId = ModContent.ProjectileType<HarbingerBeamPortal>();
			}

			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position, Vector2.Zero, portalId, 35, 0f, ai0: 120, ai1: NPC.target);*/
		}
		void ShootPatternCross(int damage, float knockback = 4)
        {
            const int ShootDirection = 9;

            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -ShootDirection, -ShootDirection, _projIdMissile, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, ShootDirection, -ShootDirection, _projIdMissile, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -ShootDirection, ShootDirection, _projIdMissile, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, ShootDirection, ShootDirection, _projIdMissile, damage, knockback, Main.myPlayer, 0f, 0f);
        }
        void ShootPatternPlus(int damage, float knockback = 4)
        {
            const int ShootDirection = 9;

            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -ShootDirection, 0, _projIdMissile, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, ShootDirection, 0, _projIdMissile, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, ShootDirection, _projIdMissile, damage, knockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, -ShootDirection, _projIdMissile, damage, knockback, Main.myPlayer, 0f, 0f);
        }

        private float _rotateSpeed = MathHelper.ToRadians(5);
        private void SetNPCRotation(float degrees)
        {
            float targetRotation = MathHelper.ToRadians(degrees);

            if (NPC.rotation == targetRotation)
            {
                return;
            }

            if (NPC.rotation > targetRotation)
            {
                if (NPC.rotation + _rotateSpeed > targetRotation)
                {
                    NPC.rotation -= _rotateSpeed;
                }
                else
                {
                    NPC.rotation = targetRotation;
                }
            }

            else if (NPC.rotation < targetRotation)
            {
                if (NPC.rotation + _rotateSpeed < targetRotation)
                {
                    NPC.rotation += _rotateSpeed;
                }
                else
                {
                    NPC.rotation = targetRotation;
                }
            }
        }
		#endregion

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UndergroundHallowedEnemies, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = 0;
            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Color color24 = NPC.GetAlpha(drawColor);
            Color color25 = Lighting.GetColor((int)(NPC.position.X + NPC.width * 0.5) / 16, (int)((NPC.position.Y + NPC.height * 0.5) / 16.0));
            Texture2D texture2D3 = TextureAssets.Npc[NPC.type].Value;
            int num156 = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            int y3 = num156 * (int)NPC.frameCounter;
            Rectangle rectangle = new Rectangle(0, y3, texture2D3.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            int num157 = 8;
            int num158 = 2;
            int num159 = 1;
            float num160 = 0f;
            int num161 = num159;
            spriteBatch.Draw(texture2D3, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), new Rectangle?(NPC.frame), color24, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 0f);
            while (num158 > 0 && num161 < num157 || num158 < 0 && num161 > num157)
            {
                Color color26 = NPC.GetAlpha(color25);
                float num162 = num157 - num161;
                if (num158 < 0)
                {
                    num162 = num159 - num161;
                }
                color26 *= num162 / (NPCID.Sets.TrailCacheLength[NPC.type] * 1.5f);
                Vector2 value4 = NPC.oldPos[num161];
                float num163 = NPC.rotation;
                Main.spriteBatch.Draw(texture2D3, value4 + NPC.Size / 2f - screenPos + new Vector2(0f, NPC.gfxOffY), new Rectangle?(rectangle), color26, num163 + NPC.rotation * num160 * (num161 - 1) * -(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt(), origin2, NPC.scale, spriteEffects, 0f);
                num161 += num158;
            }
			return NPC.IsABestiaryIconDummy;
		}

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);
        }
    }
}
