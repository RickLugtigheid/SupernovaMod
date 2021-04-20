using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Supernova.Projectiles
{
    public class Lightning : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning");
            Main.projFrames[projectile.type] = 3;
        }
        int Timer;
        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 3;
            projectile.tileCollide = true;
            projectile.timeLeft = 800;  //The amount of time the projectile is alive for
        }

        public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.modProjectile != null)
                return projectile.modProjectile.OnTileCollide(oldVelocity);
            return true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors

            if(Timer >= 12)
            {
                NPC target;
                float targetDist = 1000;
                for (int i = 0; i < 200; i++)
                {
                    target = Main.npc[i];
                    //If the npc is hostile
                    if (!target.friendly)
                    {
                        //Get the shoot trajectory from the projectile and target
                        float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                        float shootToY = target.position.Y - projectile.Center.Y;
                        float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                        //If the distance between the live targeted npc and the projectile is less than 480 pixels
                        if (distance < 480 && !target.friendly && target.active && distance < targetDist)
                        {
                            targetDist = distance;

                            //Divide the factor, 3f, which is the desired velocity
                            distance = 3f / distance;

                            //Multiply the distance by a multiplier if you wish the projectile to have go faster
                            shootToX *= distance * 8;
                            shootToY *= distance * 8;

                            //Set the velocities to the shoot values
                            projectile.velocity.X = shootToX;
                            projectile.velocity.Y = shootToY;
                        }
                    }
                }
            }
            else
                Timer++;

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 57, projectile.velocity.X * 0.01f, projectile.velocity.Y * 0.01f, 10, default(Color), 1.5f);
            Main.dust[dust].noGravity = true;
                dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 57, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 5, default(Color), 2f);
            Main.dust[dust].noGravity = true;

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.80f;
            projectile.localAI[0] += 1f;
            projectile.frameCounter++;

            if (projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;

                projectile.frame = (projectile.frame + 1) % 3;
            }
        }
    }
}
