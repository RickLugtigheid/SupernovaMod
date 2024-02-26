using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Armor.Carnage
{
    // Added instread of AutoLoad
    [AutoloadEquip(EquipType.Body)]
    public class CarnageBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = BuyPrice.RarityGreen;
            Item.rare = ItemRarityID.Green;
            Item.defense = 8; // The Defence value for this piece of armour.
        }

		public override void UpdateEquip(Player player)
		{
			player.endurance += 0.05f;
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 30);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 12);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
