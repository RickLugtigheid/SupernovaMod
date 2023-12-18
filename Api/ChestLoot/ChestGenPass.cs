using SupernovaMod.Common;
using SupernovaMod.Content.Items.Accessories;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace SupernovaMod.Api.ChestLoot
{
	public class ChestGenPass : GenPass
	{
		private ChestLootManager _chestLoot;
		public ChestGenPass(ChestLootManager chestLoot) : base("Supernova: Filling chests...", 1f)
		{
			_chestLoot = chestLoot;
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			// Loop over all chests
			for (int i = 0; i < Main.chest.Length; i++)
			{
				Chest chest = Main.chest[i];

				if (chest == null/* || chest.IsEmptyChest()*/)
				{
					continue;
				}

				try
				{
					UpdateChest(chest);
				}
				catch { }

				progress.Value = (float)i / (float)Main.chest.Length;
			}
		}

		private void UpdateChest(Chest chest)
		{
			// Get chest tile and type
			Tile chestTile = Main.tile[chest.x, chest.y];
			ChestFrameType chestType = (ChestFrameType)(chestTile.TileFrameX / 36);

			// Try to get some loot for our chest
			//
			if (_chestLoot.TryGetloot(chestType, out int itemId, out ChestLootInjectRule lootInjectRule))
			{
				if (lootInjectRule == ChestLootInjectRule.ReplaceFirstItem)
				{
					chest.item[0].SetDefaults(itemId);
				}
				else
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == ItemID.None)
						{
							chest.item[inventoryIndex].SetDefaults(itemId);
							break; // Stop the loop because our item was injected
						}
					}
				}
			}
		}
	}
}
