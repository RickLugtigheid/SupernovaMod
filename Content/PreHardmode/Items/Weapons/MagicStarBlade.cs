using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class MagicStarBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Magic Starblade");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-13, 0);
        }
        public override void SetDefaults()
        {
            Item.damage = 31;
            Item.crit = 5;
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 1000;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item29;
            Item.autoReuse = true;
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileID.HallowStar;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.mana = 7;
            Item.Size *= 0.5f;

            Item.DamageType = DamageClass.Magic;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            // Add random spread to our projectile
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Feather, 20);
            recipe.AddIngredient(ItemID.PlatinumBar, 14);
            recipe.AddIngredient(ItemID.FallenStar, 6);
            recipe.AddIngredient(ItemID.BeeWax, 4);
            recipe.AddIngredient(ItemID.Cloud, 35);
            recipe.AddIngredient(ItemID.RainCloud, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register(); 
        }
    }
}