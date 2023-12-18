using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Placeable
{
	public class ZirconiumOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 54;

            // DisplayName.SetDefault("Zirconium Ore");
            // Tooltip.SetDefault("A shiny pink ore chunk");
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
            Item.createTile = ModContent.TileType<Content.Tiles.ZirconiumOreTile>();
            Item.maxStack = 9999;
        }
    }
}
