using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Accessories
{
    public class HeartOfTheJungle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart of the Jungle");
            Tooltip.SetDefault("Boost health by 25 and Slowly regenerates life");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 2, 35, 89);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BandofRegeneration);
            recipe.AddIngredient(ItemID.Vine, 2);
            recipe.AddIngredient(ItemID.JungleSpores, 6);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.statLifeMax2 += 25;
            player.lifeRegen += 2;
        }
    }
}
