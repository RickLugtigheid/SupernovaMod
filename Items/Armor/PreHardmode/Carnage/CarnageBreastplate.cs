using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Carnage
{
    // Added instread of AutoLoad
    [AutoloadEquip(EquipType.Body)]
    public class CarnageBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Breastplate"); // Set the name
            Tooltip.SetDefault("When your health is below 200 your crit change increases by 4%");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 7, 0, 0);
            item.rare = Rarity.Green;
            item.defense = 8; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("BloodyHelm") && legs.type == mod.ItemType("BloodyBoots");
        }

        public override void UpdateEquip(Player player)
        {
            if (player.statLife <= 200)
            {
                player.meleeCrit += 4;
                player.magicCrit += 4;
                player.rangedCrit += 4;
                player.thrownCrit += 4;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 30);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 12);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
