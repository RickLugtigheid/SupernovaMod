﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Accessories
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
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.endurance += 0.08f;
            player.buffImmune[BuffID.OnFire] = true;
        }
    }
}
