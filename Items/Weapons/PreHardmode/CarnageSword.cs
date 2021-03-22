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
    public class CarnageSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Carnage Sword");
        }
		public override void SetDefaults()
		{
			item.damage = 18;
			item.melee = true;
            item.crit = 8;
            item.width = 40;
			item.height = 40;
			item.useTime = 27;
			item.useAnimation = 27;
			item.useStyle = 1;
			item.knockBack = 4;
			item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = Rarity.Green;
			item.UseSound = SoundID.Item1;
            item.autoReuse = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BloodShards"), 8);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
