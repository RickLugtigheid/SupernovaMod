using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Items.Armor.Verglas
{
    [AutoloadEquip(EquipType.Head)]
    public class VerglasVisage : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Verglas Visage");
            // Tooltip.SetDefault("10% increased ranged damage, 20% chance not to consume ammo");
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
			player.GetDamage(DamageClass.Ranged) += .1f;
            player.ammoCost80 = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "The cold protects you from lava for a short time\nThe cold generates a layer of ice that makes the first hit deal 25% less damage, after that the layer of ice will break and regenrate after 10 seconds.";
			player.lavaMax += 210;
			player.GetModPlayer<ArmorPlayer>().coldArmor = true;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowSubtle = true;
			if (!player.HasBuff<Buffs.Cooldowns.ColdArmorCooldown>())
			{
				player.armorEffectDrawOutlines = true;
			}
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
