using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Items.Weapons.PreHardmode
{
    public class VerglasSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Verglas Sword");
        }
		public override void SetDefaults()
		{
			item.damage = 30;
			item.melee = true;
            item.crit = 3;
            item.width = 40;
			item.height = 40;

			item.useStyle = 1;
			item.knockBack = 3;
            item.value = Item.buyPrice(0, 9, 47, 0); // Another way to handle value of item.
			item.rare = Rarity.Orange;
			item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useAnimation = 32;
			item.useTime = 32;

            item.shootSpeed = 9f;
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (Main.rand.Next(3) == 1) type = ProjectileID.FrostBoltSword;
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner);
			return false;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 8);
            recipe.AddIngredient(ItemID.IceBlade);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
