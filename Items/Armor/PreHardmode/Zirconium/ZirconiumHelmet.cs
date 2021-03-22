using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Zirconium
{
    [AutoloadEquip(EquipType.Head)]
    public class ZirconiumHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Casque");
            Tooltip.SetDefault("A Zirconium helmet for ranged class");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 6, 0, 0);
            item.rare = Rarity.Green;
            item.defense = 2; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ZirconiumBreastplate") && legs.type == mod.ItemType("ZirconiumLeggings");
        }

        public override void UpdateEquip(Player player)
        {

        }

        public override void UpdateArmorSet(Player player)
        {
            player.rangedDamage += 0.08f;
            player.setBonus = "+ 8% ranged damage." +
                "\n 20% chance to not consume ammo";
            player.ammoBox = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("ZirconiumBar"), 22);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
