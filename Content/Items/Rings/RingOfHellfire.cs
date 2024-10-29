using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class RingOfHellfire : SupernovaRingItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Ring of Hellfire");
            /* Tooltip.SetDefault("When the 'Ring Ability button' is pressed" +
                "\n You will gain the inferno and Hellfire Ring buff." +
                "\n The Hellfire Ring buff gives every attack a chance to spawn a fiery explosion near the target."); */
        }
        public override void SetDefaults()
        {
			base.SetDefaults();
			Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 6, 0, 0);
        }
        public override int BaseCooldown => 60 * 200;
        public override void RingActivate(Player player, float ringPowerMulti)
        {
            int buffTime = 60 * 30;
            buffTime = (int)(buffTime * ringPowerMulti);
            player.AddBuff(BuffID.Inferno, buffTime);
            player.AddBuff(ModContent.BuffType<Buffs.Rings.HellfireRingBuff>(), buffTime);

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
        public override void RingUseAnimation(Player player, int frame)
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
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}
