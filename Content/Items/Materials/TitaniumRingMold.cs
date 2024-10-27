using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
    public class TitaniumRingMold : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 60; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.LightRed;
			Item.value = Item.buyPrice(0, 10);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TitaniumBar, 6);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AdamantiteBar, 6);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}
