using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Items.Weapons.PreHardmode
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
			item.damage = 14;
			item.melee = true;
            item.crit = 4;
            item.width = 40;
			item.height = 40;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = 1;
			item.knockBack = 2;
            item.value = Item.buyPrice(0, 2, 50, 0);
            item.rare = Rarity.Green;
			item.UseSound = SoundID.Item1;
            item.useTurn = true;
            item.autoReuse = true;
        }
    }
}
