using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Npcs.PreHardmode
{
    public class CosmicAnomaly : ModNPC // ModNPC is used for Custom NPCs
    {
        private float speed;
        private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Anomaly");
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 40;
            npc.damage = 30;
            npc.defense = 5;
            npc.lifeMax = 75;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100f;
            npc.knockBackResist = 1f;
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = false; // Will not collide with the tiles.
            npc.aiStyle = -1; // Will not have any AI from any existing AI styles. 
        }
        public override void AI()
        {
            npc.rotation += (float)System.Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + MathHelper.ToRadians(80);

            Target(); // Sets the Player Target

            DespawnHandler(); // Handles if the NPC should despawn.

            Move(new Vector2(-0, -0f)); // Calls the Move Method
        }

        private void DespawnHandler()
        {
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, -10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
        }
        private void Move(Vector2 offset)
        {
            speed = 1.2f; // Sets the max speed of the npc.
            Vector2 moveTo = player.Center; // Gets the point that the npc will be moving to.
            Vector2 move = moveTo - npc.Center;
            float magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = 24f; // The larget the number the slower the npc will turn.
            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            npc.velocity = move;
        }
        private void Target()
        {
            player = Main.player[npc.target]; // This will get the player target.
        }
        private float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 1;
            npc.frameCounter %= 20;
            int frame = (int)(npc.frameCounter / 2.0);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(spawnInfo.player.ZoneSkyHeight == true)
            {
                if(!SupernovaWorld.downedHarbingerOfAnnihilation)
                    return 0.105f;

                else
                    return 0.025f;
            }
            return 0;
        }
        public override void NPCLoot()
        {
            // Does NPC already Exist?
            bool alreadySpawned = NPC.AnyNPCs(mod.NPCType("FlyingTerror"));
            if (!alreadySpawned)
			{
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("HarbingerOfAnnihilation")); // Spawn the boss within a range of the player. 
                Main.PlaySound(SoundID.Roar, player.position, 0);
            }
        }
    }
}
