using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class ForbiddenDiscProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 780;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Throwing;
        }

        public override void AI()
        {
            int dustID = Dust.NewDust(Projectile.Center, Projectile.width / 2, Projectile.height / 2, DustID.Sandnado, Projectile.velocity.X, Projectile.velocity.Y);
            Main.dust[dustID].noGravity = true;
            base.AI();
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			// Spawn dust
			//
			for (int x = 0; x < 8; x++)
			{
				int dust = Dust.NewDust(target.Center, 25, 25, DustID.Sandnado, Main.rand.Next(-3, 3), -Main.rand.Next(-3, 3), 0, default, Main.rand.NextFloat(.75f, 1));
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= Main.rand.NextFloat(1, 1.2f);
			}
		}
	}
}
