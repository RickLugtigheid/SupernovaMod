using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
    public class TerrorCleaver : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terror Cleaver");
            Tooltip.SetDefault("A big cleaver as strong as the bones of the Flying Terror");
        }
		public override void SetDefaults()
		{
			item.damage = 27;
			item.melee = true;
            item.crit = 4;
            item.width = 40;
			item.height = 40;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
            item.value = Item.buyPrice(0, 7, 0, 0); // Another way to handle value of item.
            item.rare = Rarity.Orange;
			item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("TerrorWing"));
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
