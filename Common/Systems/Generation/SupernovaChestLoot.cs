using SupernovaMod.Api.ChestLoot;
using SupernovaMod.Content.Items.Accessories;
using SupernovaMod.Content.Items.Weapons.Magic;
using SupernovaMod.Content.Items.Weapons.Ranged;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems.Generation
{
    public class SupernovaChestLoot : CustomChestLootSystem
    {
        protected override void ModifyChestLoot(ChestLootManager chestLoot)
        {
            // Add the `Starry Night` bow as loot for skyware chests.
            // Give it a '1/5' (20%) spawn rate.
            chestLoot.Add(ChestFrameType.SkywareChest, new ChestLootRule(ModContent.ItemType<StarNight>(), 5, ChestLootInjectRule.ReplaceFirstItem));

            // Add the `Magic Star Blade` as loot for gold dungeon chests.
            // Give it a '1/35' (2.8%) spawn rate.
            chestLoot.Add(ChestFrameType.LockedGoldChest, new ChestLootRule(ModContent.ItemType<MagicStarBlade>(), 35, ChestLootInjectRule.ReplaceFirstItem));

			// Add the `EerieCrystal` as loot for cavern chests.
			// Give it a '1/5' (20%) spawn rate.
			chestLoot.Add(ChestFrameType.GoldChest, new ChestLootRule(ModContent.ItemType<Content.Items.Consumables.EerieCrystal>(), 5, ChestLootInjectRule.AddItem));

			//ModifyMeteorChestLoot(chestLoot);
		}

		internal void ModifyMeteorChestLoot(ChestLootManager chestLoot)
        {
			// Add our MeteoriteChest loot
			//
			chestLoot.Add(ChestFrameType.MeteoriteChest, ItemDropRule.SequentialRulesNotScalingWithLuck(1,
                // Main loot
                //
				ItemDropRule.OneFromOptions(1, new int[]
				{
					ModContent.ItemType<MeteorBoots>()
				}),

                // Filler loot
                //
                ItemDropRule.Common(ItemID.Meteorite, 2, maximumDropped: 3),
                ItemDropRule.Common(ItemID.SilverCoin, 2, 10, 20),
				ItemDropRule.OneFromOptions(9, new int[]
				{
					ItemID.IronskinPotion,
                    ItemID.InfernoPotion,
                    ItemID.MiningPotion
				})
			));

			// Add filler loot
			//
			// TODO
		}
	}
}
