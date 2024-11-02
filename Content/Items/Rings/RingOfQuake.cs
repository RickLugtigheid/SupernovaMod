using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using SupernovaMod.Api;
using SupernovaMod.Api.Helpers;
using Terraria.Localization;

namespace SupernovaMod.Content.Items.Rings
{
    public class RingOfQuake : SupernovaRingItem
    {
        private float _rot;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.LightRed;
            Item.value = BuyPrice.RarityLightRed;
            Item.damage = 108;
            damage = 108;
        }
        public override int BaseCooldown => 47 * 60;
        public override void RingActivate(Player player, float ringPowerMulti)
        {
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with { Volume = 2 });

            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (target.CanBeChasedBy())
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.position, Vector2.Zero, 476, damage, 24f, Main.myPlayer, 0f, 0f, 0f);
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Vector2 position = ((Entity)player).position;
                int width = ((Entity)player).width;
                int height = ((Entity)player).height;
                float num = Utils.NextFloat(Main.rand, 0.8f, 1.2f);
                int dust = Dust.NewDust(position, width, height, 1, 0f, 0f, 0, default(Color), num);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Dust obj = Main.dust[dust];
                obj.velocity *= 2f;
                Dust obj2 = Main.dust[dust];
                obj2.velocity *= 2f;
            }

            if (player.GetModPlayer<Common.Players.EffectsPlayer>().ScreenShakePower < 15)
            {
                player.GetModPlayer<Common.Players.EffectsPlayer>().ScreenShakePower = 15;
            }
        }

        public override int MaxAnimationFrames => 80;

        private int? _projectile;
        protected virtual int AnimationDustType => DustID.Phantasmal;
        public override void RingUseAnimation(Player player, int frame)
        {
            if (frame == 25)
            {
                _projectile = Projectile.NewProjectile(
                    player.GetSource_Accessory(Item),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<Projectiles.VFX.Shockwave>(),
                    0, 0, player.whoAmI
                );
                var proj = Main.projectile[_projectile.Value].ModProjectile as Projectiles.VFX.Shockwave;
                proj.distortStrength = 350;
                proj.rippleCount = 5;
            }
            if (_projectile.HasValue)
            {
                Main.projectile[_projectile.Value].position = player.Center;
            }

            SoundEngine.PlaySound(SoundID.Item15);

            for (int i = 0; i < 2; i++)
            {
                _rot += MathHelper.ToRadians(67.5f);
                _rot %= MathHelper.ToRadians(360f);
                Vector2 dustPos = ((Entity)player).Center + Utils.RotatedBy(new Vector2(35f, 0f), (double)_rot, default(Vector2));
                Vector2 diff = ((Entity)player).Center - dustPos;
                diff.Normalize();
                Dust.NewDustPerfect(dustPos, DustID.Stone, (Vector2?)(diff * 2f), 0, default(Color), 1.5f).noGravity = true;
                Dust.NewDustPerfect(dustPos, AnimationDustType, (Vector2?)(diff * 2f), 0, default(Color), 1f).noGravity = true;
            }
            _rot += MathHelper.ToRadians(1f);

            for (int i = 0; i < 10; i++)
            {
                Vector2 position = player.Center + Main.rand.NextVector2CircularEdge(300, 300);
                Vector2 vector104 = Mathf.VelocityFPTP(position, player.Center, 3.5f);
                Dust dust29 = Main.dust[Dust.NewDust(position, 5, 0, Main.rand.NextBool() ? DustID.Stone : DustID.Dirt, 0f, 0f, 0, default(Color), Main.rand.NextFloat(.8f, 2))];
                dust29.noGravity = true;
                dust29.position = player.Center - vector104 * (float)Main.rand.Next(50, 650);
                dust29.velocity = vector104.RotatedBy(1.5707963705062866, default(Vector2)) * 14;
                dust29.fadeIn = 0.5f;
                dust29.customData = player.Center;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.TitaniumRingMold>());
            recipe.AddIngredient(ItemID.StoneBlock, 10);
            recipe.AddIngredient(ItemID.TitaniumBar, 4);
            recipe.AddTile(ModContent.TileType<Tiles.RingForge>());
            recipe.Register();

            //
            recipe.AddCustomShimmerResult(ModContent.ItemType<RingOfQuake_Adamantite>());
        }
    }
    public class RingOfQuake_Adamantite : RingOfQuake
    {
        protected override int AnimationDustType => DustID.Adamantite;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.TitaniumRingMold>());
            recipe.AddIngredient(ItemID.StoneBlock, 10);
            recipe.AddIngredient(ItemID.AdamantiteBar, 4);
            recipe.AddTile(ModContent.TileType<Tiles.RingForge>());
            recipe.Register();

            //
            recipe.AddCustomShimmerResult(ModContent.ItemType<RingOfQuake>());
        }
    }
}
