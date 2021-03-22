using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.BlocksAndOres
{
    public class RingForge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ring Forge");
            Tooltip.SetDefault("A forge where you can make all the rings of power.");
        }

        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 26;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = Item.buyPrice(0, 5, 40, 0);
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
            item.consumable = true;
            item.createTile = mod.TileType("RingForge");
            item.maxStack = 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 58);
            recipe.AddIngredient(ItemID.LavaBucket, 1);
            recipe.anyIronBar = true;
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
