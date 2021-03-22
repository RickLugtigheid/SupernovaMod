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
    public class BoneCrusher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bone Crusher");
            Tooltip.SetDefault("Throws your enemies a bone.");
        }
		public override void SetDefaults()
		{
			item.damage = 22;
			item.melee = true;
            item.crit = 3;
            item.width = 40;
			item.height = 40;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 7;
			item.value = Item.buyPrice(0, 12, 30, 0);
			item.rare = Rarity.Orange;
			item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shootSpeed = 7f;
            item.shoot = ProjectileID.BoneGloveProj;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BoneSword);
            recipe.AddIngredient(ItemID.Bone, 35);
            recipe.AddIngredient(mod.GetItem("BoneFragment"), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
