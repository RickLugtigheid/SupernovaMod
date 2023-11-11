using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace SupernovaMod.Content.Items.Accessories
{
    public class SacrificialTalisman : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Sacrificial Talisman");
            // Tooltip.SetDefault("When your mana is lower than 15\nyou will lose 25 health but gain 50 mana");
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
			// Because the manaFlower does not take any life we will only have the effect
            // of this accessory trigger when there are no mana potions to consume.
		    //
			if (player.manaFlower && 
                player.HasItem(ItemID.ManaPotion)
				|| player.HasItem(ItemID.LesserManaPotion)
				|| player.HasItem(ItemID.GreaterManaPotion)
			)
			{
                return;
            }

            // Check if we are under the mana amount needed for the currently held item
            //
            if (player.statMana < player.GetManaCost(player.HeldItem))
            {
                // Drain ~20 life in exchange for 50 mana
                //
				player.Hurt(PlayerDeathReason.ByCustomReason(player.name + "'s life was drained"), Main.DamageVar(20), 0, false, true, -1, false, 100, 100);
                player.statMana += 50;
                player.ManaEffect(50);
            }
        }
    }
}
