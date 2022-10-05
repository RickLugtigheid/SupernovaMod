using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Supernova.Content.Global.Projectiles
{
    public class CarnageTrowingKniveProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Trowing Knive");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.penetrate = 2;
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return projHitbox.Intersects(targetHitbox);
        }

        public override void Kill(int timeLeft)
        {
            //SoundEngine.PlaySound(0, (int)Projectile.position.X, (int)Projectile.position.Y, 1, 1f, 0f);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            Vector2 usePos = Projectile.position;
            Vector2 rotVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;

            int item = 0;
            if (Main.rand.NextFloat() < 0.07f) // This handles the rate at which the new item will drop. 0.99f == highChance
            {
                // This will spawn a javelin drop at position of javelin and in the space of the hitbox
               // item = Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType<Items.Weapons.PreHardMode.TrowingStone>(), 1, false, 0, false, false);
            }

            if (Main.netMode == 1 && item >= 0)
            {
                NetMessage.SendData(MessageID.KillProjectile);
            }
        }

        // Optional Section 

        public float isStickingToTarget
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }

        public float targetWhoAmI
        {
            get { return Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.ai[0] = 1f;
            Projectile.ai[1] = (float)target.whoAmI;
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
            Projectile.netUpdate = true;
            target.AddBuff(169, 900, false);

            int maxStickingJavelins = 6;
            Point[] stickingJavelins = new Point[maxStickingJavelins];
            int javelinIndex = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != Projectile.whoAmI &&
                    currentProjectile.active &&
                    currentProjectile.owner == Main.myPlayer &&
                    currentProjectile.type == Projectile.type &&
                    currentProjectile.ai[0] == 1f &&
                    currentProjectile.ai[1] == (float)target.whoAmI)
                {
                    stickingJavelins[javelinIndex++] = new Point(i, currentProjectile.timeLeft);
                    if (javelinIndex >= stickingJavelins.Length)
                    {
                        break;
                    }
                }
            }

            if (javelinIndex >= stickingJavelins.Length)
            {
                int oldJavelinIndex = 0;
                for (int i = 1; i < stickingJavelins.Length; i++)
                {
                    if (stickingJavelins[i].Y < stickingJavelins[oldJavelinIndex].Y)
                    {
                        oldJavelinIndex = i;
                    }
                }
                Main.projectile[stickingJavelins[oldJavelinIndex].X].Kill();
            }
        }

        // End Optional Section 

        private const float maxTicks = 28f;
        private const int alphaReducation = 25;

        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= alphaReducation;

            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= maxTicks)
                {
                    float velXmult = 0.99f;
                    float velYmult = 0.24f;
                    Projectile.ai[1] = maxTicks;
                    Projectile.velocity.X = Projectile.velocity.X * velXmult;
                    Projectile.velocity.Y = Projectile.velocity.Y + velYmult;
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            }
            // Optional Section 
            if (Projectile.ai[0] == 1f)
            {
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                int aiFactor = 15;
                bool killProj = false;
                bool hitEffect = false;
                Projectile.localAI[0] += 1f;
                hitEffect = Projectile.localAI[0] % 30f == 0f;
                int projTargetIndex = (int)Projectile.ai[1];
                if (Projectile.localAI[0] >= (float)(60 * aiFactor))
                {
                    killProj = true;
                }
                else if (projTargetIndex < 0 || projTargetIndex >= 200)
                {
                    killProj = true;
                }
                else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
                {
                    Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                    if (hitEffect)
                    {
                        Main.npc[projTargetIndex].HitEffect(0, 1.0);
                    }
                }
                else
                {
                    killProj = true;
                }

                if (killProj)
                {
                    Projectile.Kill();
                }
            }

        }
    }
}
