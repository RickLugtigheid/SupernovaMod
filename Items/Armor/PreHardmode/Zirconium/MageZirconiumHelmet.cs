using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Armor.PreHardmode.Zirconium
{
    [AutoloadEquip(EquipType.Head)]
    public class MageZirconiumHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zirconium Head Guard");
            Tooltip.SetDefault("A Zirconium helmet for magic class");
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
            player.magicDamage += 0.08f;
            player.setBonus = "+ 8% Magic damage.\n Your magic attacks have a 1 in 6 chance to spawn a mana star on hit";
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if(item.magic == true)
                if (Main.rand.Next(6) == 0)
                    Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, 184);
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
