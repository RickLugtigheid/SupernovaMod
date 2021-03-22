using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Zirconium
{
    [AutoloadEquip(EquipType.Head)]
    public class MeleeZirconiumHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Helm");
            Tooltip.SetDefault("A Zirconium helmet for melee class");
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
            player.meleeDamage += 0.08f;
            if(player.statManaMax2 <= 50)
            {
                player.statDefense += 1;
            }
            if (player.statManaMax2 <= 100)
            {
                player.statDefense += 2;
            }
            if(player.statManaMax2 <= 200)
            {
                player.statDefense += 4;
            }
            player.setBonus = "+ 8% melee damage." +
                "\n The more mana you have the more defense you have";
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
