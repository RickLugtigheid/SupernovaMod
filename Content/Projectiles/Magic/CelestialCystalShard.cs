using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class CelestialCystalShard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CrystalShard);
            Projectile.width = 10;
            Projectile.height = 22;
            Projectile.scale = .75f;
            Projectile.DamageType = DamageClass.Magic;
            AIType = ProjectileID.CrystalShard;
        }
	}
}
