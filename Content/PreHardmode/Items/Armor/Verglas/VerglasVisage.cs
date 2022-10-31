using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Armor.Verglas
{
    [AutoloadEquip(EquipType.Head)]
    public class VerglasVisage : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Verglas Visage");
            Tooltip.SetDefault("A Zirconium helmet for the ranged and throwing classes" +
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
            player.setBonus = "Increases thrown and ranged damage by 8%\nIncreases velocity by 5%";
            //player.GetModPlayer<TheGalacticaModPlayer>().VerglasArmour = true;
            player.GetDamage(DamageClass.Ranged) += .08f;
            player.GetDamage(DamageClass.Throwing) += .08f;
            player.ThrownVelocity += .05f;
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
