using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Projectiles.Melee.Yoyos
{
    public class PoisonousYoYo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Poisonous YoYo");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 99; // All Yoyos use the style 99.
            Projectile.friendly = true; // Doesn't harm NPCs
            Projectile.penetrate = -1; // -1 = Will not go through enemy.
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f; // The scale of the projectile. 2f is double size. 0.5f is half size.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 4.7f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 330f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 15f;
        }
        public override void AI()
        {
            if (Main.rand.NextBool(2))
            {
                int DustID2 = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.JungleSpore, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 80, default, 1);
                Main.dust[DustID2].noGravity = true;
                DustID2 = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.Poisoned, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 80, default, 1.4f);
                Main.dust[DustID2].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 170);
        }
    }
}
