using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class SharpeningTool : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Sharpening tool");
            // Tooltip.SetDefault("Increases melee and thrown damage by 5%");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetDamage(DamageClass.Melee) += .05f;
            player.GetDamage(DamageClass.Throwing) += .05f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.acceptedGroups = new() { RecipeGroupID.Wood, RecipeGroupID.IronBar };
            recipe.AddIngredient(ItemID.IronBar, 12);
            recipe.AddIngredient(ItemID.Granite, 12);
            recipe.AddIngredient(ItemID.Wood, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
