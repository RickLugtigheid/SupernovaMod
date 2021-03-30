using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Npcs.PreHardmode
{
    public class NightTerror : ModNPC // ModNPC is used for Custom NPCs
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Bat");
            Main.npcFrameCount[npc.type] = 2;
        }
        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 40;
            npc.damage = 20;
            npc.defense = 13;
            npc.lifeMax = 47;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100f;
            npc.knockBackResist = 1f;
            npc.noGravity = true; // Not affected by gravity
            npc.noTileCollide = false; // Will not collide with the tiles.
            npc.aiStyle = 14; // Will not have any AI from any existing AI styles. 
            animationType = NPCID.CaveBat;
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter -= .9F; // Determines the animation speed. Higher value = faster animation.
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;

            npc.spriteDirection = npc.direction;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (!spawnInfo.lihzahrd && !spawnInfo.invasion && !spawnInfo.spiderCave && !spawnInfo.desertCave && !spawnInfo.player.ZoneDungeon) && spawnInfo.player.ZoneRockLayerHeight == true ? 0.025f : 0;

        public override void NPCLoot()
        {
            // 1 in 10 chance to drop
            if (Main.rand.Next(10) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HorridChunk"));
        }
    }
}
