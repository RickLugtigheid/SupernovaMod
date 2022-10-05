using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Supernova.Common.Systems;
using Terraria.GameContent.ItemDropRules;

namespace Supernova.Content.PreHardmode.Items.Misc
{
    public class MantaFood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MantaFood");
            Tooltip.SetDefault("Dropped by Dungeon enemies\n" +
                "Use underground" +
                "\n Summons the Stone Manta Ray");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 40;
            Item.useTime = 45;
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.HoldUp; // Holds up like a summon item.
        }

        public override bool CanUseItem(Player player)
        {
            // Does NPC Exist
            bool alreadySpawned = NPC.AnyNPCs(Mod.Find<ModNPC>("StoneMantaRay").Type);

            // return NPC.downedQueenBee && Main.hardMode && !NPC.AnyNPCs(mod.NPCType("TutorialBoss")); // NPC will spawn if No existing Tutorial Boss, Queen Bee is downed and it is hardmode 
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (player.ZoneBeach)
            {
                NPC.SpawnOnPlayer(player.whoAmI, Mod.Find<ModNPC>("StoneMantaRay").Type); // Spawn the boss within a range of the player. 
                SoundEngine.PlaySound(SoundID.Roar, player.position) ;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    public class MantaFoodGlobalNPC : GlobalNPC
    {
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
            if (Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].ZoneDungeon)
            {
                if (!SupernovaBosses.downedStormSovereign && Main.rand.NextBool(24) || Main.rand.NextBool(60))
				{
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MantaFood>()));
				}
            }

            base.ModifyNPCLoot(npc, npcLoot);
		}
    }
}
