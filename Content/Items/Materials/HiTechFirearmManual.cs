using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Materials
{
    public class HiTechFirearmManual : ModItem
    {
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 1; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
		}
		public override void SetDefaults()
        {
            Item.width = 16;
			Item.height = 20;
			Item.maxStack = 32;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(0, 12, 0, 0);
        }

        public override void AddRecipes()
        {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<FirearmManual>());
            recipe.AddIngredient(ModContent.ItemType<MechroDrive>(), 2);
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
