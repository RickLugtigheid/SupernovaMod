using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Carnage
{
    [AutoloadEquip(EquipType.Legs)]
    public class CarnageBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Boots");
            Tooltip.SetDefault("Increases speed.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 7, 0, 0);
            item.rare = Rarity.Green;
            item.defense = 6; // The Defence value for this piece of armour.
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 2.45f;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 27);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
