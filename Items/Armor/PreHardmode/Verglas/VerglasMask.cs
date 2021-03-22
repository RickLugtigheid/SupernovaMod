using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Verglas
{
    [AutoloadEquip(EquipType.Head)]
    public class VerglasMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("VerglasMask");
            Tooltip.SetDefault("Pure melee helmet" +
                "\n The cold protects you from lava for a short time");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 14, 0, 0);
            item.rare = Rarity.Orange;
            item.defense = 9; // The Defence value for this piece of armour.
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
            player.meleeDamage += 0.12f;
            if(player.statLife <= 200)
            {
                player.statDefense += 3;
            }
            player.setBonus = "Shoots sharts of ice when hit." +
                "\n Increases Defence when under 200 health" +
                "\n Increases melee damage";

            //player.GetModPlayer<Supernova>().VerglasArmour = true;
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
