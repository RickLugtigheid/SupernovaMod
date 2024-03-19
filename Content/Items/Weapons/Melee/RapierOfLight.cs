using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
	public class RapierOfLight : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 58;
			Item.crit = 7;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.knockBack = 6;

			Item.value = Item.buyPrice(0, 12, 0, 0);
			Item.rare = ItemRarityID.LightRed;

			Item.useStyle = ItemUseStyleID.Rapier;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.autoReuse = true;
			Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item

			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.RapierOfLightProjectile>(); // The projectile is what makes a shortsword work
			Item.shootSpeed = 4f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 7);
			recipe.AddIngredient(ModContent.ItemType<Materials.BrokenSwordShards>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
