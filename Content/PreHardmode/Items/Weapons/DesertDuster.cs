using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
	public class DesertDuster : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desert Duster");
			Tooltip.SetDefault("Uses sand to shoot.");
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.width = 40;
            Item.crit = 4;
            Item.height = 20;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = 5;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 1.2f;
            Item.value = Item.buyPrice(0, 11, 50, 0);
            Item.autoReuse = true;
            Item.rare = 2;
            Item.UseSound = SoundID.Item11;
            Item.shoot = 10; //idk why but all the guns in the vanilla source have this
            Item.shootSpeed = 10;
            Item.useAmmo = AmmoID.Sand;

			Item.DamageType = DamageClass.Ranged;
		}
		public override Vector2? HoldoutOffset() => new Vector2(-2, 2);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
			recipe.AddIngredient(ItemID.FossilOre, 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		private int _ammoItemId;
		public override void OnConsumeAmmo(Item ammo, Player player)
		{
			_ammoItemId = ammo.netID;
			// An 8% chance not to consume ammo
			if (Main.rand.NextFloat() >= .8f)
			{
				base.OnConsumeAmmo(ammo, player);
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Add random spread to our projectile
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));

			// Get the correct projectile to shoot
			type = GetProjectileForAmmo();

			base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
		}

		private int GetProjectileForAmmo()
		{
			switch(_ammoItemId)
			{
				case ItemID.CrimsandBlock:
					return ModContent.ProjectileType<Global.Projectiles.Bullets.CrimsandBullet>();
				case ItemID.EbonsandBlock:
					return ModContent.ProjectileType<Global.Projectiles.Bullets.EbonsandBullet>();
				case ItemID.PearlsandBlock:
					return ModContent.ProjectileType<Global.Projectiles.Bullets.PearlsandBullet>();
				default: // Default & ItemID.SandBlock
					return ModContent.ProjectileType<Global.Projectiles.Bullets.SandBullet>();
			}
		}
	}
}