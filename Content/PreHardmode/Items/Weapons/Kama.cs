using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Content.PreHardmode.Items.Weapons
{
	public class Kama: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kama");
            Tooltip.SetDefault("The kama is a traditional Japanese farming implement similar to a sickle used for reaping crops and also employed as a weapon.");
        }
		public override void SetDefaults()
		{
			Item.damage = 14;
            Item.crit = 4;
            Item.width = 40;
			Item.height = 40;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
            Item.useTurn = true;
            Item.autoReuse = true;

			Item.DamageType = DamageClass.Melee;
		}
	}
}
