using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace Supernova.Content.PreHardmode.Npcs
{
    public class NightTerror : ModNPC // ModNPC is used for Custom NPCs
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Bat");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 20;
            NPC.defense = 13;
            NPC.lifeMax = 47;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 100f;
            NPC.knockBackResist = 1f;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = false; // Will not collide with the tiles.
            NPC.aiStyle = 14; // Will not have any AI from any existing AI styles. 
            AnimationType = NPCID.CaveBat;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .9F; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (!spawnInfo.Lihzahrd && !spawnInfo.Invasion && !spawnInfo.SpiderCave && !spawnInfo.DesertCave && !spawnInfo.Player.ZoneDungeon) && spawnInfo.Player.ZoneRockLayerHeight == true ? 0.025f : 0;

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
            // 15% Drop chance
            //
            if (Main.rand.NextFloat() < .15f)
			{
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.HorridChunk>()));
            }

            base.ModifyNPCLoot(npcLoot);
		}
    }
}
