using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class Kusanagi : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.crit = 2;
			Item.width = 72;
			Item.height = 80;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.ArmorPenetration = 15;
			Item.DamageType = DamageClass.Melee;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Katana);
			recipe.AddIngredient(ModContent.ItemType<Materials.BrokenSwordShards>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
