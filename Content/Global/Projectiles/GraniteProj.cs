using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Projectiles
{
    public class GraniteProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chunk of Granite");
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 0;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 1;                       //this is the projectile penetration
            //Main.projFrames[projectile.type] = 4;           //this is projectile frames
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;                 //this make that the projectile does not go thru walls
            Projectile.ignoreWater = false;
        }
        public override void Kill(int timeLeft)
        {
            for (int x = 0; x <= 7; x++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Granite, -Projectile.velocity.X, -Projectile.velocity.Y, 80, default(Color), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= Main.rand.NextFloat(.2f, .4f);
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.Granite, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 70, default(Color), 0.7f);
            Main.dust[DustID2].noGravity = true;
            //this make that the projectile faces the right way
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 9.57f;
            Projectile.localAI[0] += 1f;
        }
    }
}

