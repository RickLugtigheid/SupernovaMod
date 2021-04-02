using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class VerglasBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Bow");
            Tooltip.SetDefault("Turns wooden arrows into frostburn arrows.");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }

        public override void SetDefaults()
        {
            item.damage = 31;
            item.autoReuse = true;
            item.crit = 5;
            item.width = 16;
            item.height = 24;
            item.useTime = 53;
            item.useAnimation = 53;
            item.useStyle = 5; // Bow Use Style
            item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            item.value = Item.buyPrice(0, 9, 47, 0); // Another way to handle value of item.
            item.rare = Rarity.Orange;
            item.UseSound = SoundID.Item5; // Sound for Bows
            item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            item.shoot = ProjectileID.WoodenArrowFriendly;
            item.shootSpeed = 13f;
            item.ranged = true; // For Ranged Weapon
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) // or ProjectileID.WoodenArrowFriendly
			{
				type = ProjectileID.FrostburnArrow; // or ProjectileID.FireArrow;
            }
			return true; // return true to allow tmodloader to call Projectile.NewProjectile as normal
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
