using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
    public class FirendlyTerrorProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 23;
            projectile.height = 23;
            projectile.aiStyle = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            //this is projectile dust
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors
            int dust = Dust.NewDust(projectile.position, projectile.width + 5, projectile.height + 5, mod.DustType("TerrorDust"), projectile.velocity.X * .05f, projectile.velocity.Y * .05f, 40, default(Color), 1.5f);

            Main.dust[dust].noGravity = true; //this make so the dust has no gravity
            //this make that the projectile faces the right way
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 0.80f;
            projectile.localAI[0] += 1f;
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors

            if (projectile.localAI[0] > 130f) //projectile time left before disappears
                projectile.Kill();
        }
		public override void Kill(int timeLeft)
		{
            for (int x = 0; x <= 10; x++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height * 2, mod.DustType("TerrorDust"), -projectile.velocity.X, -projectile.velocity.Y, 80, default(Color), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].velocity *= Main.rand.NextFloat(.5f, 1.25f);
            }
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 30);
        }
    }
}
