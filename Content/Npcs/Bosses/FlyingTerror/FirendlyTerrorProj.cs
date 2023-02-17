using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Supernova.Content.Npcs.Bosses.FlyingTerror
{
    public class FirendlyTerrorProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 23;
            Projectile.height = 23;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.light = .5f;
        }

        public override void AI()
        {
            //this is projectile dust
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);   //this is the light colors
            int dust = Dust.NewDust(Projectile.position, Projectile.width + 5, Projectile.height + 5, ModContent.DustType<Dusts.TerrorDust>(), Projectile.velocity.X * .05f, Projectile.velocity.Y * .05f, 40, default, 1.5f);

            Main.dust[dust].noGravity = true; //this make so the dust has no gravity
            //this make that the projectile faces the right way
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 0.80f;
            Projectile.localAI[0] += 1f;
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);   //this is the light colors

            if (Projectile.localAI[0] > 130f) //projectile time left before disappears
                Projectile.Kill();
        }
        public override void Kill(int timeLeft)
        {
            for (int x = 0; x <= 10; x++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, ModContent.DustType<Dusts.TerrorDust>(), -Projectile.velocity.X, -Projectile.velocity.Y, 80, default, 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].velocity *= Main.rand.NextFloat(.5f, 1.25f);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 30);
        }
    }
}
