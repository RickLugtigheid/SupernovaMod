using Microsoft.Xna.Framework;
using Supernova.Common.Players;
using Supernova.Content.PreHardmode.Items.Rings.BaseRings;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Rings
{
    public class RingOfHellfire : SupernovaRingItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Ring of Hellfire");
            Tooltip.SetDefault("When the 'Ring Ability button' is pressed" +
                "\n You will gain the inferno and Hellfire Ring buff." +
				"\n The Hellfire Ring buff gives every attack a chance to spawn a fiery explosion near the target.");
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
        public override int Cooldown => 60 * 140;
        public override void RingActivate(Player player)
        {
            player.AddBuff(BuffID.Inferno, 60 * 40);
			player.AddBuff(ModContent.BuffType<Global.Buffs.Rings.HellfireRingBuff>(), 60 * 40);

			// Add dust effect
			for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Torch);
                Main.dust[dust].scale = 2;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 3;
                Main.dust[dust].velocity *= 3;
            }
			SoundEngine.PlaySound(SoundID.Item74);
		}

		public override int MaxAnimationFrames => 40;
		public override void RingUseAnimation(Player player)
		{
            SoundEngine.PlaySound(SoundID.Item15);
			Vector2 dustPos = player.Center + new Vector2(30, 0).RotatedByRandom(MathHelper.ToRadians(360));
			Vector2 diff = player.Center - dustPos;
			diff.Normalize();

			Dust.NewDustPerfect(dustPos, DustID.Lava, diff * 2).noGravity = true;
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
