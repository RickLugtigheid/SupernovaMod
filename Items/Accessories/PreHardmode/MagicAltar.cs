using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.PreHardmode
{
    public class MagicAltar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Altar");
            Tooltip.SetDefault("When your mana is lower than 15\nyou will lose 50 health but gain 50 mana");
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
            if(player.statMana <= 15 && player.statLife >= 50)
            {
                player.statLife -= 50;
                player.statMana += 50;
                Main.PlaySound(SoundID.MaxMana, player.Center);
                Main.PlaySound(SoundID.PlayerHit, player.Center);
            }
        }
    }
}
