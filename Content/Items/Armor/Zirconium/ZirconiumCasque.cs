using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Items.Armor.Zirconium
{
    [AutoloadEquip(EquipType.Head)]
    public class ZirconiumCasque : ZirconiumHelmet
	{
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Zirconium Casque");
            // Tooltip.SetDefault("5% increased ranged damage");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.defense = 3; // The Defence value for this piece of armour.
        }

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Ranged) += .05f;
		}
    }
}
