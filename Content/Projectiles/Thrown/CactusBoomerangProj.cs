using SupernovaMod.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class CactusBoomerangProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cactus Boomerang");

        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 780;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = GlobalModifiers.DamageClass_ThrowingMelee;
        }
    }
}
