using Supernova.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Accessories
{
    public class InfernalEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
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
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetModPlayer<AccessoryPlayer>().hasInfernalEmblem = true;
        }
    }
}
