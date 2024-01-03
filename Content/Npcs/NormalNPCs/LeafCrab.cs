using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SupernovaMod.Common.ItemDropRules.DropConditions;

namespace SupernovaMod.Content.Npcs.NormalNPCs
{
    public class LeafCrab : ModNPC // ModNPC is used for Custom NPCs
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Leaf Crab");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.SpecificDebuffImmunity[NPC.type][BuffID.Poisoned] = true;
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
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,


				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Peculiar little crustaceans with an inherently aggressive nature to other sentient objects outsizing it. Despite what their appearance suggests, they aren't true crabs. These hostile critters are coveted for a special piece of their exoskeleton."),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 36;
            NPC.damage = 20;
            NPC.defense = 10;
            NPC.lifeMax = 45;
            NPC.HitSound = SoundID.NPCHit33;
            NPC.DeathSound = SoundID.NPCDeath36;
            NPC.value = 1000f;
            NPC.knockBackResist = .65f;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.Crab;  //NPC behavior
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

        //public override float SpawnChance(NPCSpawnInfo spawnInfo) => !spawnInfo.Lihzahrd && !spawnInfo.Invasion && !spawnInfo.Player.ZoneDungeon && spawnInfo.Player.ZoneJungle && spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneBeach ? 0.15f : 0f; //100f is the spown rate so If you want your NPC to be rarer just change that value less the 100f or something.
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            /*if (spawnInfo.Player.ZoneBeach)
            {
				return SpawnCondition.Ocean.Chance * 0.05f;
			}*/
            return SpawnCondition.SurfaceJungle.Chance * 0.25f;
        }

		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Chlorophyte, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Chlorophyte, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.QuarionShard>(), 2, maximumDropped: 2));
        }
    }
}
