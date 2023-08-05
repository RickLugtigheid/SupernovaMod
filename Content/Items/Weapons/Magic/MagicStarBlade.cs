using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class MagicStarBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Magic Starblade");
            // Tooltip.SetDefault("Shoots a random star projectile.");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }
        public override void SetDefaults()
        {
            Item.damage = 31;
            Item.crit = 3;
            Item.width = 42;
            Item.height = 42;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = 1000;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item29;
            Item.autoReuse = true;
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileID.HallowStar;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.mana = 6;
            Item.Size *= 0.5f;

            Item.DamageType = DamageClass.Magic;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Add random spread to our projectile
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(7));

            // Set the projectile type to a random star projectile
            type = TerrariaRandom.NextProjectileIDAnyStar();

            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Feather, 10);
            recipe.AddIngredient(ItemID.PlatinumBar, 8);
            recipe.AddIngredient(ItemID.FallenStar, 6);
            recipe.AddIngredient(ItemID.BeeWax, 4);
            recipe.AddIngredient(ItemID.Cloud, 20);
            recipe.AddIngredient(ItemID.RainCloud, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}