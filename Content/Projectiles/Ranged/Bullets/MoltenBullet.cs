﻿using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SupernovaMod.Content.Projectiles.Ranged.Bullets
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
            Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.penetrate = 2;
			Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            //this make that the projectile faces the right way
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(Main.rand.NextBool() ? BuffID.OnFire : BuffID.OnFire3, crit ? 240 : 120);
        }
    }
}
