using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Projectiles.Magic
{
    public class ZirconProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zircon Shot");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 1;                       //this is the projectile penetration
            //Main.projFrames[projectile.type] = 4;           //this is projectile frames
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;

            if (Projectile.localAI[0] > 3f)
            {
                int num90 = 1;

                if (Projectile.localAI[0] > 50f)
                {
                    num90 = 2;
                }
                for (int num91 = 0; num91 < num90; num91++)
                {
                    int num92 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.ZirconDust>(), Projectile.velocity.X, Projectile.velocity.Y, 100, default, Scale: 1.5f);
                    Main.dust[num92].noGravity = true;
                    Dust expr_46AC_cp_0 = Main.dust[num92];
                    expr_46AC_cp_0.velocity.X = expr_46AC_cp_0.velocity.X * 0.3f;
                    Dust expr_46CA_cp_0 = Main.dust[num92];
                    expr_46CA_cp_0.velocity.Y = expr_46CA_cp_0.velocity.Y * 0.3f;
                    Main.dust[num92].noLight = true;
                }
                //this make that the projectile faces the right way
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
                Projectile.localAI[0] += 1f;
            }
        }


        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Dusts.ZirconDust>(), Projectile.oldVelocity.X * 0.7f, Projectile.oldVelocity.Y * 0.7f);
            }
        }
    }
}

