using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Rings
{
    public class RingOfProtection : RingBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ring of Protection");
            Tooltip.SetDefault("When the 'Ring Ability button' is pressed you take 30% less damage for 30 seconds.");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = Rarity.Green;
            item.value = Item.buyPrice(0, 5, 0, 0);
        }
		public override int Cooldown => 9300;
		public override void OnRingActivate(Player player)
		{
            // Add dust effect
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Lead);
                Main.dust[dust].scale = 1.4f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.6f;
                Main.dust[dust].velocity *= 1.6f;
            }
        }
		public override void OnRingCooldown(int curentCooldown, Player player)
		{
            // Only run the first 30 seconds (1800ms / 60 = 30sec)
            if (curentCooldown >= ((BuffCooldown * SupernovaPlayer.ringCooldownDecrease) - 1800))
                player.endurance += 0.30f; // Damage reduction +30%
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.SilverBar, 4);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.TungstenBar, 4);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}