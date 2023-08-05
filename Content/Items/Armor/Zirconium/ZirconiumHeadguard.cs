using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Items.Armor.Zirconium
{
    [AutoloadEquip(EquipType.Head)]
    public class ZirconiumHeadguard : ZirconiumHelmet
	{
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Zirconium Headguard");
            // Tooltip.SetDefault("5% increased magic and summon damage");
        }

        public override void SetDefaults()
        {
			base.SetDefaults();
			Item.defense = 2; // The Defence value for this piece of armour.
        }

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += .05f;
			player.GetDamage(DamageClass.Summon) += .05f;
		}
    }
}
