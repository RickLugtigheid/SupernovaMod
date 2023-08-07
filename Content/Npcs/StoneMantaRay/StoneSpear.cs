using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Npcs.StoneMantaRay
{
    public class StoneSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stone Spear");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 6000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.light = 0.5f;
            //projectile.extraUpdates = 1;
            AIType = ProjectileID.Boulder;
        }

        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width * 2, Projectile.height * 2, DustID.Stone, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 1.0f, 80, default, 1.6f);
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= 0.3f;
            }
            base.AI();
        }
    }
}
