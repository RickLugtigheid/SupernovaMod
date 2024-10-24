using Terraria;
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
            int dust = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.Sandnado, Projectile.velocity.X, Projectile.velocity.Y, Scale: 1.25f);
            Main.dust[dust].noGravity = true;

            if (Main.rand.NextBool(6))
            {
                dust = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.YellowStarDust, Projectile.velocity.X, Projectile.velocity.Y, Scale: Main.rand.NextFloat(.7f, 2));
                Main.dust[dust].noGravity = true;
            }
            base.AI();
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            // Spawn dust
            //
            int dust;
			for (int x = 0; x < 8; x++)
			{
				dust = Dust.NewDust(Projectile.Center, 25, 25, DustID.Sandnado, 7 * hit.HitDirection, Main.rand.Next(-3, 3), 0, default, Main.rand.NextFloat(.75f, 1));
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= Main.rand.NextFloat(.5f, 1.5f);
			}
            for (int i = 0; i < 3; i++)
            {
                dust = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.YellowStarDust, Projectile.velocity.X, Projectile.velocity.Y, Scale: Main.rand.NextFloat(.7f, 2));
                Main.dust[dust].noGravity = true;
            }
        }
	}
}
