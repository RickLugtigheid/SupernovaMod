using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class FirearmManual : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Firearm Manual");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 20;
            item.maxStack = 999;
            item.rare = Rarity.Blue;
            item.value = Item.buyPrice(0, 0, 7, 23);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 1);
            recipe.AddIngredient(ItemID.Wood, 6);
            recipe.AddIngredient(ItemID.Silk, 4);
            recipe.anyIronBar = true;
            recipe.anyWood = true;
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
