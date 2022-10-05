﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Accessories
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
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.GetModPlayer<Common.Players.AccessoryPlayer>().hasBagOfFungus = true;
        }
    }
}
