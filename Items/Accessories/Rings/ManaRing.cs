using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Rings
{
    public class ManaRing : RingBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Ring");
            Tooltip.SetDefault($"Gives half of your mana back or fills your mana bar in hardmode when the 'Ring Ability button' is pressed. {RingBase.RING_HELP}");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.rare = Rarity.Green;
            item.value = Item.buyPrice(0, 4, 0, 0);
        }
		public override int cooldown => 450;
		public override void OnRingActivate(Player player)
		{
            player.statMana += Main.hardMode ? player.statManaMax2: player.statManaMax2 / 2;
		}
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GoldenRingMold"));

            recipe.AddIngredient(ItemID.ManaCrystal, 3);
            recipe.AddIngredient(ItemID.ManaRegenerationPotion);
            recipe.AddTile(mod.GetTile("RingForge"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}