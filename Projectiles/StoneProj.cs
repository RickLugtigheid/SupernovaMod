using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Supernova.Projectiles
{
    public class StoneProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("A Stone");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 1;
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
            int item = 0;
            if (Main.rand.NextFloat() < 0.1f) // This handles the rate at which the item will drop. 1f == 100%
                item = Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, ModContent.ItemType<Items.Weapons.PreHardmode.TrowingStone>(), 1, false, 0, false, false);
            else
			{
                // Spawn dust on hit
                for (int i = 0; i <= Main.rand.Next(7, 14); i++)
                {
                    int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Stone, projectile.velocity.X * 0.05f, -projectile.velocity.Y * 0.2f, 20, default(Color), Main.rand.NextFloat(.7f, 1.65f));
                    //Main.dust[dustID].velocity *= .3f;
                    Main.dust[dustID].noGravity = true;
                }

                // Break sound
                Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
            }

            if (Main.netMode == 1 && item >= 0)
                NetMessage.SendData(MessageID.KillProjectile);
        }

        // Optional Section 

        public float isStickingToTarget
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        public float targetWhoAmI
        {
            get { return projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            projectile.ai[0] = 1f;
            projectile.ai[1] = (float)target.whoAmI;
            projectile.velocity = (target.Center - projectile.Center) * 0.75f;
            projectile.netUpdate = true;
            target.AddBuff(169, 900, false);

            int maxStickingJavelins = 6;
            Point[] stickingJavelins = new Point[maxStickingJavelins];
            int javelinIndex = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != projectile.whoAmI &&
                    currentProjectile.active &&
                    currentProjectile.owner == Main.myPlayer &&
                    currentProjectile.type == projectile.type &&
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
            if (projectile.alpha > 0)
            {
                projectile.alpha -= alphaReducation;
            }

            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }

            if (projectile.ai[0] == 0f)
            {
                projectile.ai[1] += 1f;
                if (projectile.ai[1] >= maxTicks)
                {
                    float velXmult = 0.98f;
                    float velYmult = 0.38f;
                    projectile.ai[1] = maxTicks;
                    projectile.velocity.X = projectile.velocity.X * velXmult;
                    projectile.velocity.Y = projectile.velocity.Y + velYmult;
                }

                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            }
            // Optional Section 
            if (projectile.ai[0] == 1f)
            {
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                int aiFactor = 15;
                bool killProj = false;
                bool hitEffect = false;
                projectile.localAI[0] += 1f;
                hitEffect = projectile.localAI[0] % 30f == 0f;
                int projTargetIndex = (int)projectile.ai[1];
                if (projectile.localAI[0] >= (float)(60 * aiFactor))
                {
                    killProj = true;
                }
                else if (projTargetIndex < 0 || projTargetIndex >= 200)
                {
                    killProj = true;
                }
                else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
                {
                    projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
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
                    projectile.Kill();
                }
            }

        }
    }
}
