using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class GlowGunProj : ModProjectile
    {
        public const float MAX_DEVIATION_ANGLE = 1.2f;
        public const float HOMING_RANGE = 150;
        public const float HOMING_ANGLE = .83f;
        public const int TIME_LEFT = 210;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Glowing Projectile");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = TIME_LEFT;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            AIType = 521;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // Vanilla explosions do less damage to Eater of Worlds in expert mode, so for balance we will too.
            //
            if (Main.expertMode)
            {
                if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
                {
                    modifiers.FinalDamage /= 5;
				}
            }
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 position = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item24, position);

            // Truffle Explosion effect
            for (int j = 0; j <= Main.rand.Next(3, 7); j++)
            {
                Vector2 velocity = (Projectile.velocity * Main.rand.Next(4, 6)).RotatedByRandom(360);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y, velocity.X, velocity.Y, ProjectileID.Mushroom, (int)(Projectile.damage * .75f), 3 * 1, Projectile.owner, 1, 0);
            }
        }
        public override void AI()
        {
            //this make that the projectile faces the right way
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Projectile.localAI[0] += 1f;

            for (int i = 0; i < 2; i++)
            {
                int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GlowingMushroom, Projectile.velocity.X, Projectile.velocity.Y, 100, default, Scale: 1.5f);
                Main.dust[dustId].noGravity = true;
            }
        }

        internal Color ColorFunction(float completionRatio)
        {
            float fadeOpacity = (float)Math.Sqrt((double)(1f - completionRatio));
            return Color.DeepSkyBlue.MultiplyRGB(Color.Blue) * fadeOpacity;
        }

        public override bool PreDrawExtras()
        {
            return base.PreDrawExtras();
        }
    }
}
