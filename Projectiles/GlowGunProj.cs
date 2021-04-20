using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class GlowGunProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Projectile");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = -1;
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
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            // Vanilla explosions do less damage to Eater of Worlds in expert mode, so for balance we will too.
            if (Main.expertMode)
            {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                {
                    damage /= 5;
                }
            }
        }
		public override void Kill(int timeLeft)
        {
            projectile.tileCollide = false;
            projectile.width = 100;
            projectile.height = 100;
            projectile.Size *= 10;

            Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item24, (int)position.X, (int)position.Y);

            // Truffle Explosion effect
            for (int j = 0; j <= Main.rand.Next(3, 7); j++)
            {
                Vector2 vel = projectile.velocity.RotatedByRandom(360);
                Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X * .06f, vel.Y * .06f, ProjectileID.MushroomSpray, (int)(projectile.damage * .75f), 3 * 1, projectile.owner, 1, 0);
            }
        }
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(projectile.position, projectile.width * 2, projectile.height * 2, 41, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 20, default(Color), Main.rand.NextFloat(.6f, 1.75f));
            Main.dust[DustID2].noGravity = true;
        }
	}
}
