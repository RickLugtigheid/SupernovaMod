using SupernovaMod.Api;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class SigilOfBalance : ModItem
    {
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.value = BuyPrice.RarityLightRed;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual = false)
		{
			HeartOfPower.HandleUpdateAccessory(player);
			ManaCore.HandleUpdateAccessory(player);
			SoulOfSwiftness.HandleUpdateAccessory(player);
			IronSkull.HandleUpdateAccessory(player);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<HeartOfPower>());
			recipe.AddIngredient(ModContent.ItemType<ManaCore>());
			recipe.AddIngredient(ModContent.ItemType<SoulOfSwiftness>());
			recipe.AddIngredient(ModContent.ItemType<IronSkull>());
			recipe.AddIngredient(ModContent.ItemType<Materials.Starcore>());
			recipe.AddTile(TileID.TinkerersWorkbench);
            //recipe.AddTile(ModContent.TileType<Tiles.StarfireForgeTile>()); // TODO: Use TinkerersWorkbench? instead when the starcore is available
            recipe.Register();
		}
	}
}
