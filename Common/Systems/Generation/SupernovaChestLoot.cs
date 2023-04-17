using SupernovaMod.Api.ChestLoot;
using SupernovaMod.Content.Items.Accessories;
using SupernovaMod.Content.Items.Weapons.Magic;
using SupernovaMod.Content.Items.Weapons.Ranged;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems.Generation
{
    public class SupernovaChestLoot : CustomChestLootSystem
    {
        protected override void ModifyChestLoot(ChestLoot chestLoot)
        {
            // Add the `Starry Night` bow as loot for skyware chests.
            // Give it a '1/5' (20%) spawn rate.
            chestLoot.Add(ChestFrameType.LockedGoldChest, new ChestLootRule(ModContent.ItemType<StarNight>(), 5, ChestLootInjectRule.ReplaceFirstItem));

            // Add the `Magic Star Blade` as loot for gold dungeon chests.
            // Give it a '1/35' (2.8%) spawn rate.
            chestLoot.Add(ChestFrameType.LockedGoldChest, new ChestLootRule(ModContent.ItemType<MagicStarBlade>(), 35, ChestLootInjectRule.ReplaceFirstItem));
        }

		internal void ModifyMeteorChestLoot(ChestLoot chestLoot)
        {
            // Add our main loot
            //
            chestLoot.Add(ChestFrameType.MeteoriteChest, new ChestLootRule(ModContent.ItemType<MeteorBoots>(), 1, ChestLootInjectRule.ReplaceFirstItem));
        
            // Add filler loot
            //
            // TODO
        }
	}
}
