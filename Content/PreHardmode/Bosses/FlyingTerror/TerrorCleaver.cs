using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Content.PreHardmode.Bosses.FlyingTerror
{
	public class TerrorCleaver : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terror Cleaver");
			Tooltip.SetDefault("Shoots scythes of terror");
            //Tooltip.SetDefault("A big cleaver as strong as the bones of the Flying Terror");
        }

		public override void SetDefaults()
		{
			Item.damage = 22;
            Item.crit = 4;
            Item.width = 40;
			Item.height = 40;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 7, 0, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TerrorScythe>();
			Item.shootSpeed = 6;
			Item.DamageType = DamageClass.Melee;
		}

		/*private bool _canShoot = true;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Flip our canShoot bool so we shoot once every 2 swings
			_canShoot = !_canShoot;
			return _canShoot;
		}*/

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TerrorTuft>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
