using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.VFX
{
    public class Shockwave : ModProjectile
    {
        public override string Texture => "SupernovaMod/Assets/Textures/InvisibleProjectile";

        public int rippleCount = 3;
        public int rippleSize = 5;
        public int rippleSpeed = 30;
        public float distortStrength = 200;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = short.MaxValue;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 48;
            Projectile.height = 62;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 999;
        }

        public override void AI()
        {
            if (Projectile.ai[2] == 0)
            {
                Projectile.ai[2] = 150;
            }

            Projectile.timeLeft++; // Prevent despawning
            Projectile.ai[2]++;

            if (Projectile.ai[2] > 150 && Main.netMode != NetmodeID.Server)
            {
                if (Filters.Scene["Shockwave"].IsActive())
                {
                    float progress = (180 - Projectile.ai[2]) / 40;
                    Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
                }
                else
                {
                    Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(Projectile.Center);
                }
            }
            if (Projectile.ai[2] > 180)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                {
                    Filters.Scene["Shockwave"].Deactivate();
                }
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
