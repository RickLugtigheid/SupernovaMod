using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
    public class ZirconiumBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 55; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.ZirconiumBar>());
			Item.width = 24;
            Item.height = 24;
            Item.value = 10000;
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Placeable.ZirconiumOre>(), 4);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}