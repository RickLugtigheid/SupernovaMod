using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
    public class HellfireRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellfire Rifle");
            Tooltip.SetDefault("Turns Wooden bullets into Molten Bullets");
        }
        public override Vector2? HoldoutOffset() => new Vector2(-8, 2);
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.width = 40;
            Item.crit = 4;
            Item.height = 20;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = 5;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 2.4f;
            Item.value = Item.buyPrice(0, 15, 50, 0);
            Item.autoReuse = true;
            Item.rare = 2;
            Item.UseSound = SoundID.Item11;
            Item.shoot = 10; //idk why but all the guns in the vanilla source have this
            Item.shootSpeed = 11f;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;

            Item.scale = .95f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WoodenRifle>());
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
            recipe.AddIngredient(ItemID.HellstoneBar, 17);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

		public override void OnConsumeAmmo(Item ammo, Player player)
		{
            // An 18% chance not to consume ammo
            if (Main.rand.NextFloat() >= .18f)
			{
                base.OnConsumeAmmo(ammo, player);
            }
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            // Add random spread to our projectile
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));

            // Convert wooden bullets to molten bullets
            if (type == ModContent.ProjectileType<Global.Projectiles.WoodenBullet>())
			{
                type = ModContent.ProjectileType<Global.Projectiles.MoltenBullet>();
			}

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
    }
}