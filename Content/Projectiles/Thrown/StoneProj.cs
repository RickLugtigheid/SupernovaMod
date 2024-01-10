using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SupernovaMod.Common.Systems;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class StoneProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("A Stone");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
			Projectile.DamageType = GlobalModifiers.DamageClass_ThrowingRanged;
			Projectile.penetrate = 1;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return projHitbox.Intersects(targetHitbox);
        }

        public override void OnKill(int timeLeft)
        {
            int item = 0;
            if (Main.rand.NextFloat() < 0.1f) // This handles the rate at which the item will drop. 1f == 100%
            {
                item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.position, ModContent.ItemType<Items.Weapons.Throwing.TrowingStone>());
            }
            else
            {
                // Spawn dust on kill
                //
                for (int i = 0; i <= Main.rand.Next(7, 14); i++)
                {
                    int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X * 0.05f, -Projectile.velocity.Y * 0.2f, 20, default, Main.rand.NextFloat(.7f, 1.65f));
                    //Main.dust[dustID].velocity *= .3f;
                    Main.dust[dustID].noGravity = true;
                }

                // Break sound
                //SoundEngine.PlaySound(0, (int)Projectile.position.X, (int)Projectile.position.Y, 1, 1f, 0f);
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }

            if (Main.netMode == 1 && item >= 0)
                NetMessage.SendData(MessageID.KillProjectile);
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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Projectile.ai[0] = 1f;
            Projectile.ai[1] = target.whoAmI;
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
                    currentProjectile.ai[1] == target.whoAmI)
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
            {
                Projectile.alpha -= alphaReducation;
            }

            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }

            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= maxTicks)
                {
                    float velXmult = 0.98f;
                    float velYmult = 0.38f;
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
                if (Projectile.localAI[0] >= 60 * aiFactor)
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
