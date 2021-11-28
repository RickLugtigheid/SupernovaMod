using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

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
			item.damage = 22;
			item.melee = true;
            item.crit = 4;
            item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.knockBack = 2.5f;
			item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = Rarity.Orange;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                //Emit dusts when the sword is swung 
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Blood, Scale: 1.4f);
            }
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
