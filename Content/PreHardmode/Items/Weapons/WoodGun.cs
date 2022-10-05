using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class WoodGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wood Gun");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);

        public override void SetDefaults()
        {
            Item.damage = 3;
            Item.width = 40;
            Item.crit = 3;
            Item.height = 20;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = 5;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 1.7f;
            Item.value = Item.buyPrice(0, 1, 70, 0); // Another way to handle value of item.
            Item.autoReuse = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item41;
            Item.shoot = 10;
            Item.shootSpeed = 6;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            // Add random spread to our projectile
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}