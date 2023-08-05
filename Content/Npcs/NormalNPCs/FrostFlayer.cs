using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.ModLoader.Utilities;
using SupernovaMod.Common;

namespace SupernovaMod.Content.Npcs.NormalNPCs
{
    public class FrostFlayer : ModNPC
    {
        private float speed;
        private Player player;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frost Flayer");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x directions
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("These mindless glacial constructs are formed from the souls of those unfortunate enough to perish within this frigid wonderland to frostbite. Now they're out to skin you alive and steal yours!"),
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 24;
            NPC.defense = 6;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 1200f;
            NPC.knockBackResist = .25f;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles.
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 

			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Frostburn]  = true;
            NPC.buffImmune[BuffID.Confused]   = true;
        }

        private Vector2 _dashTarget;
        public override void AI()
        {
			player = Main.player[NPC.target]; // Sets the Player Target

            DespawnHandler(); // Handles if the NPC should despawn.

            if (NPC.localAI[0] > 110 || Vector2.Distance(NPC.Center, player.Center) < 600)
            {
                NPC.localAI[0]++;
                if (NPC.localAI[0] >= 120)
                {
                    //if (NPC.localAI[0] == 140)
                    {
                        NPC.knockBackResist = 0;
						if (NPC.Center.X < player.Center.X - 2f)
						{
							NPC.direction = 1;
						}
						if (NPC.Center.X > player.Center.X + 2f)
						{
							NPC.direction = -1;
						}
						NPC.spriteDirection = NPC.direction;

						_dashTarget = player.Center;
                    }

					NPC.rotation += MathHelper.ToRadians(25);

                    Vector2 dustVelocity = NPC.velocity.RotatedBy(NPC.rotation);
					Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.IceTorch, dustVelocity.X, dustVelocity.Y);

                    if (NPC.localAI[0] > 140 && NPC.localAI[0] < 150)
                    {
						float gateValue = 100f;
						Vector2 distanceFromTarget = _dashTarget - NPC.Center;
						SupernovaUtils.MoveNPCSmooth(NPC, gateValue, distanceFromTarget, 20, 1, true);
					}	

					if (NPC.localAI[0] > 190)
                    {
                        NPC.localAI[0] = 0;
						NPC.knockBackResist = .25f;
					}
					return;
				}
			}
            else
            {
				NPC.localAI[0] = 0;
			}

			Move(Vector2.Zero); // Calls the Move Method
            NPC.rotation = NPC.GetTargetLookRotation(player.Center);
		}

		private void DespawnHandler()
        {
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0f, -10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
        }

        private void Move(Vector2 offset)
        {
            speed = 1.2f; // Sets the max speed of the npc.
            Vector2 moveTo = player.Center; // Gets the point that the npc will be moving to.
            Vector2 move = moveTo - NPC.Center;
            float magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = 24f; // The larget the number the slower the npc will turn.
            move = (NPC.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            NPC.velocity = move;
        }

        private float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1;
            NPC.frameCounter %= 20;
            int frame = (int)(NPC.frameCounter / 2.0);
            if (frame >= Main.npcFrameCount[NPC.type]) frame = 0;
            NPC.frame.Y = frame * frameHeight;
        }

        //public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneOverworldHeight == true && spawnInfo.Player.ZoneSnow == true && NPC.downedQueenBee == true ? 0.06f : 0;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.ZoneSnow || spawnInfo.PlayerSafe || !NPC.downedQueenBee || spawnInfo.Player.ZoneDungeon || spawnInfo.PlayerInTown || spawnInfo.Player.ZoneOldOneArmy || Main.snowMoon || Main.pumpkinMoon)
            {
                return 0f;
            }
            return 0.015f;
        }

		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceTorch, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.IceTorch, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.Rime>(), 4, maximumDropped: 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.IceBlock, 2, 0, 10));

            base.ModifyNPCLoot(npcLoot);
        }

        public override void OnKill()
        {
            SoundEngine.PlaySound(SoundID.NPCDeath6);
            base.OnKill();
        }
    }
}
