using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;
using Terraria.Audio;
using SupernovaMod.Content.Npcs.FlyingTerror;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
	public class TerrorRecurve : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Terror Recurve");
            // Tooltip.SetDefault("");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.autoReuse = false;
            Item.crit = 1;
            Item.width = 16;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            Item.value = BuyPrice.RarityBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5; // Sound for Bows
            Item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 7;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
		}

        private int _shots = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            _shots++;
            if (_shots > 2)
            {
				SoundEngine.PlaySound(SoundID.Item14, player.Center);
                /*int num220 = 8;
				for (int num221 = 0; num221 < num220; num221++)
				{
					// Create velocity for angle
					Vector2 value17 = -Vector2
						// Normalize so the velocity ammount of the projectile doesn't matter
						.Normalize(velocity)
						// Rotate by angle
						.RotatedBy(MathHelper.ToRadians(360 / num220 * (num221 - 2)))
						// Make the velocity 6
						* 6;

					// Create a projectile for velocity
					Projectile proj = Main.projectile[Projectile.NewProjectile(Item.GetSource_ItemUse(Item), position, value17, ModContent.ProjectileType<Projectiles.Magic.TerrorProjFirendly>(), damage / 2, 1f, player.whoAmI, 1, Main.rand.Next(-45, 1))];
					proj.DamageType = DamageClass.Ranged;
					proj.penetrate = 1;
				}*/
                int proj = Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(6)), ModContent.ProjectileType<Projectiles.Magic.TerrorProjFirendly>(), (int)(damage * 1.5f), 4, player.whoAmI);
                Main.projectile[proj].DamageType = DamageClass.Ranged;
                Main.projectile[proj].penetrate  = 1;
				_shots = 0;
			}
			return true;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<TerrorTuft>());
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
