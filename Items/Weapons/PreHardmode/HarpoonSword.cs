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
    public class HarpoonSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpoon Blade");
            Tooltip.SetDefault("Right click to shoots a harpoon at your enemies.");
            // Tooltip2.SetDefault("On Hit it lets the enemey bleed.");
            Item.staff[item.type] = true;
        }
        public override void SetDefaults()
		{
            item.damage = 24;
            item.melee = true;
            item.crit = 4;
            item.width = 40;
			item.height = 40;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 1;
			item.value = 10000;
			item.rare = Rarity.Orange;
            item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // Right Click function
            {
                item.melee = false;
                item.shoot = 23;
                item.damage = 28;
                item.shootSpeed = 30f;
                item.useStyle = 5;
                item.knockBack = 1;

                item.UseSound = SoundID.Item10;
            }
            else // Default Left Click
            {
                item.melee = true;
                item.useStyle = 1;
                item.knockBack = 7;
                item.damage = 31;
                // Left Click has no projectile
                item.shootSpeed = 0f;
                item.shoot = 0;
                item.UseSound = SoundID.Item1;
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 7);
            recipe.anyIronBar = true;
            recipe.AddIngredient(ItemID.Harpoon);
            recipe.AddIngredient(ItemID.SharkFin);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
