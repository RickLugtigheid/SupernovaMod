using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Armor.Verglas
{
    [AutoloadEquip(EquipType.Head)]
    public class VerglasHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Verglas Helmet");
            Tooltip.SetDefault("A Verglas helmet for the melee class" +
                "\n The cold protects you from lava for a short time");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 14, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 9; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<VerglasBreastplate>() && legs.type == ModContent.ItemType<VerglasBoots>();

        public override void UpdateEquip(Player player)
        {
            player.lavaMax += 210;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Melee) += .08f;
            if (player.statLife <= 200)
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
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
