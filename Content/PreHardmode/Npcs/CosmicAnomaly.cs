using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Supernova.Common.Systems;
using Terraria.Audio;

namespace Supernova.Content.PreHardmode.Npcs
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
        }
        public override void AI()
        {
            NPC.rotation += (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + MathHelper.ToRadians(80);

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
            if(spawnInfo.Player.ZoneSkyHeight == true)
            {
                if(!SupernovaBosses.downedHarbingerOfAnnihilation)
                    return 0.105f;

                else
                    return 0.025f;
            }
            return 0;
        }

		public override void OnKill()
		{
            // Does NPC already Exist?
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<Bosses.HarbingerOfAnnihilation.HarbingerOfAnnihilation>());
            if (!alreadySpawned)
            {
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Bosses.HarbingerOfAnnihilation.HarbingerOfAnnihilation>()); // Spawn the boss within a range of the player. 
                SoundEngine.PlaySound(SoundID.Roar, player.position);
            }

            base.OnKill();
		}

		/*public override void NPCLoot()
        {
            // Does NPC already Exist?
            bool alreadySpawned = NPC.AnyNPCs(mod.NPCType("HarbingerOfAnnihilation"));
            if (!alreadySpawned)
			{
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("HarbingerOfAnnihilation")); // Spawn the boss within a range of the player. 
                Main.PlaySound(SoundID.Roar, player.position, 0);
            }
        }*/
	}
}
