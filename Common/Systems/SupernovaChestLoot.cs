using Supernova.Api.ChestLoot;
using Supernova.Content.Items.Weapons.Magic;
using Supernova.Content.Items.Weapons.Ranged;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Supernova.Common.Systems
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
	}
}
