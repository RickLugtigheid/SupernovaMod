using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Rings
{
    public class RingOfHellfire : RingBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ring of Hellfire");
            Tooltip.SetDefault("When the 'Ring Ability button' is pressed" +
                "\n You will gain the inferno buff" +
                "\n and your health will be increaded by 45, damage by 10% and defence by 4 for 20 seconds" + RingBase.RING_HELP);
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = 3;
            item.value = Item.buyPrice(0, 6, 0, 0);
        }
		public override int cooldown => 5000;
		public override void OnRingActivate(Player player)
		{
            player.AddBuff(BuffID.Inferno, 1200);
        }
        public override void OnRingCooldown(int curentCooldown, Player player)
		{
            if (curentCooldown >= (cooldown - 1200))
			{
                player.statLifeMax2 += 50;
                player.rangedDamage += 0.35f;
                player.meleeDamage += 0.35f;
                player.thrownDamage += 0.35f;
                player.magicDamage += 0.35f;
                player.statDefense += 4;
            }
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GoldenRingMold"));

            recipe.AddIngredient(ItemID.HellstoneBar, 5);

            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
