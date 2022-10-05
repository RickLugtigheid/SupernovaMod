using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Supernova.Content.PreHardmode.Items.Accessories
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
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 8, 0, 0);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            if (player.statMana <= 15 && player.statLife >= 50)
            {
                player.statLife -= 50;
                player.statMana += 100;
                SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
                SoundEngine.PlaySound(SoundID.PlayerHit, player.Center);
            }
        }
    }
}
