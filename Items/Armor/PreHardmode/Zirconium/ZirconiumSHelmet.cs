using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Zirconium
{
    [AutoloadEquip(EquipType.Head)]
    public class ZirconiumSHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Helmet");
            //Tooltip.SetDefault("+2% change to not ConsumeAmmo");
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

        public override void UpdateArmorSet(Player player)
        {
            player.thrownDamage += 0.4f;
            player.setBonus = "+ 4% summon damage." +
                "\n the more health you have the more minion slots you have.";
            if (player.statLife >= 150)
            {
                player.maxMinions++;
            }
            if (player.statLife >= 350)
            {
                player.maxMinions++;
            }
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
