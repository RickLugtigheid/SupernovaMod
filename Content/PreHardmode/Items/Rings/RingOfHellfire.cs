using Supernova.Api;
using Supernova.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Rings
{
	public class RingOfHellfire : SupernovaRing
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Ring of Hellfire");
            Tooltip.SetDefault("When the 'Ring Ability button' is pressed" +
                "\n You will gain the inferno buff" +
                "\n and your health will be increased by 45, damage by 10% and defence by 4 for 20 seconds");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.accessory = true;
        }
        public override int Cooldown => 5000;
        public override void OnRingActivate(Player player)
        {
            player.AddBuff(BuffID.Inferno, 1200);

            // Add dust effect
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Torch);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.7f;
                Main.dust[dust].velocity *= 1.7f;
            }
        }
        public override void OnRingCooldown(int curentCooldown, Player player)
		{
            // Only run the first 20 seconds (1200ms / 60 = 20sec)
            //
            if (curentCooldown >= ((Cooldown * RingPlayer.ringCooldownMulti) - 1200))
            {
                player.statLifeMax2 += 50;
                player.GetDamage(DamageClass.Ranged) += 0.35f;
                player.GetDamage(DamageClass.Melee) += 0.35f;
                player.GetDamage(DamageClass.Throwing) += 0.35f;
                player.GetDamage(DamageClass.Magic) += 0.35f;
                player.statDefense += 4;
            }
        }
		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(ModContent.TileType<Global.Tiles.RingForge>());
            recipe.Register();
        }
    }
}
