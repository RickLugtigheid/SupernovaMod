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
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
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
        public int GetDamage() => isEoW ? projectile.damage / 4 : projectile.damage / 2;
        private bool isEoW = false;
		public override bool? CanHitNPC(NPC target)
		{
            if (target.type == NPCID.EaterofWorldsBody || target.type == NPCID.EaterofWorldsHead) isEoW = true;
			return base.CanHitNPC(target);
		}
		public override void Kill(int timeLeft)
        {
            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item24, (int)position.X, (int)position.Y);

            int Type = 7;
            const int ShootDirection = 3;
            int ShootDamage = GetDamage();
            float ShootKnockback = 1.2f;

            // Spore explosion
            if(Main.rand.NextBool(2))
			{
                Projectile.NewProjectile(position.X, position.Y, -ShootDirection * 3, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection * 3, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, 0, ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, 0, -ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, -ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, -ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            }
            else
			{
                Projectile.NewProjectile(position.X, position.Y, -ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection, 0, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, 0, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, 0, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, -ShootDirection * 3, -ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection * 3, -ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, -ShootDirection * 3, ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection * 3, ShootDirection * 3, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            }
        }
		public override void AI()
        {
            //this is projectile dust
            for(int i = 0; i <= 2; i++)
			{
                int DustID2 = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height * 2, 3, projectile.velocity.X * .05f, projectile.velocity.Y * .05f, 20, default(Color), 1.5f);
                Main.dust[DustID2].noGravity = true;
            }
        }
    }
}
