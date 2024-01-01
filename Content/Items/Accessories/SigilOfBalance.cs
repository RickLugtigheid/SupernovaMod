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
			// HeartOfPower
			player.GetDamage(DamageClass.Generic) += .04f * (player.statLife / 100);
			// ManaCore
			player.manaCost -= .06f * (player.statLife / 100);
			// SoulOfSwiftness
			UpdateAccessorySoulOfSwiftness(player);
			//
			UpdateAccessoryIronSkull(player);
		}
		private void UpdateAccessorySoulOfSwiftness(Player player)
		{
			if (player.statLife < 100)
			{
				player.moveSpeed *= 1.14f;
				return;
			}
			float multi = .14f / (player.statLife / 100);
			player.moveSpeed *= 1 + multi;
		}
		private void UpdateAccessoryIronSkull(Player player)
		{
			if (player.statLife < 100)
			{
				player.statDefense += 11;
				return;
			}
			player.statDefense += (int)(10 / (player.statLife / 100f));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<HeartOfPower>());
			recipe.AddIngredient(ModContent.ItemType<ManaCore>());
			recipe.AddIngredient(ModContent.ItemType<SoulOfSwiftness>());
			recipe.AddIngredient(ModContent.ItemType<IronSkull>());
			//recipe.AddIngredient(ModContent.ItemType<Materials.Starcore>());
			//recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddTile(ModContent.TileType<Tiles.StarfireForgeTile>()); // TODO: Use MythrilAnvil? instead when the starcore is available
			recipe.Register();
		}
	}
}
