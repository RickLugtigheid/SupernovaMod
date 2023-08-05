using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class MagnifyingGlass : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Magnifying Glass");
            // Tooltip.SetDefault("Increases critical strike chance by 5%");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetCritChance(DamageClass.Generic) += 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BlackLens);
            recipe.AddIngredient(ItemID.Lens, 4);
            recipe.AddIngredient(ItemID.IronBar);
            recipe.acceptedGroups = new() { RecipeGroupID.IronBar };
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
