using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class BloodLens : ModItem
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
            Item.value = Item.buyPrice(0, 8, 0, 0);
			Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
			player.nightVision = true;
			player.pickSpeed -= 0.20f;
			player.AddBuff(BuffID.Spelunker, 1);
			player.AddBuff(BuffID.Shine, 1);
			player.AddBuff(BuffID.Dangersense, 1);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 4);
            recipe.AddIngredient(ItemID.SoulofSight);
			recipe.AddIngredient(ModContent.ItemType<LensOfGreed>());
			recipe.AddIngredient(ItemID.UltrabrightHelmet);
			recipe.AddIngredient(ItemID.TrapsightPotion);
			recipe.AddIngredient(ItemID.HunterPotion);
			recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
