using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class LensOfGreed : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lens of Greed");
            Tooltip.SetDefault("Provides spelunker to the user");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 2, 70, 35);
            item.rare = Rarity.Green;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.AddBuff(BuffID.Spelunker, 1);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Lens);
            recipe.AddIngredient(ItemID.SpelunkerPotion, 3);
            recipe.AddIngredient(ItemID.Emerald);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
