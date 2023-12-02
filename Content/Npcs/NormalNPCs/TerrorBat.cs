using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Npcs.NormalNPCs
{
    public class TerrorBat : ModNPC // ModNPC is used for Custom NPCs
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                // Influences how the NPC looks in the Bestiary
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Small but ferocious beastlings seen wandering the underground. Formed in the thick of pitch-black darkness under their master's influence."),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 17;
            NPC.defense = 8;
            NPC.lifeMax = 55;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.value = 100f;
            NPC.knockBackResist = .5f;
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = false; // Will not collide with the tiles.

            NPC.aiStyle = NPCAIStyleID.Bat;
            AIType = NPCID.CaveBat; // Will not have any AI from any existing AI styles. 
            AnimationType = NPCID.CaveBat;

            NPC.rarity = 3; // For Lifeform Analyzer
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter -= .9F; // Determines the animation speed. Higher value = faster animation.
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;

            NPC.spriteDirection = NPC.direction;
        }

        //public override float SpawnChance(NPCSpawnInfo spawnInfo) => !spawnInfo.Lihzahrd && !spawnInfo.Invasion && !spawnInfo.SpiderCave && !spawnInfo.DesertCave && !spawnInfo.Player.ZoneDungeon && spawnInfo.Player.ZoneRockLayerHeight == true ? 0.025f : 0;
        public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Cavern.Chance * 0.014f;

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			// 15% drop rate
			//
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Consumables.BugOnAStick>(), 7));

			// 1% drop rate
			//
			npcLoot.Add(ItemDropRule.Common(ItemID.DepthMeter, 100));

			// 0.4% drop rate
			//
			npcLoot.Add(ItemDropRule.Common(ItemID.BatBat, 165));

			base.ModifyNPCLoot(npcLoot);
        }

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.TerrorDust>(), hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.TerrorDust>(), hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(8))
            {
                target.AddBuff(BuffID.Rabies, Main.rand.Next(30, 80) * 60);
            }
        }
    }
}
