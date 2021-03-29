using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.StoneMantaRay
{
    public class StoneGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surgestone Glove");

        }

        public override void SetDefaults()
        {
            
            item.thrown = true; // Set this to true if the weapon is throwable.
            //item.maxStack = 1; // Makes it so the weapon stacks.
            item.damage = 17;
            item.crit = 3;
            item.knockBack = 2f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 23;
            item.useTime = 23;
            item.width = 32;
            item.height = 32;
            //item.consumable = true; // Makes it so one is taken from stack after use.
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = 5000;
            item.rare = 2;
            item.shootSpeed = 12f;
            item.shoot = mod.ProjectileType("TrowingStone");
        }

        public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
        {
            var posArray = new Vector2[num];
            float spread = (float)(angle * 0.0344532925);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = System.Math.Atan2(speedX, speedY);
            double randomAngle;
            for (int i = 0; i < num; ++i)
            {
                randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
            }
            return (Vector2[])posArray;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2[] speeds = randomSpread(speedX, speedY, 8, 6);
            for (int i = 0; i < 3; ++i)
            {
                Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
}
