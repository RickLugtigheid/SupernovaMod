using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
    public class TerrorBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror Bomb");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.light = 0.5f;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors

            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 2, projectile.height + 2, mod.DustType("TerrorDust"), projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 80, default(Color), 3.7f);

            Main.dust[dust].noGravity = true; //this make so the dust has no gravity
            Main.dust[dust].velocity *= 0.05f;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, (int)projectile.position.X, (int)projectile.position.Y);
            if (projectile.owner == Main.myPlayer)
            {
                int num220 = 14;
                for (int num221 = 0; num221 < num220; num221++)
                {
                    // Create velocity for angle
                    Vector2 value17 = -Vector2
                        // Normalize so the velocity ammount of the projectile doesn't matter
                        .Normalize(projectile.velocity)
                        // Rotate by angle
                        .RotatedBy(MathHelper.ToRadians(360 / num220 * (num221-2)))
                        // Make the velocity 7.5
                        * 8;
                    
                    // Create a projectile for velocity
                    Projectile.NewProjectile(projectile.position.X, projectile.position.Y, value17.X, value17.Y, mod.ProjectileType("TerrorProj"), projectile.damage / 2, 1f, projectile.owner, 0, Main.rand.Next(-45, 1));
                }
            }
        }
    }
}
