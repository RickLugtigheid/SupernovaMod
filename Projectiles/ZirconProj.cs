using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class ZirconProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zircon Shot");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.penetrate = 1;                       //this is the projectile penetration
            //Main.projFrames[projectile.type] = 4;           //this is projectile frames
            projectile.hostile = false;
            projectile.magic = true;                        //this make the projectile do magic damage
            projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
            projectile.ignoreWater = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(8) == 0)
            {
                target.AddBuff(BuffID.Frozen, 40);
            }
            if (Main.rand.Next(12) == 0)
            {
                target.AddBuff(BuffID.Frostburn, 10);
            }
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 20);
            }

            projectile.localAI[0] += 1f;

            if (projectile.localAI[0] > 3f)
            {
                int num90 = 1;

                if (projectile.localAI[0] > 50f)
                {
                    num90 = 2;
                }
                for (int num91 = 0; num91 < num90; num91++)
                {
                    int num92 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, mod.DustType("ZirconDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                    Main.dust[num92].noGravity = true;
                    Dust expr_46AC_cp_0 = Main.dust[num92];
                    expr_46AC_cp_0.velocity.X = expr_46AC_cp_0.velocity.X * 0.3f;
                    Dust expr_46CA_cp_0 = Main.dust[num92];
                    expr_46CA_cp_0.velocity.Y = expr_46CA_cp_0.velocity.Y * 0.3f;
                    Main.dust[num92].noLight = true;
                }
                //this make that the projectile faces the right way
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
                projectile.localAI[0] += 1f;
                projectile.alpha = (int)projectile.localAI[0] * 2;
            }
        }


        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod.DustType("ZirconDust"), projectile.oldVelocity.X * 0.7f, projectile.oldVelocity.Y * 0.7f);
            }
        }
    }
}

