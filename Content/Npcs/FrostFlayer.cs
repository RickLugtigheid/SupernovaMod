using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;

namespace SupernovaMod.Content.Npcs
{
    public class FrostFlayer : ModNPC
    {
        private float speed;
        private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Flayer");
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
            NPC.damage = 30;
            NPC.defense = 15;
            NPC.lifeMax = 150;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 1200f;
            NPC.knockBackResist = 1f;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles.
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
        }

        public override void AI()
        {
            NPC.rotation += (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.ToRadians(10);

            Target(); // Sets the Player Target

            DespawnHandler(); // Handles if the NPC should despawn.

            Move(new Vector2(-0, -0f)); // Calls the Move Method

            Vector2 position = NPC.Center;
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
            speed = 2.2f; // Sets the max speed of the npc.
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

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneOverworldHeight == true && spawnInfo.Player.ZoneSnow == true && NPC.downedQueenBee == true ? 0.06f : 0;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // 1 in 26 chance to drop
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.Rime>(), 26));

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
