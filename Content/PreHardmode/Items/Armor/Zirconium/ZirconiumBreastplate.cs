using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Content.PreHardmode.Items.Armor.Zirconium
{
    // Added instread of AutoLoad
    [AutoloadEquip(EquipType.Body)]
    public class ZirconiumBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Breastplate"); // Set the name
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 7, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4; // The Defence value for this piece of armour.
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.ZirconiumBar>(), 32);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
