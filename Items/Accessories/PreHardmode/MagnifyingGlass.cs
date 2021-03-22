using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class MagnifyingGlass : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnifying Glass");
            Tooltip.SetDefault("Increases critical strike chance by 5%");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 12, 0, 0);
            item.accessory = true;
            item.rare = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.magicCrit += 5;
            player.meleeCrit += 5;
            player.rangedCrit += 5;
            player.thrownCrit += 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BlackLens);
            recipe.AddIngredient(ItemID.Lens, 4);
            recipe.AddIngredient(ItemID.IronBar);
            recipe.anyIronBar = true;
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
