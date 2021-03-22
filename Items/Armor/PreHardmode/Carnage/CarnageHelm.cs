using Terraria;
using Terraria.ModLoader;

namespace Supernova.Items.Armor.PreHardmode.Carnage
{
    [AutoloadEquip(EquipType.Head)]
    public class CarnageHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Helmet");
            Tooltip.SetDefault("increases damage by 3%");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 7, 0, 0);
            item.rare = Rarity.Green;
            item.defense = 6; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BloodyBreastplate") && legs.type == mod.ItemType("BloodyBoots");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "When you get hit may Rage Giving you +10% crit \nReduces damage taken by 5%";
            player.GetModPlayer<SupernovaPlayer>().arCarnage = true;
            player.endurance += 0.05f;
        }
        public override void UpdateEquip(Player player)
        {
            player.allDamage += 0.03f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 23);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 12);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
