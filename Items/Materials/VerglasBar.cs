using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class VerglasBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Bar");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = 2100;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar);
            recipe.anyIronBar = true;
            recipe.AddIngredient(mod.GetItem("Rime"));
            recipe.AddIngredient(ItemID.IceBlock, 3);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}
