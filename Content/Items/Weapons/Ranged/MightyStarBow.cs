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
            Item.damage = 37;
            Item.crit = 0;
            Item.knockBack = 2;
            Item.width = 16;
            Item.height = 24;

			Item.autoReuse = true;
			Item.useAnimation = 9;
			Item.useTime = 3;
			Item.reuseDelay = 23;
			Item.shootSpeed = 17;
            Item.UseSound = SoundID.Item5;

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
			// Does not work as expected...
			//else if (type == ProjectileID.JestersArrow)
			//{
			//	type = ProjectileID.StarCannonStar;
			//}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			proj.DamageType = DamageClass.Ranged;
			return false;
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
