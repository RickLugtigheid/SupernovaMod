using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
    public class FirearmManual : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

            // DisplayName.SetDefault("Firearm Manual");
            // Tooltip.SetDefault("A guide to making guns");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 8, 0, 0);
        }
    }
}
