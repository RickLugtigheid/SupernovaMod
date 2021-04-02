using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Projectiles
{
    public class MoltenBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Molten Bullet");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.penetrate = Main.rand.Next(1, 4);
            projectile.CloneDefaults(ProjectileID.Bullet);
            aiType = ProjectileID.Bullet; 
        }

        public override void AI()
        {
            //this make that the projectile faces the right way
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.penetrate = Main.rand.Next(1, 4);
            target.AddBuff(BuffID.OnFire, 120);
        }
    }
}
