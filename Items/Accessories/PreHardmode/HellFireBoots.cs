using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class HellFireBoots : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HellFire Boots");
            Tooltip.SetDefault("Provides the ability to walk on water and lava" +
                "\n Grants immunity to fire blocks and 7 seconds of immunity to lava \n" +
                "Inflicts fire damage on attack\n" +
                "Reduces damage from touching");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.accessory = true;
            item.rare = Rarity.LigtRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.endurance += 0.08f;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;
            player.lavaMax += 420;
            player.waterWalk = true;
            player.magmaStone = true;
            player.lavaRose = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LavaWaders);
            recipe.AddIngredient(mod.GetItem("DemonHorns"));
            recipe.AddIngredient(ItemID.MagmaStone);
            recipe.AddIngredient(ItemID.ObsidianRose);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
