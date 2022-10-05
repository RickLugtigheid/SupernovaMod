using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.Global.Projectiles
{
    public class GlowGunProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Projectile");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;         
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;           
            Projectile.timeLeft = 600;                    
            Projectile.ignoreWater = true;      
            Projectile.tileCollide = true;      
            Projectile.extraUpdates = 1;                                           
            AIType = 521;
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
            Projectile.tileCollide = false;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.Size *= 10;

            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item24, position);

            // Truffle Explosion effect
            for (int j = 0; j <= Main.rand.Next(3, 7); j++)
            {
                Vector2 vel = Projectile.velocity.RotatedByRandom(360);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y, vel.X * .06f, vel.Y * .06f, ProjectileID.MushroomSpray, (int)(Projectile.damage * .75f), 3 * 1, Projectile.owner, 1, 0);
            }
        }
        public override void AI()
        {
            //this is projectile dust
            int DustID2 = Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, 41, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 20, default(Color), Main.rand.NextFloat(.6f, 1.75f));
            Main.dust[DustID2].noGravity = true;
        }
	}
}
