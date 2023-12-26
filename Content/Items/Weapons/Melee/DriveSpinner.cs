using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class DriveSpinner : ModItem
    {
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			//Tooltip.SetDefault("Inflicts Electrified.");
		}

		public override void SetDefaults()
		{
			Item.knockBack = 1.8f;
			Item.damage = 47;
			Item.crit = 4;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 24;
			Item.height = 24;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.channel = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 25;
			Item.useTime = 56;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.Yoyos.DriveSpinnerProjectile>();
			Item.shootSpeed = 23f;
			Item.value = BuyPrice.RarityLightRed;
			Item.rare = ItemRarityID.LightRed;

			Item.DamageType = DamageClass.Melee;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Materials.MechroDrive>(), 3);
			recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddIngredient(ItemID.WhiteString);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
