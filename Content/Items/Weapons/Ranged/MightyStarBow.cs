using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class MightyStarBow : ModItem
    {
		private int _shots = 0;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-3, 0);

		public override void SetDefaults()
        {
            Item.crit = 1;
            Item.width = 16;
            Item.height = 24;

			Item.autoReuse = true;
			Item.damage = 61;
			Item.useAnimation = 15;
			Item.useTime = 5;
			Item.reuseDelay = 27;
			Item.UseSound = SoundID.Item5; // Sound for Bows
			Item.shootSpeed = 17.5f;

			Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = BuyPrice.RarityLightPurple; // Another way to handle value of item.
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
			Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.DamageType = DamageClass.Ranged;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ProjectileID.StarCloakStar;
			}
			else if (type == ProjectileID.JestersArrow)
			{
				type = ProjectileID.StarCannonStar;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			proj.DamageType = DamageClass.Ranged;
			return false;
			/*if (_shots >= 2)
			{
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Enchanted_Pink, Scale: 1.4f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2f;
					Main.dust[dust].velocity *= 2f;
				}
				SoundEngine.PlaySound(SoundID.Item4);
				ShootExtra(player, source, position, velocity, type, damage, knockback);
				_shots = 0;
			}
			_shots++;*/
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

        private void ShootExtra(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 speed = Main.MouseWorld - position;
			speed.Normalize();
			speed *= 17.5f;

			for (int i = 0; i < Main.rand.Next(2, 4); ++i)
			{
				Vector2 muzzleOffset = Vector2.Normalize(velocity) * -Main.rand.Next(20, 100);
				muzzleOffset.Y += Main.rand.Next(-55, 55);
				int proj = Projectile.NewProjectile(source, position.X + muzzleOffset.X, position.Y + muzzleOffset.Y, speed.X, speed.Y, ProjectileID.StarCloakStar, (int)(damage * .77f), knockback, player.whoAmI);
				Main.projectile[proj].scale = .65f;
				Main.projectile[proj].DamageType = DamageClass.Ranged;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient<StarNight>();
			recipe.AddIngredient<Materials.Starcore>(3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
