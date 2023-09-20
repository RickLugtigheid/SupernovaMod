using SupernovaMod.Common.Players;
using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class RingOfProtection : SupernovaRingItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
			base.SetDefaults();
			Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }
        public override int BaseCooldown => 60 * 60; // 1 minute
        public override void RingActivate(Player player, float ringPowerMulti)
        {
            // Add dust effect
            //
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Lead, Scale: 1.4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.75f;
                Main.dust[dust].velocity *= 1.75f;
            }

			// Add the ShadowDodge buff to the player
			//
			int buffTime = 60 * 30;
			buffTime = (int)(buffTime * ringPowerMulti);
			player.AddBuff(BuffID.ShadowDodge, buffTime);
        }

        public override int MaxAnimationFrames => 1;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ResourcePlayer resourcePlayer = player.GetModPlayer<ResourcePlayer>();
            player.statDefense += (int)(2 * resourcePlayer.ringPower);
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.SilverBar, 4);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.TungstenBar, 4);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}