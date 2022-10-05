using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Supernova.Content.Global.Projectiles
{
    public class Lightning : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning");
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 800;  //The amount of time the projectile is alive for
        }

        public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.ModProjectile != null)
                return projectile.ModProjectile.OnTileCollide(oldVelocity);
            return true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.15f) / 255f, ((255 - Projectile.alpha) * 0.45f) / 255f, ((255 - Projectile.alpha) * 0.05f) / 255f);   //this is the light colors

			if (Projectile.ai[1] >= 12)
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
                        float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                        float shootToY = target.position.Y - Projectile.Center.Y;
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
                            Projectile.velocity.X = shootToX;
                            Projectile.velocity.Y = shootToY;
                        }
                    }
                }
            }
            else
			{
                Projectile.ai[1]++;
			}

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.01f, Projectile.velocity.Y * 0.01f, 10, default(Color), 1.5f);
            Main.dust[dust].noGravity = true;
                dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 5, default(Color), 2f);
            Main.dust[dust].noGravity = true;

            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.80f;
            Projectile.localAI[0] += 1f;
            Projectile.frameCounter++;

            if (Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;

                Projectile.frame = (Projectile.frame + 1) % 3;
            }
        }
    }
}
