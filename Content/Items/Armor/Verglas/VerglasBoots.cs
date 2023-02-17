using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.Items.Armor.Verglas
{
    [AutoloadEquip(EquipType.Legs)]
    public class VerglasBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Verglas Boots");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 14, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7; // The Defence value for this piece of armour.
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 15);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
