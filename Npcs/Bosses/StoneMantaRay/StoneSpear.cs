using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Npcs.Bosses.StoneMantaRay
{
    public class StoneSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Spear");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 6000;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.light = 0.5f;
            //projectile.extraUpdates = 1;
            aiType = ProjectileID.Boulder;
        }

		public override void AI()
		{
            for (int i = 0; i < 5; i++)
			{
                int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width * 2, projectile.height * 2, DustID.Stone, projectile.velocity.X * 0.25f, projectile.velocity.Y * 1.0f, 80, default(Color), 1.6f);
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= 0.3f;
            }
            base.AI();
		}
	}
}
