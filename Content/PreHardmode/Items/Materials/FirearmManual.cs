using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Materials
{
    public class FirearmManual : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

            DisplayName.SetDefault("Firearm Manual");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 20;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 7, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 1);
            recipe.AddIngredient(ItemID.Wood, 6);
            recipe.AddIngredient(ItemID.Silk, 4);
            recipe.acceptedGroups = new() { RecipeGroupID.Wood, RecipeGroupID.IronBar };
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
