using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class DemonHorns : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Horn");
            Tooltip.SetDefault("Reduces damage taken by 8%\n" +
                "Immumity to OnFire");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.accessory = true;
            item.rare = Rarity.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.endurance += 0.08f;
            player.buffImmune[BuffID.OnFire] = true;
        }
    }
}
