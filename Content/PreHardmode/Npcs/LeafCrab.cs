using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace Supernova.Content.PreHardmode.Npcs
{
    public class LeafCrab : ModNPC // ModNPC is used for Custom NPCs
    {
        private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leaf Crab");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.damage = 10;
            NPC.defense = 7;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 1000f;
            NPC.knockBackResist = 1f;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.Harpy;  //NPC behavior
            AnimationType = NPCID.Harpy;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .8F; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            if (frame >= Main.npcFrameCount[NPC.type] - 1) frame = 0;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }

        public override void AI()
        {
            Target(); // Sets the Player Target
        }

        private void Target()
        {
            player = Main.player[NPC.target]; // This will get the player target.
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (!spawnInfo.Lihzahrd && !spawnInfo.Invasion && !spawnInfo.Player.ZoneDungeon) && spawnInfo.Player.ZoneJungle && spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneBeach ? 0.15f : 0f; //100f is the spown rate so If you want your NPC to be rarer just change that value less the 100f or something.

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
            if (Main.rand.NextBool(2))
			{
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.QuarionShard>()));
			}

			base.ModifyNPCLoot(npcLoot);
		}
    }
}
