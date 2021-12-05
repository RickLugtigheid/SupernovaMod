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
                "\n and your health will be increaded by 45, damage by 10% and defence by 4 for 20 seconds");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = Rarity.Orange;
            item.value = Item.buyPrice(0, 6, 0, 0);
        }
		public override int Cooldown => 5000;
        public override void OnRingActivate(Player player)
        {
            player.AddBuff(BuffID.Inferno, 1200);

            // Add dust effect
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Fire);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.7f;
                Main.dust[dust].velocity *= 1.7f;
            }
        }
        public override void OnRingCooldown(int curentCooldown, Player player)
		{
            // Only run the first 20 seconds (1200ms / 60 = 20sec)
            if (curentCooldown >= ((BuffCooldown * SupernovaPlayer.ringCooldownDecrease) - 1200))
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
