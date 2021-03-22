using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Verglas
{
    [AutoloadEquip(EquipType.Legs)]
    public class VerglasBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Boots");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 14, 0, 0);
            item.rare = Rarity.Orange;
            item.defense = 7; // The Defence value for this piece of armour.
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 15);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
