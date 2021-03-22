using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class ThornBal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ball of Throns");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            aiType = 521;
        }
        public override void Kill(int timeLeft)
        {
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item24, (int)position.X, (int)position.Y);

            int Type = 7;
            const int ShootDirection = 7;
            int ShootDamage = 11;
            float ShootKnockback = 1.2f;

            // Spore explosion
            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, 0, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, 0, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, -ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(position.X + 20, position.Y + 20, ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
        }
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 5f), projectile.width + 2, projectile.height + 2, 3, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), 1.2f);
            Main.dust[DustID2].noGravity = true;
        }
    }
}
