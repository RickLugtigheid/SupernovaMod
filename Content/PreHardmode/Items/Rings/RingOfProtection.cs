using Supernova.Api.Core;
using Supernova.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Rings
{
	public class RingOfProtection : SupernovaRing
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Ring of Protection");
            Tooltip.SetDefault("When the 'Ring Ability button' is pressed you take 30% less damage for 30 seconds.");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.accessory = true;
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
            //
            if (curentCooldown >= ((Cooldown * RingPlayer.ringCooldownMulti) - 1800))
                player.endurance += 0.30f; // Damage reduction +30%
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.SilverBar, 4);
            recipe.AddTile(ModContent.TileType<Global.Tiles.RingForge>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.TungstenBar, 4);
            recipe.AddTile(ModContent.TileType<Global.Tiles.RingForge>());
            recipe.Register();
        }
    }
}