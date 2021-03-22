using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Projectiles
{
    public class PoisonousYoYo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Poisonous YoYo");
        }
        int Timer;
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = 99; // All Yoyos use the style 99.
            projectile.friendly = true; // Doesn't harm NPCs
            projectile.penetrate = -1; // -1 = Will not go through enemy.
            projectile.melee = true; // The projectile is a melee projectile.
            projectile.scale = 1f; // The scale of the projectile. 2f is double size. 0.5f is half size.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 4.7f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 330f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 15f;
        }
        public override void AI()
        {
            if (projectile.ai[0] == 1)
                Shoot();

            if (projectile.ai[0] > 35)
                projectile.ai[0] = 0;
        }
        void Shoot()
        {
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X += 4, projectile.velocity.Y += 4, ProjectileID.HornetStinger, (int)((double)90 * 0.1), 3 * 1, projectile.owner, 1, 0);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 170);
        }
    }
}
