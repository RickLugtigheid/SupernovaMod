using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Rings.BaseRings;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Rings
{
    public class GemRing : SupernovaRingItem
    {
        public override RingType RingType => RingType.Projectile;
		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Gem Ring");
            // Tooltip.SetDefault("When the 'Ring Ability button' is pressed, .");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.accessory = true;

            damage = 17;
        }

        public override int MaxAnimationFrames => 60;

        public override int BaseCooldown => 4800;
        public override void RingActivate(Player player, float ringPowerMulti)
        {
            // Add dust effect
            for (int j = 0; j < 5; j++)
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.GemDiamond);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 1.5f;
                Main.dust[dust].velocity *= 1.5f;
            }

            Vector2 position = player.Center;
            SoundEngine.PlaySound(SoundID.Item14, position);
            int ShootDamage = (int)(damage * ringPowerMulti);
            int Type = 121;
            float ShootKnockback = 3.6f;

            Vector2 diff = Main.MouseWorld - player.Center;

            diff.Normalize();

            Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 9.5f, diff.Y * 9.5f, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            int Type1 = 122;
            Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 9, diff.Y * 9, Type1, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            int Type2 = 123;
            Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 8.5f, diff.Y * 8.5f, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            int Type3 = 124;
            Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 8, diff.Y * 8, Type3, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            int Type4 = 125;
            Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 7.5f, diff.Y * 7.5f, Type4, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            int Type5 = 126;
            Projectile.NewProjectile(Item.GetSource_FromAI(), position.X, position.Y, diff.X * 7, diff.Y * 7, Type5, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
        }

        private int[] _gemDusts = new int[] { DustID.GemTopaz, DustID.GemSapphire, DustID.GemRuby, DustID.GemEmerald, DustID.GemDiamond };
        private float _rot = 0;
        public override void RingUseAnimation(Player player, int frame)
        {
            SoundEngine.PlaySound(SoundID.Item15);

            // Spawn gem dust on the player
            //
            for (int i = 0; i < _gemDusts.Length; i++)
            {
                _rot += MathHelper.ToRadians(45);
                _rot = _rot % MathHelper.ToRadians(360);

                Vector2 dustPos = player.Center + new Vector2(15, 0).RotatedBy(_rot);
                Vector2 diff = player.Center - dustPos;
                diff.Normalize();

                int dustType = _gemDusts[i];

                Dust.NewDustPerfect(dustPos, dustType, diff).noGravity = true;
            }
            _rot += MathHelper.ToRadians(1);

            // Spawn gem dust on the mouse position
            Dust.NewDustPerfect(Main.MouseWorld, DustID.AmberBolt).noGravity = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.GoldenRingMold>());
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddTile(ModContent.TileType<Content.Tiles.RingForge>());
            recipe.Register();
        }
    }
}