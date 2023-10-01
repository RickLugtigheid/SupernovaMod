using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace SupernovaMod.Content.Npcs.FlyingTerror
{
    public class TerrorBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terror Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);   //this is the light colors

            int dust = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, ModContent.DustType<Dusts.TerrorDust>(), Projectile.velocity.X * 0.45f, Projectile.velocity.Y * 0.45f, 80, default, 3);

            Main.dust[dust].noGravity = true; //this make so the dust has no gravity
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            if (Projectile.owner == Main.myPlayer)
            {
                int num220 = 14;
                for (int num221 = 0; num221 < num220; num221++)
                {
                    // Create velocity for angle
                    Vector2 value17 = -Vector2
                        // Normalize so the velocity ammount of the projectile doesn't matter
                        .Normalize(Projectile.velocity)
                        // Rotate by angle
                        .RotatedBy(MathHelper.ToRadians(360 / num220 * (num221 - 2)))
                        // Make the velocity 7.5
                        * 8;

                    // Create a projectile for velocity
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y, value17.X, value17.Y, ModContent.ProjectileType<TerrorProj>(), Projectile.damage / 2, 1f, Projectile.owner, 0, Main.rand.Next(-45, 1));
                }
            }
        }
    }
}
