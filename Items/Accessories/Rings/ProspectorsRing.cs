using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Rings
{
    public class ProspectorsRing : RingBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prospectors Ring");
            Tooltip.SetDefault("Gives spelunker effect and faster mining speed for 12 seconds when the 'Ring Ability button' is pressed.");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = Rarity.Green;
            item.value = Item.buyPrice(0, 5, 0, 0);
        }
		public override int Cooldown => 1800;
        public override void OnRingActivate(Player player)
        {
            player.AddBuff(BuffID.Spelunker, 720);

            // Add dust effect
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Gold);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.5f;
                Main.dust[dust].velocity *= 1.5f;
            }
        }
		public override void OnRingCooldown(int curentCooldown, Player player)
		{
            // Only run the first 12 seconds (720ms / 60 = 12sec)
            if (curentCooldown >= ((BuffCooldown * SupernovaPlayer.ringCooldownDecrease) - 720))
                player.pickSpeed -= .5f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.SpelunkerPotion, 2);
            recipe.AddIngredient(ItemID.GoldOre, 4);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.SpelunkerPotion, 2);
            recipe.AddIngredient(ItemID.PlatinumOre, 4);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}