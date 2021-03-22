using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Supernova.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingerMissile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Harbinger Missile");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 45;
            projectile.aiStyle = 1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 6000;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.light = 0.2f;
            aiType = ProjectileID.DeathLaser;
        }
    }
}
