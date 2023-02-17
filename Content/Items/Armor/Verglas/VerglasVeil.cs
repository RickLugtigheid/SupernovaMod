using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Armor.Verglas
{
    [AutoloadEquip(EquipType.Head)]
    public class VerglasVeil : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Verglas Veil");
            Tooltip.SetDefault("A Zirconium helmet for the magic and summoner classes" +
                "\n The cold protects you from lava for a short time");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 14, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<VerglasBreastplate>() && legs.type == ModContent.ItemType<VerglasBoots>();

        public override void UpdateEquip(Player player)
        {
            player.lavaMax += 210;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Magic) += .08f;
            player.GetDamage(DamageClass.Summon) += .08f;
            player.statManaMax += 40;
            player.setBonus = "Increases max mana by 40\nIncreases magic and summon damage by 8%";
            //player.GetModPlayer<TheGalacticaModPlayer>().VerglasArmour = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
