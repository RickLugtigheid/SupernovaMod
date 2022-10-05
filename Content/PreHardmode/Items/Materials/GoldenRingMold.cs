using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Materials
{
    public class GoldenRingMold : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Ring Mold");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 12000;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 8);
            recipe.AddTile(ModContent.TileType<Global.Tiles.RingForge>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 8);
            recipe.AddTile(ModContent.TileType<Global.Tiles.RingForge>());
            recipe.Register();
        }
    }
}
