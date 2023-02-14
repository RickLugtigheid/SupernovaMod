using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Supernova.Api.ChestLoot
{
	public abstract class CustomChestLootSystem : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			// Fill our chest loot
			ChestLoot loot = new ChestLoot();
			ModifyChestLoot(loot);
			
			// Add our chest loot
			tasks.Add(new ChestGenPass(loot));
		}
		/// <summary>
		/// Lets you add custom loot to a chest type.
		/// </summary>
		/// <param name="chestLoot"></param>
		protected virtual void ModifyChestLoot(ChestLoot chestLoot) { }
	}
}
