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
    public class VerglasSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Verglas Sword");
        }
		public override void SetDefaults()
		{
			item.damage = 30;
			item.melee = true;
            item.crit = 3;
            item.width = 40;
			item.height = 40;

			item.useStyle = 1;
			item.knockBack = 10;
            item.value = Item.buyPrice(0, 9, 47, 0); // Another way to handle value of item.
			item.rare = Rarity.Orange;
			item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useAnimation = 34;
			item.useTime = 34;
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (Main.rand.Next(3) == 1) type = ProjectileID.FrostBoltSword;
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner);
			return false;
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                //Emit dusts when the sword is swung 
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 92, Scale: 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
            target.AddBuff(BuffID.Frostburn, 80);
			base.OnHitNPC(player, target, damage, knockBack, crit);
		}
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("VerglasBar"), 8);
            recipe.AddIngredient(ItemID.IceBlade);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
