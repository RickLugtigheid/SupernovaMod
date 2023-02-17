using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Projectiles.Magic
{
    public class CarnageScepterProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Scepter");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            AIType = 521;
        }
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 5f), Projectile.width + 2, Projectile.height + 2, ModContent.DustType<Dusts.BloodDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default, 0.9f);
            Main.dust[DustID2].noGravity = true;
        }

        private int _bounces;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            _bounces++;
            if (_bounces > 1) return true;

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X / 2;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y / 2;
            }
            return false; // return false because we are handling collision
        }
    }
}
