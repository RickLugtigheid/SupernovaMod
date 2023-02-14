# Chest Loot System

This folder is dedicated to the chest loot system.
With this system a programmer can easily add custom loot to a chest type.

## Example
In the following example we create a class that uses the chest loot system to add the `Starry Night` as possible sky islands loot.
```cs
public class SupernovaChestLoot : CustomChestLootSystem
{
	protected override void ModifyChestLoot(ChestLoot chestLoot)
	{
		// Add the Starry Night bow as loot for skyware chests.
		// Give it a '1/25' (4%) spawn rate.
		chestLoot.Add(ChestFrameType.SkywareChest, ItemDropRule.Common(ModContent.ItemType<StarNight>(), 25));
	}
}
```