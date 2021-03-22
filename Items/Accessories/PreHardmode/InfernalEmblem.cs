using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
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
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 8, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetModPlayer<SupernovaPlayer>().aInfernalEmblem = true;
        }
    }
}
