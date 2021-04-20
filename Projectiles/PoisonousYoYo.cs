using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

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
            if (Main.rand.NextBool(2))
            {
                int DustID2 = Dust.NewDust(projectile.position, projectile.width + 2, projectile.height + 2, 44, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 80, default(Color), 1);
                Main.dust[DustID2].noGravity = true;
                DustID2 = Dust.NewDust(projectile.position, projectile.width + 2, projectile.height + 2, 46, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 80, default(Color), 1.4f);
                Main.dust[DustID2].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 170);
        }
    }
}
