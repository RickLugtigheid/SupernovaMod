using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class GraniteProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunk of Granite");
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 0;
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
        public override void Kill(int timeLeft)
        {
            for (int x = 0; x <= 7; x++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Granite, -projectile.velocity.X, -projectile.velocity.Y, 80, default(Color), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= Main.rand.NextFloat(.2f, .4f);
            }
        }
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(projectile.position, projectile.width + 2, projectile.height + 2, DustID.Granite, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 70, default(Color), 0.7f);
            Main.dust[DustID2].noGravity = true;
            //this make that the projectile faces the right way
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 9.57f;
            projectile.localAI[0] += 1f;
        }
    }
}

