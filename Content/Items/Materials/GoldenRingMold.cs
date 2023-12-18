using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
    public class GoldenRingMold : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

            // DisplayName.SetDefault("Golden Ring Mold");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = 12000;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 8);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 8);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}
