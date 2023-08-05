using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class LensOfGreed : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Lens of Greed");
            // Tooltip.SetDefault("Provides spelunker to the user");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 2, 70, 35);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.AddBuff(BuffID.Spelunker, 1);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Lens);
            recipe.AddIngredient(ItemID.SpelunkerPotion, 3);
            recipe.AddIngredient(ItemID.Emerald);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
