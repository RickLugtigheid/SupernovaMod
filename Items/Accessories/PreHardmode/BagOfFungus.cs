using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class BagOfFungus : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Spore Bag");
            Tooltip.SetDefault("Has a chance to release Glowing Mushrooms when damaged");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = Rarity.Green;
            item.value = Item.buyPrice(0, 1, 50, 0);
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetModPlayer<SupernovaPlayer>().aBagOfFungus = true;
        }
    }
}
