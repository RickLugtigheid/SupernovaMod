using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SupernovaMod.Common.Systems
{
	public class ModRecipes : ModSystem
	{
		public override void AddRecipes()
		{
			AddVannilaRecipes();
		}
		/// <summary>
		/// Add recipes for vannila Items. 
		/// </summary>
		private void AddVannilaRecipes()
		{
			Recipe recipe;
			// Add Bloody Tear recipe
			//
			recipe = Recipe.Create(ItemID.BloodMoonStarter);
			recipe.AddIngredient(ModContent.ItemType<Content.Items.Materials.BloodShards>(), 12);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
