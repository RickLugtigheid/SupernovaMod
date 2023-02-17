using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Content.Projectiles.Ranged.Bullets
{
    public class MoltenBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Molten Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.penetrate = Main.rand.Next(1, 4);
            Projectile.CloneDefaults(ProjectileID.Bullet);
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            //this make that the projectile faces the right way
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.penetrate = Main.rand.Next(1, 4);
            target.AddBuff(BuffID.OnFire, 120);
        }
    }
}
