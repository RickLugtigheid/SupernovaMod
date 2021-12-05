using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class SacrificialTalisman : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sacrificial Talisman");
            Tooltip.SetDefault("When your mana is lower than 15\nyou will lose 50 health but gain 100 mana");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = Rarity.Orange;
            item.value = Item.buyPrice(0, 8, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            if (player.statMana <= 15 && player.statLife >= 50)
            {
                player.statLife -= 50;
                player.statMana += 100;
                Main.PlaySound(SoundID.MaxMana, player.Center);
                Main.PlaySound(SoundID.PlayerHit, player.Center);
            }
        }
    }
}
