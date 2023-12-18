using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
    public class VerglasBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
			Item.DefaultToPlaceableTile(ModContent.TileType<Content.Tiles.VerglasBar>());
			Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.value = 2100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemID.IronBar);
            recipe.acceptedGroups = new() { RecipeGroupID.IronBar };
            recipe.AddIngredient(ModContent.ItemType<Rime>());
            recipe.AddIngredient(ItemID.IceBlock, 3);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
}
