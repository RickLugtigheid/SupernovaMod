using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Accessories
{
    public class HellFireBoots : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("HellFire Boots");
            Tooltip.SetDefault("Provides the ability to walk on water and lava" +
                "\n Grants immunity to fire blocks and 7 seconds of immunity to lava \n" +
                "Inflicts fire damage on attack\n" +
                "Reduces damage from touching lava");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
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
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LavaWaders);
            recipe.AddIngredient(ModContent.ItemType<DemonHorns>());
            recipe.AddIngredient(ItemID.MagmaStone);
            recipe.AddIngredient(ItemID.ObsidianRose);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
