using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Npcs.PreHardmode
{
    public class Geist : ModNPC // ModNPC is used for Custom NPCs
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Geist");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 40;
            npc.damage = 20;
            npc.defense = 16;
            npc.lifeMax = 70;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 1200f;
            npc.knockBackResist = 2f;
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = true; // Will not collide with the tiles.
            npc.aiStyle = NPCID.Wraith; // Will not have any AI from any existing AI styles. 
            aiType = NPCID.Wraith;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;

            npc.spriteDirection = npc.direction;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (!spawnInfo.lihzahrd && !spawnInfo.invasion) && !Main.dayTime && !spawnInfo.player.ZoneRockLayerHeight ? 0.037f : 0;
    }
}
