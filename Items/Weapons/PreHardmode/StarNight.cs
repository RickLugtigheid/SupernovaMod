using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace Supernova.Items.Weapons.PreHardmode
{
    public class StarNight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starry Night");
            Tooltip.SetDefault("This weapon charges up when you shoot." +
                "\n When it is fully charged it will shoot 6 deadly stars at your enemy");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        int I =5;
        bool once =true;

        public override void SetDefaults()
        {
            item.damage = 14;
            item.autoReuse = false;
            item.crit = 6;
            item.width = 16;
            item.height = 24;
            item.useAnimation = 10;
            item.useTime = 10;
            item.useStyle = 5; // Bow Use Style
            item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            item.value = Item.buyPrice(0, 2, 77, 0); // Another way to handle value of item.
            item.rare = Rarity.Green;
            item.UseSound = SoundID.Item5; // Sound for Bows
            item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            item.shoot = 1;
            item.shootSpeed = 16f;
            item.ranged = true; // For Ranged Weapon
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly) // or ProjectileID.WoodenArrowFriendly
            {
                if(I <= 6)
                {
                    item.autoReuse = true;
                    if (once == true)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            int dust = Dust.NewDust(player.position, player.width, player.height, 58);
                            Main.dust[dust].scale = 1.5f;
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].velocity *= 3f;
                            Main.dust[dust].velocity *= 3f;
                        }
                        once = false;
                    }
                    item.useAnimation = 4;
                    item.useTime = 4;
                    item.UseSound = SoundID.Item29; // Sound for Bows
                    type = 92; // or ProjectileID.FireArrow;
                }
                else
                {
                    item.useAnimation = 35;
                    item.useTime = 35;
                    item.UseSound = SoundID.Item5; // Sound for Bows
                    type = ProjectileID.WoodenArrowFriendly; // or ProjectileID.FireArrow;
                    item.autoReuse = false;
                    if (I >= 14)
                    {
                        I = 0;
                        once = true;
                    }
                }
                I++;
            }
            return true; // return true to allow tmodloader to call Projectile.NewProjectile as normal
        }
    }
}
