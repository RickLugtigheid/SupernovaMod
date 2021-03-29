using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Verglas
{
    [AutoloadEquip(EquipType.Head)]
    public class VerglasVisage : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Visage");
            Tooltip.SetDefault("Ranged and throwing helmet" +
                "\n The cold protects you from lava for a short time");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 14, 0, 0);
            item.rare = Rarity.Orange;
            item.defense = 7; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("VerglasBreastplate") && legs.type == mod.ItemType("VerglasBoots");
        }

        public override void UpdateEquip(Player player)
        {
            player.lavaMax += 210;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Shoots sharts of ice when hit." +
                "\n Increases thrown velocity and ranged damage";
            //player.GetModPlayer<TheGalacticaModPlayer>().VerglasArmour = true;
            player.rangedDamage += 0.1f;
            player.thrownDamage += 0.1f;
            player.thrownVelocity += 0.05f;

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
