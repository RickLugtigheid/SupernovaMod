using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class WoodenTarget : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Target");
            Tooltip.SetDefault("increases trown damage by 2,5%" +
                                "\n increases trown velocity");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 2, 35, 89);
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 1000);
            recipe.AddIngredient(ItemID.StoneBlock, 200);
            recipe.AddIngredient(ItemID.Rope, 120);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.thrownDamage += 0.25f;
            player.thrownVelocity += 0.3f;
        }
    }
}
