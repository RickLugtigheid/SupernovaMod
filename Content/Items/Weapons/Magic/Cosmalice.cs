using SupernovaMod.Api;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
	public class Cosmalice : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 47;
			Item.crit = 2;
			Item.width = 24;
			Item.height = 28;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = BuyPrice.RarityGreen;
			Item.rare = ItemRarityID.LightRed;
			Item.mana = 10;
			Item.UseSound = SoundID.Item21;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.EldrichBolt>();
			Item.shootSpeed = 12;
			Item.DamageType = DamageClass.Magic;
		}

		/*public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpellTome);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}*/
	}
}
