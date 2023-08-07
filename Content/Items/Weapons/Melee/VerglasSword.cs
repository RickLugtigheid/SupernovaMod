using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class VerglasSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Verglas Splitter");
        }
        public override void SetDefaults()
        {
			Item.width = 32;
			Item.height = 32;
			Item.UseSound = SoundID.Item1;
			Item.damage = 45;
			Item.crit = 2;
			Item.knockBack = 5.5f;
			Item.autoReuse = true;
			Item.scale = 1f;
			Item.shootSpeed = 6f;
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.VerglasSlash>();
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = BuyPrice.RarityOrange;
		}

		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			//Projectile.NewProjectile(source, position, new Vector2(0.1f, 0).RotatedBy(velocity.ToRotation()), ModContent.ProjectileType<Projectiles.Melee.VerglasSlash>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax * 1.3f);
			//Projectile.NewProjectile(source, position, new Vector2(player.direction, 0), ModContent.ProjectileType<Projectiles.Melee.VerglasSlash2>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);

			Projectile.NewProjectile(source, position, new Vector2(player.direction, 0), ModContent.ProjectileType<Projectiles.Melee.VerglasSlash>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax * 1.3f);
			return false;
		}*/

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float adjustedItemScale = player.GetAdjustedItemScale(Item); // Get the melee scale of the player and item.
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI); // Sync the changes in multiplayer.

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Content.Items.Materials.VerglasBar>(), 8);
            recipe.AddIngredient(ItemID.IceBlade);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
