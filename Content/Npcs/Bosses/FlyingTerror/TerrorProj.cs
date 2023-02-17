using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace SupernovaMod.Content.Npcs.Bosses.FlyingTerror
{
    public class TerrorProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror");
        }

        public override void SetDefaults()
        {
            Projectile.width = 23;
            Projectile.height = 23;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 250;
            //projectile.light = 25;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }

        public override void AI()
        {
            if (Projectile.localAI[0] > 100f) //projectile time left before disappears
                Projectile.Kill();

            //Lighting.AddLight(projectile.Center, new Vector3(205, 0, 255));   //this is the light colors
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, ModContent.DustType<Dusts.TerrorDust>(), Projectile.velocity.X, Projectile.velocity.Y, 80, default, 1.25f);

                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= .5f;
            }
        }
    }
}
