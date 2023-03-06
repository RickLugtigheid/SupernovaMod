using SupernovaMod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class InfernalEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Emblem of inferno");
            Tooltip.SetDefault("Spaws fire when you get hit");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 8, 0, 0);
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetModPlayer<AccessoryPlayer>().accInfernalEmblem = true;
        }
    }
}
