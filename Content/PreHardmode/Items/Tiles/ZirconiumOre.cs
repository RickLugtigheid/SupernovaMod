using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Tiles
{
    public class ZirconiumOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Ore");
            Tooltip.SetDefault("A shiny pink ore chunk");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(0, 0, 1, 25);
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Global.Tiles.ZirconiumOreTile>();
            Item.maxStack = 999;
        }
    }
}
