using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Supernova.Npcs.PreHardmode
{
    public class LeafCrab : ModNPC // ModNPC is used for Custom NPCs
    {
        private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leaf Crab");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            /* Removed as of 0.10
            //npc.name = "Tutorial Zombie";
            //npc.displayName = "Tutorial Zombie";
            */
            npc.width = 18;
            npc.height = 40;
            npc.damage = 10;
            npc.defense = 7;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 1000f;
            npc.knockBackResist = 1f;
            npc.aiStyle = 3;
            aiType = NPCID.Harpy;  //npc behavior
            animationType = NPCID.Harpy;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;

            npc.spriteDirection = npc.direction;
        }

        public override void AI()
        {
            Target(); // Sets the Player Target
        }

        private void Target()
        {
            player = Main.player[npc.target]; // This will get the player target.
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (!spawnInfo.lihzahrd && !spawnInfo.invasion) && spawnInfo.player.ZoneJungle && spawnInfo.player.ZoneOverworldHeight || spawnInfo.player.ZoneBeach ? 0.15f : 0f; //100f is the spown rate so If you want your NPC to be rarer just change that value less the 100f or something.

        public override void NPCLoot()
        {
            if (Main.rand.Next(2) == 0) // For items that you want to have a chance to drop 
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("QuarionShard"));
            }
        }
    }
}
