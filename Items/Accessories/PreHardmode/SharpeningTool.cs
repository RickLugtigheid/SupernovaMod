using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class SharpeningTool : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharpening tool");
            Tooltip.SetDefault("Increases melee and thrown damage by 5%");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 12, 0, 0);
            item.accessory = true;
            item.rare = Rarity.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.meleeDamage += 0.05f;
			player.thrownDamage += 0.05f;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.anyIronBar = true;
            recipe.anyWood = true;
            recipe.AddIngredient(ItemID.IronBar, 12);
            recipe.AddIngredient(ItemID.Granite, 12);
            recipe.AddIngredient(ItemID.Wood, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
