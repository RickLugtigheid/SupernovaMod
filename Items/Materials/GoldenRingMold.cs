using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Materials
{
    public class GoldenRingMold : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Ring Mold");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.value = 12000;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 8);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumBar, 8);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
