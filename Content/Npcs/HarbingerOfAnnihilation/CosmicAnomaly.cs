using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SupernovaMod.Common.Systems;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;

namespace SupernovaMod.Content.Npcs.HarbingerOfAnnihilation
{
	public class CosmicAnomaly : ModNPC // ModNPC is used for Custom NPCs
    {
        private float speed;
        private Player player;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cosmic Anomaly");

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x directions
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
				new FlavorTextBestiaryInfoElement("Little subjects cast away from a greater whole. Subduing them may only result in said greater whole arriving."),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 30;
            NPC.defense = 5;
            NPC.lifeMax = 75;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 100f;
            NPC.knockBackResist = 1f;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = false; // Will not collide with the tiles.
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 

            NPC.rarity = 3; // For Lifeform Analyzer
        }

        public override void AI()
        {
            NPC.rotation += (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.ToRadians(80);

            Target(); // Sets the Player Target

            DespawnHandler(); // Handles if the npc should despawn.

            Move(new Vector2(-0, -0f)); // Calls the Move Method
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
            speed = 1; // Sets the max speed of the npc.
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

            if (NPC.confused)
            {
                move = -move;
            }

            NPC.velocity = move;
        }
        private void Target()
        {
            player = Main.player[NPC.target]; // This will get the player target.
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
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneSkyHeight == true)
            {
                if (!DownedSystem.downedHarbingerOfAnnihilation)
                    return 0.03f;

                else
                    return 0.02f;
            }
            return 0;
        }

        public override void OnKill()
        {
            // Does NPC already Exist?
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<HarbingerOfAnnihilation>());
            if (!alreadySpawned)
            {
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<HarbingerOfAnnihilation>()); // Spawn the boss within a range of the player. 
                SoundEngine.PlaySound(SoundID.Roar, player.position);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.NPCDeath6);
            }

            base.OnKill();
        }
    }
}
