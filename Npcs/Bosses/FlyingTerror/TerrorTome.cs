using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
    public class TerrorTome : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Tome");
        }
        int i;
        public override void SetDefaults()
        {
            item.damage = 14;
            item.crit = 14;
            item.magic = true;          //this make the item do magic damage
            item.width = 24;
            item.height = 28;
            item.useTime = 19;
            item.useAnimation = 19;
            item.useStyle = 5;        //this is how the item is holded
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.buyPrice(0, 7, 0, 0);
            item.rare = 6;
            item.mana = 4;             //mana use
            item.UseSound = SoundID.Item21;            //this is the sound when you use the item
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("FirendlyTerrorProj");  //this make the item shoot your projectile
            item.shootSpeed = 10f;    //projectile speed when shoot
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            i++;
            if (i >= 5)
            {
                item.shoot = 585;
                i = 0;
            }
            else
            {
                item.shoot = mod.ProjectileType("ProTerror");
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("TerrorWing"));
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}