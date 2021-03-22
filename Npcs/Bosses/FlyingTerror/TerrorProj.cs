using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Supernova.Npcs.Bosses.FlyingTerror
{
    public class TerrorProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 100;
            projectile.light = 0.25f;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
            Vector2 usePos = projectile.position;
            Vector2 rotVector = (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;
        }

        public override void AI()
        {
            if (projectile.localAI[0] > 100f) //projectile time left before disappears
            {
                projectile.Kill();
            }

            //Lighting.AddLight(projectile.Center, new Vector3(205, 0, 255));   //this is the light colors

            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 2, projectile.height + 2, mod.DustType("TerrorDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 37, default(Color), 2.7f);

            Main.dust[dust].noGravity = true; //this make so the dust has no gravity
            Main.dust[dust].velocity *= 4f;
        }
    }
}
